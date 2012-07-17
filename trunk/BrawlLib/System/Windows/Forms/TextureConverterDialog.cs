using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using BrawlLib.IO;
using System.ComponentModel;
using System.IO;

namespace System.Windows.Forms
{
    public partial class TextureConverterDialog : Form
    {
        private Bitmap _source, _preview, _indexed;
        private ColorInformation _colorInfo;
        private UnsafeBuffer _cmprBuffer;
        private ColorPalette _tempPalette;
        private bool _previewing = true, _updating = false;

        private string _imageSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ImageSource { get { return _imageSource; } set { _imageSource = value; } }

        private BRESNode _parent;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BRESNode ParentNode { get { return _parent; } }

        private TEX0Node _original;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TEX0Node TextureNode { get { return _original; } }

        private REFTEntryNode _originalREFT;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public REFTEntryNode REFTImgNode { get { return _originalREFT; } }

        private PLT0Node _originalPalette;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PLT0Node PaletteNode { get { return _originalPalette; } }

        private FileMap _textureData;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap TextureData { get { return _textureData; } }

        private FileMap _paletteData;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileMap PaletteData { get { return _paletteData; } }

        public TextureConverterDialog()
        {
            InitializeComponent();

            dlgOpen.Filter = "All Image Formats (*.png,*.tga,*.tiff,*.tif,*.bmp,*.jpg,*.jpeg,*.gif)|*.png;*.tga;*.tif;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tiff,*.tif)|*.tiff;*.tif|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif";

            foreach (WiiPixelFormat f in Enum.GetValues(typeof(WiiPixelFormat)))
                cboFormat.Items.Add(f);

            foreach (WiiPaletteFormat f in Enum.GetValues(typeof(WiiPaletteFormat)))
                cboPaletteFormat.Items.Add(f);

            foreach (QuantizationAlgorithm f in Enum.GetValues(typeof(QuantizationAlgorithm)))
                cboAlgorithm.Items.Add(f);

            cboAlgorithm.SelectedItem = QuantizationAlgorithm.MedianCut;
        }

        public DialogResult ShowDialog(IWin32Window owner, BRESNode parent)
        {
            _parent = parent;
            _original = null;
            _originalPalette = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, TEX0Node original)
        {
            _parent = null;
            _original = original;
            _originalPalette = original.GetPaletteNode();
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        public DialogResult ShowDialog(IWin32Window owner, REFTEntryNode original)
        {
            _parent = null;
            _originalREFT = original;
            _original = null;
            _originalPalette = null;
            _paletteData = _textureData = null;
            //cboFormat.Items.RemoveAt(9);
            //cboFormat.Items.RemoveAt(9);
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }
        new public DialogResult ShowDialog(IWin32Window owner)
        {
            _parent = null;
            _original = null;
            _originalPalette = null;
            _paletteData = _textureData = null;
            DialogResult = DialogResult.Cancel;
            try { return base.ShowDialog(owner); }
            catch (Exception x) { MessageBox.Show(x.ToString()); return DialogResult.Cancel; }
            finally { DisposeImages(); }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_imageSource == null)
            {
                if (!LoadImages())
                {
                    Close();
                    return;
                }
            }
            else if (!LoadImages(_imageSource))
            {
                Close();
                return;
            }

            if (_original != null)
            {
                _updating = true;

                cboFormat.SelectedItem = _original.Format;
                numLOD.Value = _original.LevelOfDetail;

                FixPaletteFields();

                if (_originalPalette != null)
                {
                    grpPalette.Enabled = true;
                    cboPaletteFormat.SelectedItem = _originalPalette.Format;
                    numPaletteCount.Value = _originalPalette.Colors;
                }

                _updating = false;
                UpdatePreview();
            }
            else if (_originalREFT != null)
            {
                _updating = true;
                cboFormat.SelectedItem = _originalREFT.TextureFormat;
                numLOD.Value = 1;
                //numLOD.Enabled = false;
                _updating = false;
                UpdatePreview();
            }
            else
                Recommend();
        }

        private bool LoadImages()
        {
            if (dlgOpen.ShowDialog(this) != DialogResult.OK)
                return false;
            return LoadImages(dlgOpen.FileName);
        }

        private bool LoadImages(string path)
        {
            DisposeImages();

            if (path.EndsWith(".tga"))
                _source = TGA.FromFile(path);
            else
                _source = (Bitmap)Bitmap.FromFile(path);

            //if (_source.PixelFormat != PixelFormat.Format32bppArgb)
            //    using (Bitmap bmp = _source)
            //        _source = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppArgb);

            //_source.SetResolution(96.0f, 96.0f);
            _preview = new Bitmap(_source.Width, _source.Height, PixelFormat.Format32bppArgb);

            txtPath.Text = path;
            lblSize.Text = String.Format("{0} x {1}", _source.Width, _source.Height);

            _colorInfo = _source.GetColorInformation();
            lblColors.Text = _colorInfo.ColorCount.ToString();
            lblTransparencies.Text = _colorInfo.AlphaColors.ToString();

            //Get max LOD
            int maxLOD = 1;
            for (int w = _source.Width, h = _source.Height; (w != 1) && (h != 1); w >>= 1, h >>= 1, maxLOD++) ;
            numLOD.Maximum = maxLOD;

            return true;
        }

        private void DisposeImages()
        {
            pictureBox1.Picture = null;
            if (_preview != null) { _preview.Dispose(); _preview = null; }
            if (_source != null) { _source.Dispose(); _source = null; }
            if (_indexed != null) { _indexed.Dispose(); _indexed = null; }
        }

        private void CopyPreview(Bitmap src)
        {
            Rectangle r = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData srcData = src.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = _preview.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Memory.Move(dstData.Scan0, srcData.Scan0, (uint)(srcData.Stride * src.Height));

            _preview.UnlockBits(dstData);
            src.UnlockBits(srcData);
        }

        private void UpdatePreview()
        {
            if (_source == null)
                return;

            //Copy source to preview
            //Rectangle r = new Rectangle(0, 0, _source.Width, _source.Height);
            //BitmapData srcData = _source.LockBits(r, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            //BitmapData dstData = _preview.LockBits(r, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            //Memory.Move(dstData.Scan0, srcData.Scan0, (uint)(srcData.Stride * _source.Height));

            //_preview.UnlockBits(dstData);
            //_source.UnlockBits(srcData);

            if (_cmprBuffer != null) { _cmprBuffer.Dispose(); _cmprBuffer = null; }
            if (_indexed != null) { _indexed.Dispose(); _indexed = null; }

            WiiPixelFormat format = (WiiPixelFormat)cboFormat.SelectedItem;
            switch (format)
            {
                case WiiPixelFormat.I4: //_preview.Clamp(WiiPixelFormat.I4); break;
                case WiiPixelFormat.I8: //_preview.Clamp(WiiPixelFormat.I8); break;
                case WiiPixelFormat.IA4: //_preview.Clamp(WiiPixelFormat.IA4); break;
                case WiiPixelFormat.IA8: //_preview.Clamp(WiiPixelFormat.IA8); break;
                case WiiPixelFormat.RGB565: //_preview.Clamp(WiiPixelFormat.RGB565); break;
                case WiiPixelFormat.RGB5A3: //_preview.Clamp(WiiPixelFormat.RGB5A3); break;
                case WiiPixelFormat.RGBA8:
                    {
                        CopyPreview(_source);
                        _preview.Clamp(format);
                        break;
                    }
                case WiiPixelFormat.CMPR:
                    {
                        CopyPreview(_source);
                        _cmprBuffer = TextureConverter.CMPR.GeneratePreview(_preview);
                        break;
                    }
                case WiiPixelFormat.CI4:
                case WiiPixelFormat.CI8:
                    {
                        _indexed = _source.Quantize((QuantizationAlgorithm)cboAlgorithm.SelectedItem, (int)numPaletteCount.Value, format, (WiiPaletteFormat)cboPaletteFormat.SelectedItem, null);
                        CopyPreview(_indexed);
                        break;
                    }
            }

            UpdateSize();

            if (_previewing)
                pictureBox1.Picture = _preview;
            else
                pictureBox1.Picture = _source;
        }
        private void UpdateSize()
        {
            if (_source == null)
                return;

            int w = _source.Width, h = _source.Height;
            int palSize = grpPalette.Enabled ? (((int)numPaletteCount.Value * 2) + 0x40) : 0;
            lblDataSize.Text = String.Format("{0:n0}B", TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem).GetMipOffset(ref w, ref h, (int)numLOD.Value + 1) + 0x40 + palSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadImages();
            UpdatePreview();
        }

        private void btnRecommend_Click(object sender, EventArgs e)
        {
            Recommend();
        }

        private void Recommend()
        {
            if ((_source == null) || (_updating))
                return;

            _updating = true;
            if (_colorInfo.IsGreyscale)
            {
                if (_colorInfo.ColorCount <= 16)
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.I4 : WiiPixelFormat.CI4;
                else if (_colorInfo.ColorCount <= 272)
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.I8 : WiiPixelFormat.IA8;
                else
                    cboFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPixelFormat.RGB565 : WiiPixelFormat.RGB5A3;
            }
            else
            {
                if (_colorInfo.ColorCount <= 16)
                    cboFormat.SelectedItem = WiiPixelFormat.CI4;
                else if (_colorInfo.ColorCount <= 272)
                    cboFormat.SelectedItem = WiiPixelFormat.CI8;
                else if (_colorInfo.AlphaColors <= 1)
                    cboFormat.SelectedItem = WiiPixelFormat.CMPR;
                else
                    cboFormat.SelectedItem = WiiPixelFormat.RGB5A3;
            }
            FixPaletteFields();

            _updating = false;
            UpdatePreview();
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((_source == null) || (_updating))
                return;

            _updating = true;

            FixPaletteFields();

            _updating = false;
            UpdatePreview();
        }

        private void FixPaletteFields()
        {
            switch ((WiiPixelFormat)cboFormat.SelectedItem)
            {
                case WiiPixelFormat.I4:
                case WiiPixelFormat.I8:
                case WiiPixelFormat.IA4:
                case WiiPixelFormat.IA8:
                case WiiPixelFormat.RGB565:
                case WiiPixelFormat.RGB5A3:
                case WiiPixelFormat.RGBA8:
                case WiiPixelFormat.CMPR:
                    grpPalette.Enabled = false; break;

                case WiiPixelFormat.CI4:
                    {
                        grpPalette.Enabled = true;
                        numPaletteCount.Maximum = 16;
                        numPaletteCount.Value = 16;
                        cboPaletteFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                        break;
                    }
                case WiiPixelFormat.CI8:
                    {
                        grpPalette.Enabled = true;
                        numPaletteCount.Maximum = 256;
                        numPaletteCount.Value = Math.Min(256, _colorInfo.ColorCount.Align(16));
                        cboPaletteFormat.SelectedItem = (_colorInfo.AlphaColors == 0) ? WiiPaletteFormat.RGB565 : WiiPaletteFormat.RGB5A3;
                        break;
                    }
            }
        }

        private void formatChanged(object sender, EventArgs e) { if ((_source != null) && (!_updating)) UpdatePreview(); }
        private void numLOD_ValueChanged(object sender, EventArgs e) { if ((_source != null) || (!_updating)) UpdateSize(); }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            if (_previewing = chkPreview.Checked)
                pictureBox1.Picture = _preview;
            else
                pictureBox1.Picture = _source;
        }

        private void btnCancel_Click(object sender, EventArgs e) { Close(); }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            TextureConverter format = TextureConverter.Get((WiiPixelFormat)cboFormat.SelectedItem);
            if (format.IsIndexed)
            {
                if (_originalREFT == null)
                    _textureData = format.EncodeTextureIndexed(_indexed, (int)numLOD.Value, (WiiPaletteFormat)cboPaletteFormat.SelectedItem, out _paletteData);
                else
                    _textureData = format.EncodeREFTTextureIndexed(_indexed, (int)numLOD.Value, (WiiPaletteFormat)cboPaletteFormat.SelectedItem);
            }
            else
            {
                if ((format.RawFormat == WiiPixelFormat.CMPR) && (_cmprBuffer != null))
                    if (_originalREFT == null)
                        _textureData = ((CMPR)format).EncodeTextureCached(_source, (int)numLOD.Value, _cmprBuffer);
                    else
                        _textureData = ((CMPR)format).EncodeREFTTextureCached(_source, (int)numLOD.Value, _cmprBuffer);
                else if (_originalREFT == null)
                    _textureData = format.EncodeTexture(_source, (int)numLOD.Value);
                else
                    _textureData = format.EncodeREFTTexture(_source, (int)numLOD.Value, WiiPaletteFormat.IA8, false);
            }

            if (_parent != null)
            {
                _original = _parent.CreateResource<TEX0Node>(Path.GetFileNameWithoutExtension(_imageSource));
                if (_paletteData != null)
                {
                    _originalPalette = _parent.CreateResource<PLT0Node>(_original.Name);
                    _originalPalette.Name = _original.Name;
                    _originalPalette.ReplaceRaw(_paletteData);
                }
                _original.ReplaceRaw(_textureData);
            }
            else if (_original != null)
            {
                if (_originalPalette != null)
                {
                    if (_paletteData != null)
                        _originalPalette.ReplaceRaw(_paletteData);
                    else
                    {
                        _originalPalette.Remove();
                        _originalPalette.Dispose();
                    }
                }
                else if (_paletteData != null)
                {
                    if ((_original.Parent == null) || (_original.Parent.Parent == null))
                    {
                        _paletteData.Dispose();
                        _paletteData = null;
                    }
                    else
                    {
                        _parent = _original.Parent.Parent as BRESNode;
                        _originalPalette = _parent.CreateResource<PLT0Node>(_original.Name);
                        _originalPalette.Name = _original.Name;
                        _originalPalette.ReplaceRaw(_paletteData);
                    }
                }
                _original.ReplaceRaw(_textureData);
            }
            else if (_originalREFT != null)
                _originalREFT.ReplaceRaw(_textureData);
            
            DialogResult = DialogResult.OK;
            Close();
        }


        #region Designer

        private CheckBox chkPreview;
        private GroupBox groupBox1;
        private Button btnRecommend;
        private NumericUpDown numLOD;
        private Label label5;
        private ComboBox cboFormat;
        private Label label4;
        private GroupBox groupBox2;
        private Label lblTransparencies;
        private Label lblColors;
        private Label lblSize;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnOkay;
        private Button btnCancel;
        private GroupBox grpPalette;
        private ComboBox cboAlgorithm;
        private Label label8;
        private NumericUpDown numPaletteCount;
        private Label label7;
        private ComboBox cboPaletteFormat;
        private Label label6;
        private GroupBox groupBox4;
        private GoodPictureBox pictureBox1;
        private Panel panel1;
        private Label lblDataSize;
        private TextBox txtPath;
        private Panel panel2;
        private Button button1;
        private Label label9;
        private OpenFileDialog dlgOpen;

        private void InitializeComponent()
        {
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numLOD = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRecommend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTransparencies = new System.Windows.Forms.Label();
            this.lblDataSize = new System.Windows.Forms.Label();
            this.lblColors = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpPalette = new System.Windows.Forms.GroupBox();
            this.cboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numPaletteCount = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cboPaletteFormat = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.GoodPictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLOD)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.grpPalette.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPaletteCount)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkPreview
            // 
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Location = new System.Drawing.Point(9, 15);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(66, 21);
            this.chkPreview.TabIndex = 0;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numLOD);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboFormat);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 108);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 71);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // numLOD
            // 
            this.numLOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numLOD.Location = new System.Drawing.Point(75, 42);
            this.numLOD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLOD.Name = "numLOD";
            this.numLOD.Size = new System.Drawing.Size(98, 20);
            this.numLOD.TabIndex = 9;
            this.numLOD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLOD.ValueChanged += new System.EventHandler(this.numLOD_ValueChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "MIP Levels:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFormat
            // 
            this.cboFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Location = new System.Drawing.Point(75, 15);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(98, 21);
            this.cboFormat.TabIndex = 7;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.cboFormat_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Format:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRecommend
            // 
            this.btnRecommend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecommend.Location = new System.Drawing.Point(75, 14);
            this.btnRecommend.Name = "btnRecommend";
            this.btnRecommend.Size = new System.Drawing.Size(98, 21);
            this.btnRecommend.TabIndex = 5;
            this.btnRecommend.Text = "Recommend";
            this.btnRecommend.UseVisualStyleBackColor = true;
            this.btnRecommend.Click += new System.EventHandler(this.btnRecommend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.lblTransparencies);
            this.groupBox2.Controls.Add(this.lblDataSize);
            this.groupBox2.Controls.Add(this.lblColors);
            this.groupBox2.Controls.Add(this.lblSize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 99);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 20);
            this.label9.TabIndex = 7;
            this.label9.Text = "Data Size:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTransparencies
            // 
            this.lblTransparencies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTransparencies.Location = new System.Drawing.Point(97, 51);
            this.lblTransparencies.Name = "lblTransparencies";
            this.lblTransparencies.Size = new System.Drawing.Size(76, 20);
            this.lblTransparencies.TabIndex = 5;
            this.lblTransparencies.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDataSize
            // 
            this.lblDataSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataSize.Location = new System.Drawing.Point(97, 71);
            this.lblDataSize.Name = "lblDataSize";
            this.lblDataSize.Size = new System.Drawing.Size(76, 20);
            this.lblDataSize.TabIndex = 6;
            this.lblDataSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColors
            // 
            this.lblColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColors.Location = new System.Drawing.Point(97, 31);
            this.lblColors.Name = "lblColors";
            this.lblColors.Size = new System.Drawing.Size(76, 20);
            this.lblColors.TabIndex = 4;
            this.lblColors.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSize
            // 
            this.lblSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSize.Location = new System.Drawing.Point(94, 11);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(79, 20);
            this.lblSize.TabIndex = 3;
            this.lblSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Transparent:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Colors:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Size:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOkay.Location = new System.Drawing.Point(8, 95);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(79, 23);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(92, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpPalette
            // 
            this.grpPalette.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPalette.Controls.Add(this.cboAlgorithm);
            this.grpPalette.Controls.Add(this.label8);
            this.grpPalette.Controls.Add(this.numPaletteCount);
            this.grpPalette.Controls.Add(this.label7);
            this.grpPalette.Controls.Add(this.cboPaletteFormat);
            this.grpPalette.Controls.Add(this.label6);
            this.grpPalette.Location = new System.Drawing.Point(3, 185);
            this.grpPalette.Name = "grpPalette";
            this.grpPalette.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grpPalette.Size = new System.Drawing.Size(179, 99);
            this.grpPalette.TabIndex = 5;
            this.grpPalette.TabStop = false;
            this.grpPalette.Text = "Palette";
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Location = new System.Drawing.Point(75, 68);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(98, 21);
            this.cboAlgorithm.TabIndex = 13;
            this.cboAlgorithm.SelectedIndexChanged += new System.EventHandler(this.formatChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "Algorithm:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPaletteCount
            // 
            this.numPaletteCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numPaletteCount.Increment = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.Location = new System.Drawing.Point(75, 42);
            this.numPaletteCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numPaletteCount.Minimum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.Name = "numPaletteCount";
            this.numPaletteCount.Size = new System.Drawing.Size(97, 20);
            this.numPaletteCount.TabIndex = 10;
            this.numPaletteCount.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPaletteCount.ValueChanged += new System.EventHandler(this.formatChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "Colors:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboPaletteFormat
            // 
            this.cboPaletteFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPaletteFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPaletteFormat.FormattingEnabled = true;
            this.cboPaletteFormat.Location = new System.Drawing.Point(75, 15);
            this.cboPaletteFormat.Name = "cboPaletteFormat";
            this.cboPaletteFormat.Size = new System.Drawing.Size(98, 21);
            this.cboPaletteFormat.TabIndex = 10;
            this.cboPaletteFormat.SelectedIndexChanged += new System.EventHandler(this.formatChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Format:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnOkay);
            this.groupBox4.Controls.Add(this.btnRecommend);
            this.groupBox4.Controls.Add(this.chkPreview);
            this.groupBox4.Controls.Add(this.btnCancel);
            this.groupBox4.Location = new System.Drawing.Point(3, 290);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(179, 124);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Picture = null;
            this.pictureBox1.Size = new System.Drawing.Size(379, 397);
            this.pictureBox1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.grpPalette);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(379, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 417);
            this.panel1.TabIndex = 9;
            // 
            // txtPath
            // 
            this.txtPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPath.Location = new System.Drawing.Point(0, 0);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(304, 20);
            this.txtPath.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtPath);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 20);
            this.panel2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(304, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 10;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TextureConverterDialog
            // 
            this.AcceptButton = this.btnOkay;
            this.ClientSize = new System.Drawing.Size(564, 417);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(0, 435);
            this.Name = "TextureConverterDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Texture Converter";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLOD)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.grpPalette.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPaletteCount)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

    }
}
