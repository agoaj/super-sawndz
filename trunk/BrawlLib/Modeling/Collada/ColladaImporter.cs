using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.IO;
using System.Windows.Forms;
using BrawlLib.Wii.Models;
using BrawlLib.Imaging;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Graphics;
using System.Globalization;

namespace BrawlLib.Modeling
{
    public unsafe partial class Collada : Form
    {
        public Collada() { InitializeComponent(); }

        private CheckBox fltVerts;
        private CheckBox fltNrms;
        private CheckBox fltUVs;
        private CheckBox addClrs;
        private CheckBox rmpClrs;
        private CheckBox forceTriangles;
        private CheckBox CCW;
        private CheckBox rmpMats;
        private Button button1;
        private Button button2;
        private Panel panel1;
        private Label label1;
        private ComboBox mdlType;

        private Label Status;
        public string _filePath;

        public void Say(string text)
        {
            Status.Text = text;
            Update();
        }
        
        public MDL0Node ShowDialog(string filePath)
        {
            mdlType.SelectedIndex = 0;
            if (base.ShowDialog() == DialogResult.OK)
            {
                panel1.Visible = false;
                Height = 63;
                UseWaitCursor = true;
                Text = "Please wait...";
                Show();
                Update();
                MDL0Node model = ImportModel(filePath);
                Close();
                return model;
            }
            else return null;
        }

        public class ImportOptions
        {
            public int _mdlType = 0;
            public bool _forceTriangles = true;
            public bool _fltVerts = false;
            public bool _fltNrms = false;
            public bool _fltUVs = false;
            public bool _addClrs = true;
            public bool _rmpClrs = true;
            public bool _rmpMats = true;
            public bool _forceCCW = false;
        }

        public static string Error;

        public static int index = 0;
        public MDL0Node ImportModel(string filePath)
        {
            MDL0Node model = new MDL0Node() { _name = Path.GetFileNameWithoutExtension(filePath), _origPath = filePath };
            model.InitGroups();

            model._importOptions._mdlType = mdlType.SelectedIndex;
            model._importOptions._forceTriangles = forceTriangles.Checked;
            model._importOptions._fltVerts = fltVerts.Checked;
            model._importOptions._fltNrms = fltNrms.Checked;
            model._importOptions._fltUVs = fltUVs.Checked;
            model._importOptions._addClrs = addClrs.Checked;
            model._importOptions._rmpClrs = rmpClrs.Checked;
            model._importOptions._rmpMats = rmpMats.Checked;
            model._importOptions._forceCCW = CCW.Checked;

            using (DecoderShell shell = DecoderShell.Import(filePath))
            try
            {
                Say("Extracting textures...");
                Error = "There was a problem reading the textures.";

                //Extract images, removing duplicates
                foreach (ImageEntry img in shell._images)
                {
                    string name;
                    MDL0TextureNode tex;

                    if (img._path != null)
                    {
                        name = Path.GetFileNameWithoutExtension(img._path);
                        //int ind1 = img._path.LastIndexOf('/') + 1;
                        //int ind2 = img._path.LastIndexOf('.');

                        //if (ind2 >= 0)
                        //    name = img._path.Substring(ind1, ind2 - ind1);
                        //else
                        //    name = img._path.Substring(ind1);
                    }
                    else
                        name = img._name != null ? img._name : img._id;

                    tex = model.FindOrCreateTexture(name);
                    img._node = tex;
                }

                Say("Extracting materials...");
                Error = "There was a problem reading the materials.";

                //Extract materials and create shaders
                int tempNo = -1;
                foreach (MaterialEntry mat in shell._materials)
                {
                    tempNo += 1;
                    MDL0MaterialNode matNode = new MDL0MaterialNode();

                    matNode._parent = model._matGroup;
                    matNode._name = mat._name != null ? mat._name : mat._id;

                    matNode._index = tempNo;

                    if (tempNo == 0)
                    {
                        MDL0ShaderNode shadNode = new MDL0ShaderNode();
                        shadNode._parent = model._shadGroup;
                        shadNode._name = "Shader" + tempNo;
                        model._shadList.Add(shadNode);
                    }
                    matNode.Shader = "Shader0";
                    matNode.ShaderNode = (MDL0ShaderNode)model._shadGroup.Children[0];

                    mat._node = matNode;

                    //Find effect
                    if (mat._effect != null)
                        foreach (EffectEntry eff in shell._effects)
                            if (eff._id == mat._effect) //Attach textures and effects to material
                                if (eff._shader != null)
                                    foreach (LightEffectEntry l in eff._shader._effects)
                                        if (l._type == LightEffectType.diffuse && l._texture != null)
                                            foreach (ImageEntry img in shell._images)
                                                if (img._id == l._texture)
                                                {
                                                    MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
                                                    (mr._texture = img._node as MDL0TextureNode)._references.Add(mr);
                                                    mr._name = mr._texture.Name;
                                                    matNode._children.Add(mr);
                                                    mr._parent = matNode;
                                                    mr._minFltr = mr._magFltr = 1;
                                                    mr._index1 = mr._index2 = mr.Index;
                                                    mr._uWrap = mr._vWrap = 1; //"Repeat" works best as a default
                                                    break;
                                                }
                            
                    matNode._numTextures = (byte)matNode.Children.Count;
                    model._matList.Add(matNode);
                }

                Say("Extracting scenes...");

                //Extract scenes
                foreach (SceneEntry scene in shell._scenes)
                {
                    scene._nodes.Sort(NodeEntry.Compare); //Parse joints first
                    foreach (NodeEntry node in scene._nodes)
                        EnumNode(node, model._boneGroup, scene, model, shell);
                }

                //Clean up and set everything left
                model._version = 9; //The user can change the version later

                if (model._importOptions._mdlType == 0)
                    model._unk4 = model._unk5 = 1;
                else
                    model._unk4 = 1;

                Say("Building model...");

                //If there are no bones, rig all objects to a single bind.
                if (model._boneGroup._children.Count == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        MDL0BoneNode bone = new MDL0BoneNode();
                        bone.Scale = new Vector3(1);

                        bone._bindMatrix =
                        bone._inverseBindMatrix =
                        Matrix.Identity;

                        switch (i)
                        {
                            case 0:
                                bone._name = "TopN";
                                model._boneGroup._children.Add(bone);
                                bone._parent = model._boneGroup;
                                break;
                            case 1:
                                bone._name = "TransN";
                                model._boneGroup._children[0]._children.Add(bone);
                                bone._parent = model._boneGroup._children[0];
                                bone.ReferenceCount = model._polyList.Count;
                                break;
                        }
                    }
                    if (model._polyList != null && model._polyList.Count != 0)
                        foreach (MDL0PolygonNode poly in model._polyList)
                        {
                            poly._nodeId = 0;
                            poly.SingleBindInf = (MDL0BoneNode)model._boneGroup._children[0]._children[0];
                        }
                }
                else
                {
                    //Check each polygon to see if it can be single-binded!
                    if (model._polyList != null && model._polyList.Count != 0)
                        foreach (MDL0PolygonNode p in model._polyList)
                        {
                            IMatrixNode node = null; 
                            bool singlebind = true;

                            foreach (Vertex3 v in p._manager._vertices)
                                if (v._influence != null)
                                {
                                    if (node == null)
                                        node = v._influence;

                                    if (v._influence != node)
                                    {
                                        singlebind = false;
                                        break;
                                    }
                                }

                            if (singlebind && p._singleBind == null)
                            {
                                //Increase reference count ahead of time for rebuild
                                if (p._manager._vertices[0]._influence != null)
                                    p._manager._vertices[0]._influence.ReferenceCount++;

                                foreach (Vertex3 v in p._manager._vertices)
                                    if (v._influence != null)
                                        v._influence.ReferenceCount--;
                                
                                p._nodeId = -2; //Continued on polygon rebuild
                            }
                        }
                }

                Error = "There was a problem adding color nodes.";
                //Check for color nodes
                if (model._importOptions._addClrs && model._colorGroup._children.Count == 0)
                {
                    model._noColors = true;
                    RGBAPixel pixel = new RGBAPixel() { R = 128, G = 128, B = 128, A = 255 };

                    //Color nodes will be remapped later
                    if (model._polyList != null && model._polyList.Count != 0)
                        foreach (MDL0PolygonNode p in model._polyList)
                        {
                            p._elementIndices[2] = 0;
                            RGBAPixel* pIn = (RGBAPixel*)(p._manager._faceData[2] = new UnsafeBuffer(4 * p._manager._pointCount)).Address;
                            for (int i = 0; i < p._manager._pointCount; i++)
                                *pIn++ = pixel;
                        }
                }

                Error = "There was a problem remapping the materials.";
                if (model._importOptions._rmpMats && model._matList != null)
                {
                    //Remap materials
                    if (model._polyList != null)
                        foreach (MDL0PolygonNode p in model._polyList)
                            foreach (MDL0MaterialNode m in model._matList)
                                if (m.Children.Count > 0 && 
                                    m.Children[0] != null && 
                                    p.MaterialNode != null &&
                                    p.MaterialNode.Children.Count > 0 &&
                                    p.MaterialNode.Children[0] != null &&
                                    m.Children[0].Name == p.MaterialNode.Children[0].Name)
                                    p.MaterialNode = m;

                    //Clean up materials
                    for (int i = 0; i < model._matList.Count; i++)
                        if (((MDL0MaterialNode)model._matList[i])._polygons.Count == 0)
                            model._matList.RemoveAt(i--);
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Cannot continue importing this model.\n" + Error + "\n\nException:\n" + x.ToString());
                model = null;
                Close();
            }
            finally
            {
                //Clean up the mess we've made
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                //Clean the model and then build it!
                if (model != null)
                {
                    model.CleanGroups();
                    model.BuildFromScratch(this);
                }
            }
            return model;
        }
        static int tempNo = 0;
        private void EnumNode(NodeEntry node, ResourceNode parent, SceneEntry scene, MDL0Node model, DecoderShell shell)
        {
            PrimitiveManager manager = null;
            MDL0BoneNode bone = null;
            Influence inf = null;

            if (node._type == NodeType.JOINT)
            {
                Error = "There was a problem creating a new bone.";

                bone = new MDL0BoneNode();
                bone._name = node._name != null ? node._name : node._id;

                Say("Parsing bone: \"" + bone._name + "\"");

                bone._bindState = node._transform;
                node._node = bone;

                parent._children.Add(bone);
                bone._parent = parent;

                bone.RecalcBindState();

                foreach (NodeEntry e in node._children)
                    EnumNode(e, bone, scene, model, shell);

                inf = new Influence(bone);
                model._influences._influences.Add(inf);
            }

            foreach (InstanceEntry inst in node._instances)
            {
                if (inst._isController)
                {
                    foreach (SkinEntry skin in shell._skins)
                        if (skin._id == inst._url)
                        {
                            foreach (GeometryEntry g in shell._geometry)
                                if (g._id == skin._skinSource)
                                {
                                    Error = @"
                                    There was a problem decoding weighted primitives for the object " + (node._name != null ? node._name : node._id) + 
                                    ".\nOne or more vertices may not be weighted correctly.";
                                    Say("Decoding weighted primitives for " + (g._name != null ? g._name : g._id) + "...");
                                    manager = DecodePrimitivesWeighted(g, skin, scene, model._influences, ref Error);
                                    break;
                                }
                            break;
                        }
                }
                else
                {
                    foreach (GeometryEntry g in shell._geometry)
                        if (g._id == inst._url)
                        {
                            Error = "There was a problem decoding unweighted primitives for the object " + (node._name != null ? node._name : node._id) + ".";
                            Say("Decoding unweighted primitives for " + (g._name != null ? g._name : g._id) + "...");
                            manager = DecodePrimitivesUnweighted(g);
                            break;
                        }
                }

                if (manager != null)
                {
                    Error = "There was a problem creating a new object for " + (node._name != null ? node._name : node._id);
                    int i = 0;
                    foreach (Vertex3 v in manager._vertices)
                        v.Index = i++;

                    MDL0PolygonNode poly = new MDL0PolygonNode() { _manager = manager };
                    poly._manager._polygon = poly;
                    poly._name = node._name != null ? node._name : node._id;

                    //Attach single-bind
                    if (parent != null && parent is MDL0BoneNode)
                        poly.SingleBindInf = (MDL0BoneNode)parent;
                    
                    //Attach material
                    if (inst._material != null)
                        foreach (MaterialEntry mat in shell._materials)
                            if (mat._id == inst._material._target)
                            {
                                (poly._material = (mat._node as MDL0MaterialNode))._polygons.Add(poly);
                                break;
                            }

                    model._numFaces += poly._numFaces = manager._faceCount = manager._pointCount / 3;
                    model._numVertices += poly._numVertices = manager._pointCount;

                    poly._parent = model._polyGroup;
                    poly._index = tempNo++;

                    model._polyList.Add(poly);
                }
            }
        }

        private class ColladaEntry : IDisposable
        {
            internal string _id, _name, _sid;
            internal object _node;

            ~ColladaEntry() { Dispose(); }
            public virtual void Dispose() { GC.SuppressFinalize(this); }
        }
        private class ImageEntry : ColladaEntry
        {
            internal string _path;
        }
        private class MaterialEntry : ColladaEntry
        {
            internal string _effect;
        }
        private class EffectEntry : ColladaEntry
        {
            internal EffectShaderEntry _shader;
        }
        private class GeometryEntry : ColladaEntry
        {
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<PrimitiveEntry> _primitives = new List<PrimitiveEntry>();

            internal int _faces, _lines;

            internal string _verticesId;
            internal InputEntry _verticesInput;

            public override void Dispose()
            {
                foreach (SourceEntry p in _sources)
                    p.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SourceEntry : ColladaEntry
        {
            internal SourceType _arrayType;
            internal string _arrayId;
            internal int _arrayCount;
            internal object _arrayData; //Parser must dispose!

            internal string _accessorSource;
            internal int _accessorCount;
            internal int _accessorStride;

            public override void Dispose()
            {
                if (_arrayData is UnsafeBuffer)
                    ((UnsafeBuffer)_arrayData).Dispose();
                _arrayData = null;
                GC.SuppressFinalize(this);
            }
        }
        private class InputEntry : ColladaEntry
        {
            internal SemanticType _semantic;
            internal int _set;
            internal int _offset;
            internal string _source;
            internal int _outputOffset;
        }
        private class PrimitiveEntry
        {
            internal PrimitiveType _type;

            internal string _material;
            internal int _entryCount;
            internal int _entryStride;
            internal int _faceCount, _pointCount;

            internal List<InputEntry> _inputs = new List<InputEntry>();

            internal List<PrimitiveFace> _faces = new List<PrimitiveFace>();
        }
        private class PrimitiveFace
        {
            internal int _pointCount;
            internal int _faceCount;
            internal ushort[] _pointIndices;
        }
        private class SkinEntry : ColladaEntry
        {
            internal string _skinSource;
            internal Matrix _bindMatrix = Matrix.Identity;

            //internal Matrix _bindShape;
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<InputEntry> _jointInputs = new List<InputEntry>();

            internal List<InputEntry> _weightInputs = new List<InputEntry>();
            internal int _weightCount;
            internal int[][] _weights;

            public override void Dispose()
            {
                foreach (SourceEntry src in _sources)
                    src.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SceneEntry : ColladaEntry
        {
            internal List<NodeEntry> _nodes = new List<NodeEntry>();

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (NodeEntry node in _nodes)
                    if ((n = DecoderShell.FindNodeInternal(name, node)) != null)
                        return n;
                return null;
            }
        }
        private class NodeEntry : ColladaEntry
        {
            internal NodeType _type = NodeType.NONE;
            internal FrameState _transform;
            //internal NodeEntry _parent;
            internal List<NodeEntry> _children = new List<NodeEntry>();
            internal List<InstanceEntry> _instances = new List<InstanceEntry>();

            public static int Compare(NodeEntry n1, NodeEntry n2)
            {
                if ((n1._type == NodeType.NODE || n1._type == NodeType.NONE) && n2._type == NodeType.JOINT)
                    return 1;
                if (n1._type == NodeType.JOINT && (n2._type == NodeType.NODE || n2._type == NodeType.NONE))
                    return -1;

                return 0;
            }
        }
        private class InstanceEntry : ColladaEntry
        {
            internal bool _isController;
            internal string _url;
            internal InstanceMaterial _material;
            internal List<string> skeletons = new List<string>();
        }
        private class InstanceMaterial : ColladaEntry
        {
            internal string _symbol, _target;
            internal List<VertexBind> _vertexBinds = new List<VertexBind>();
        }
        private class VertexBind : ColladaEntry
        {
            internal string _semantic;
            internal string _inputSemantic;
            internal int _inputSet;
        }
        private class EffectShaderEntry : ColladaEntry
        {
            internal ShaderType _type;

            internal float _shininess, _reflectivity, _transparency;

            internal List<LightEffectEntry> _effects = new List<LightEffectEntry>();
        }
        private class LightEffectEntry : ColladaEntry
        {
            internal LightEffectType _type;
            internal RGBAPixel _color;

            internal string _texture;
            internal string _texCoord;
        }
        private enum ShaderType
        {
            None,
            phong,
            lambert,
            blinn
        }
        private enum LightEffectType
        {
            None,
            ambient,
            diffuse,
            emission,
            reflective,
            specular,
            transparent
        }
        private enum PrimitiveType
        {
            None,
            polygons,
            polylist,
            triangles,
            trifans,
            tristrips,
            lines,
            linestrips
        }
        private enum SemanticType
        {
            None,
            POSITION,
            VERTEX,
            NORMAL,
            TEXCOORD,
            COLOR,
            WEIGHT,
            JOINT,
            INV_BIND_MATRIX,
            TEXTANGENT,
            TEXBINORMAL
        }
        private enum SourceType
        {
            None,
            Float,
            Int,
            Name
        }
        private enum NodeType
        {
            NODE,
            JOINT,
            NONE
        }

        private class DecoderShell : IDisposable
        {
            internal List<ImageEntry> _images = new List<ImageEntry>();
            internal List<MaterialEntry> _materials = new List<MaterialEntry>();
            internal List<EffectEntry> _effects = new List<EffectEntry>();
            internal List<GeometryEntry> _geometry = new List<GeometryEntry>();
            internal List<SkinEntry> _skins = new List<SkinEntry>();
            internal List<SceneEntry> _scenes = new List<SceneEntry>();
            internal XmlReader _reader;

            public static DecoderShell Import(string path)
            {
                using (FileMap map = FileMap.FromFile(path))
                using (XmlReader reader = new XmlReader(map.Address, map.Length))
                    return new DecoderShell(reader);
            }
            ~DecoderShell() { Dispose(); }
            public void Dispose()
            {
                foreach (GeometryEntry geo in _geometry)
                    geo.Dispose();
            }

            private void Output(string message)
            {
                MessageBox.Show(message);
            }

            private DecoderShell(XmlReader reader)
            {
                _reader = reader;

                while (reader.BeginElement())
                {
                    if (reader.Name.Equals("COLLADA", true))
                        ParseMain();

                    reader.EndElement();
                }

                _reader = null;
            }

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (SceneEntry scene in _scenes)
                    foreach (NodeEntry node in scene._nodes)
                        if ((n = FindNodeInternal(name, node)) != null)
                            return n;
                return null;
            }
            internal static NodeEntry FindNodeInternal(string name, NodeEntry node)
            {
                NodeEntry e;

                if (node._name == name || node._sid == name || node._id == name)
                    return node;

                foreach (NodeEntry n in node._children)
                    if ((e = FindNodeInternal(name, n)) != null)
                        return e;

                return null;
            }

            private void ParseMain()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("asset", true))
                        ParseAsset();
                    else if (_reader.Name.Equals("library_images", true))
                        ParseLibImages();
                    else if (_reader.Name.Equals("library_materials", true))
                        ParseLibMaterials();
                    else if (_reader.Name.Equals("library_effects", true))
                        ParseLibEffects();
                    else if (_reader.Name.Equals("library_geometries", true))
                        ParseLibGeometry();
                    else if (_reader.Name.Equals("library_controllers", true))
                        ParseLibControllers();
                    else if (_reader.Name.Equals("library_visual_scenes", true))
                        ParseLibScenes();

                    _reader.EndElement();
                }
            }
            
            public float _scale = 1;
            private void ParseAsset()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("unit", true))
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("meter", true))
                                float.TryParse((string)_reader.Value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out _scale);
                    _reader.EndElement();
                }
            }

            private void ParseLibImages()
            {
                ImageEntry img;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("image", true))
                    {
                        img = new ImageEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                img._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                img._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("init_from", true))
                                img._path = _reader.ReadElementString();

                            _reader.EndElement();
                        }

                        _images.Add(img);
                    }
                    _reader.EndElement();
                }
            }
            private void ParseLibMaterials()
            {
                MaterialEntry mat;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("material", true))
                    {
                        mat = new MaterialEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                mat._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                mat._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("instance_effect", true))
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("url", true))
                                        mat._effect = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                            _reader.EndElement();
                        }

                        _materials.Add(mat);
                    }

                    _reader.EndElement();
                }
            }
            private void ParseLibEffects()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("effect", true))
                        _effects.Add(ParseEffect());
                    _reader.EndElement();
                }
            }
            private EffectEntry ParseEffect()
            {
                EffectEntry eff = new EffectEntry();

                while (_reader.ReadAttribute())
                {
                    if (_reader.Name.Equals("id", true))
                        eff._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        eff._name = (string)_reader.Value;
                }

                while (_reader.BeginElement())
                {
                    //Only common is supported
                    if (_reader.Name.Equals("profile_COMMON", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique", true))
                            {
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("phong", true))
                                        eff._shader = ParseShader(ShaderType.phong);
                                    else if (_reader.Name.Equals("lambert", true))
                                        eff._shader = ParseShader(ShaderType.lambert);
                                    else if (_reader.Name.Equals("blinn", true))
                                        eff._shader = ParseShader(ShaderType.blinn);

                                    _reader.EndElement();
                                }
                            }
                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }
                return eff;
            }
            private EffectShaderEntry ParseShader(ShaderType type)
            {
                EffectShaderEntry s = new EffectShaderEntry();
                s._type = type;
                float v;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("ambient", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.ambient));
                    else if (_reader.Name.Equals("diffuse", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.diffuse));
                    else if (_reader.Name.Equals("emission", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.emission));
                    else if (_reader.Name.Equals("reflective", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.reflective));
                    else if (_reader.Name.Equals("specular", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.specular));
                    else if (_reader.Name.Equals("transparent", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.transparent));
                    else if (_reader.Name.Equals("shininess", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._shininess = v;
                            _reader.EndElement();
                        }
                    }
                    else if (_reader.Name.Equals("reflectivity", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._reflectivity = v;
                            _reader.EndElement();
                        }
                    }
                    else if (_reader.Name.Equals("transparency", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._transparency = v;
                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return s;
            }
            private LightEffectEntry ParseLightEffect(LightEffectType type)
            {
                LightEffectEntry eff = new LightEffectEntry();
                eff._type = type;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("color", true))
                        eff._color = ParseColor();
                    else if (_reader.Name.Equals("texture", true))
                    {
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("texture", true))
                                eff._texture = (string)_reader.Value;
                            else if (_reader.Name.Equals("texcoord", true))
                                eff._texCoord = (string)_reader.Value;
                    }

                    _reader.EndElement();
                }

                return eff;
            }
            private void ParseLibGeometry()
            {
                GeometryEntry geo;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("geometry", true))
                    {
                        geo = new GeometryEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                geo._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                geo._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("mesh", true))
                            {
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("source", true))
                                        geo._sources.Add(ParseSource());
                                    else if (_reader.Name.Equals("vertices", true))
                                    {
                                        while (_reader.ReadAttribute())
                                            if (_reader.Name.Equals("id", true))
                                                geo._verticesId = (string)_reader.Value;

                                        while (_reader.BeginElement())
                                        {
                                            if (_reader.Name.Equals("input", true))
                                                geo._verticesInput = ParseInput();

                                            _reader.EndElement();
                                        }
                                    }
                                    else if (_reader.Name.Equals("polygons", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.polygons));
                                    else if (_reader.Name.Equals("polylist", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.polylist));
                                    else if (_reader.Name.Equals("triangles", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.triangles));
                                    else if (_reader.Name.Equals("tristrips", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.tristrips));
                                    else if (_reader.Name.Equals("trifans", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.trifans));
                                    else if (_reader.Name.Equals("lines", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.lines));
                                    else if (_reader.Name.Equals("linestrips", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.linestrips));

                                    _reader.EndElement();
                                }
                            }
                            _reader.EndElement();
                        }

                        _geometry.Add(geo);
                    }
                    _reader.EndElement();
                }
            }
            private PrimitiveEntry ParsePrimitive(PrimitiveType type)
            {
                PrimitiveEntry prim = new PrimitiveEntry() { _type = type };
                PrimitiveFace p;
                int val;
                int stride = 0, elements = 0;

                switch (type)
                {
                    case PrimitiveType.trifans:
                    case PrimitiveType.tristrips:
                    case PrimitiveType.triangles:
                        stride = 3;
                        break;
                    case PrimitiveType.lines:
                    case PrimitiveType.linestrips:
                        stride = 2;
                        break;
                    case PrimitiveType.polygons:
                    case PrimitiveType.polylist:
                        stride = 4;
                        break;
                }

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("material", true))
                        prim._material = (string)_reader.Value;
                    else if (_reader.Name.Equals("count", true))
                        prim._entryCount = int.Parse((string)_reader.Value);

                prim._faces.Capacity = prim._entryCount;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("input", true))
                    {
                        prim._inputs.Add(ParseInput());
                        elements++;
                    }
                    else if (_reader.Name.Equals("p", true))
                    {
                        List<ushort> indices = new List<ushort>(stride * elements);

                        p = new PrimitiveFace();
                        //p._pointIndices.Capacity = stride * elements;
                        while (_reader.ReadValue(&val))
                            indices.Add((ushort)val);

                        p._pointCount = indices.Count / elements;
                        p._pointIndices = indices.ToArray();

                        switch (type)
                        {
                            case PrimitiveType.trifans:
                            case PrimitiveType.tristrips:
                            case PrimitiveType.polygons:
                            case PrimitiveType.polylist:
                                p._faceCount = p._pointCount - 2;
                                break;

                            case PrimitiveType.triangles:
                                p._faceCount = p._pointCount / 3;
                                break;

                            case PrimitiveType.lines:
                                p._faceCount = p._pointCount / 2;
                                break;

                            case PrimitiveType.linestrips:
                                p._faceCount = p._pointCount - 1;
                                break;
                        }

                        prim._faceCount += p._faceCount;
                        prim._pointCount += p._pointCount;
                        prim._faces.Add(p);
                    }

                    _reader.EndElement();
                }

                prim._entryStride = elements;

                return prim;
            }
            private InputEntry ParseInput()
            {
                InputEntry inp = new InputEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        inp._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        inp._name = (string)_reader.Value;
                    else if (_reader.Name.Equals("semantic", true))
                        inp._semantic = (SemanticType)Enum.Parse(typeof(SemanticType), (string)_reader.Value, true);
                    else if (_reader.Name.Equals("set", true))
                        inp._set = int.Parse((string)_reader.Value);
                    else if (_reader.Name.Equals("offset", true))
                        inp._offset = int.Parse((string)_reader.Value);
                    else if (_reader.Name.Equals("source", true))
                        inp._source = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                return inp;
            }
            private SourceEntry ParseSource()
            {
                SourceEntry src = new SourceEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        src._id = (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("float_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Float;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            UnsafeBuffer buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            float* pOut = (float*)buffer.Address;
                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("int_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Int;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            UnsafeBuffer buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            int* pOut = (int*)buffer.Address;
                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("Name_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Name;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            string[] list = new string[src._arrayCount];
                            src._arrayData = list;

                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadStringSingle())
                                    break;
                                else
                                    list[i] = (string)_reader.Value;
                        }
                    }
                    else if (_reader.Name.Equals("technique_common", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("accessor", true))
                            {
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("source", true))
                                        src._accessorSource = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;
                                    else if (_reader.Name.Equals("count", true))
                                        src._accessorCount = int.Parse((string)_reader.Value);
                                    else if (_reader.Name.Equals("stride", true))
                                        src._accessorStride = int.Parse((string)_reader.Value);

                                //Ignore params
                            }

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return src;
            }

            private void ParseLibControllers()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("controller", false))
                    {
                        string id = null;
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", false))
                                id = (string)_reader.Value;

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("skin", false))
                                _skins.Add(ParseSkin(id));

                            _reader.EndElement();
                        }
                    }
                    _reader.EndElement();
                }
            }

            private SkinEntry ParseSkin(string id)
            {
                SkinEntry skin = new SkinEntry();
                skin._id = id;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("source", false))
                        skin._skinSource = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_shape_matrix", false))
                        skin._bindMatrix = ParseMatrix();
                    else if (_reader.Name.Equals("source", false))
                        skin._sources.Add(ParseSource());
                    else if (_reader.Name.Equals("joints", false))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false))
                                skin._jointInputs.Add(ParseInput());

                            _reader.EndElement();
                        }
                    else if (_reader.Name.Equals("vertex_weights", false))
                    {
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("count", false))
                                skin._weightCount = int.Parse((string)_reader.Value);

                        skin._weights = new int[skin._weightCount][];

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false))
                                skin._weightInputs.Add(ParseInput());
                            else if (_reader.Name.Equals("vcount", false))
                            {
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int val;
                                    _reader.ReadValue(&val);
                                    skin._weights[i] = new int[val * skin._weightInputs.Count];
                                }
                            }
                            else if (_reader.Name.Equals("v", false))
                            {
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int[] weights = skin._weights[i];
                                    fixed (int* p = weights)
                                        for (int x = 0; x < weights.Length; x++)
                                            _reader.ReadValue(&p[x]);
                                }
                            }
                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return skin;
            }

            private void ParseLibScenes()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("visual_scene", true))
                        _scenes.Add(ParseScene());

                    _reader.EndElement();
                }
            }

            private SceneEntry ParseScene()
            {
                SceneEntry sc = new SceneEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        sc._id = (string)_reader.Value;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("name", true))
                        sc._name = (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("node", true))
                        sc._nodes.Add(ParseNode());

                    _reader.EndElement();
                }

                return sc;
            }

            private NodeEntry ParseNode()
            {
                NodeEntry node = new NodeEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        node._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        node._name = (string)_reader.Value;
                    else if (_reader.Name.Equals("sid", true))
                        node._sid = (string)_reader.Value;
                    else if (_reader.Name.Equals("type", true))
                        node._type = (NodeType)Enum.Parse(typeof(NodeType), (string)_reader.Value, true);

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("matrix", true))
                    {
                        node._transform = ParseMatrix().Derive();
                        //if (node._type != NodeType.JOINT)
                        //    node._type = NodeType.JOINT;
                    }
                    else if (_reader.Name.Equals("node", true))
                    {
                        node._children.Add(ParseNode());
                        //if (node._type != NodeType.JOINT)
                        //    node._type = NodeType.JOINT;
                    }
                    else if (_reader.Name.Equals("instance_controller", true))
                        node._instances.Add(ParseInstance(true));
                    else if (_reader.Name.Equals("instance_geometry", true))
                        node._instances.Add(ParseInstance(false));

                    _reader.EndElement();
                }
                return node;
            }

            private InstanceEntry ParseInstance(bool controller)
            {
                InstanceEntry c = new InstanceEntry();
                c._isController = controller;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("url", true))
                        c._url = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("skeleton", true))
                        c.skeletons.Add(_reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value);

                    if (_reader.Name.Equals("bind_material", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique_common", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("instance_material", true))
                                        c._material = ParseMatInstance();
                                    _reader.EndElement();
                                }
                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }

                return c;
            }

            private InstanceMaterial ParseMatInstance()
            {
                InstanceMaterial mat = new InstanceMaterial();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("symbol", true))
                        mat._symbol = (string)_reader.Value;
                    else if (_reader.Name.Equals("target", true))
                        mat._target = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_vertex_input", true))
                        mat._vertexBinds.Add(ParseVertexInput());
                    _reader.EndElement();
                }
                return mat;
            }
            private VertexBind ParseVertexInput()
            {
                VertexBind v = new VertexBind();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("semantic", true))
                        v._semantic = (string)_reader.Value;
                    else if (_reader.Name.Equals("input_semantic", true))
                        v._inputSemantic = (string)_reader.Value;
                    else if (_reader.Name.Equals("input_set", true))
                        v._inputSet = int.Parse((string)_reader.Value);

                return v;
            }

            private Matrix ParseMatrix()
            {
                Matrix m;
                float* pM = (float*)&m;
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        _reader.ReadValue(&pM[x * 4 + y]);
                return m;
            }
            private RGBAPixel ParseColor()
            {
                float f;
                RGBAPixel c;
                byte* p = (byte*)&c;
                for (int i = 0; i < 4; i++)
                {
                    if (!_reader.ReadValue(&f))
                        p[i] = 255;
                    else
                        p[i] = (byte)(f * 255.0f + 0.5f);
                }
                return c;
            }
        }

        private void InitializeComponent()
        {
            this.Status = new System.Windows.Forms.Label();
            this.fltVerts = new System.Windows.Forms.CheckBox();
            this.fltNrms = new System.Windows.Forms.CheckBox();
            this.fltUVs = new System.Windows.Forms.CheckBox();
            this.addClrs = new System.Windows.Forms.CheckBox();
            this.rmpClrs = new System.Windows.Forms.CheckBox();
            this.forceTriangles = new System.Windows.Forms.CheckBox();
            this.CCW = new System.Windows.Forms.CheckBox();
            this.rmpMats = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.mdlType = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(12, 9);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(107, 13);
            this.Status.TabIndex = 0;
            this.Status.Text = "Parsing DAE model...";
            this.Status.UseWaitCursor = true;
            // 
            // fltVerts
            // 
            this.fltVerts.AutoSize = true;
            this.fltVerts.Location = new System.Drawing.Point(12, 12);
            this.fltVerts.Name = "fltVerts";
            this.fltVerts.Size = new System.Drawing.Size(116, 17);
            this.fltVerts.TabIndex = 1;
            this.fltVerts.Text = "Force float vertices";
            this.fltVerts.UseVisualStyleBackColor = true;
            // 
            // fltNrms
            // 
            this.fltNrms.AutoSize = true;
            this.fltNrms.Location = new System.Drawing.Point(12, 35);
            this.fltNrms.Name = "fltNrms";
            this.fltNrms.Size = new System.Drawing.Size(115, 17);
            this.fltNrms.TabIndex = 2;
            this.fltNrms.Text = "Force float normals";
            this.fltNrms.UseVisualStyleBackColor = true;
            // 
            // fltUVs
            // 
            this.fltUVs.AutoSize = true;
            this.fltUVs.Location = new System.Drawing.Point(12, 58);
            this.fltUVs.Name = "fltUVs";
            this.fltUVs.Size = new System.Drawing.Size(99, 17);
            this.fltUVs.TabIndex = 3;
            this.fltUVs.Text = "Force float UVs";
            this.fltUVs.UseVisualStyleBackColor = true;
            // 
            // addClrs
            // 
            this.addClrs.AutoSize = true;
            this.addClrs.Checked = true;
            this.addClrs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.addClrs.Location = new System.Drawing.Point(12, 81);
            this.addClrs.Name = "addClrs";
            this.addClrs.Size = new System.Drawing.Size(141, 17);
            this.addClrs.TabIndex = 4;
            this.addClrs.Text = "Add color nodes, if none";
            this.addClrs.UseVisualStyleBackColor = true;
            this.addClrs.CheckedChanged += new System.EventHandler(this.addClrs_CheckedChanged);
            // 
            // rmpClrs
            // 
            this.rmpClrs.AutoSize = true;
            this.rmpClrs.Checked = true;
            this.rmpClrs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rmpClrs.Location = new System.Drawing.Point(12, 104);
            this.rmpClrs.Name = "rmpClrs";
            this.rmpClrs.Size = new System.Drawing.Size(196, 17);
            this.rmpClrs.TabIndex = 5;
            this.rmpClrs.Text = "Remap all objects to one color node";
            this.rmpClrs.UseVisualStyleBackColor = true;
            // 
            // forceTriangles
            // 
            this.forceTriangles.AutoSize = true;
            this.forceTriangles.Checked = true;
            this.forceTriangles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.forceTriangles.Enabled = false;
            this.forceTriangles.Location = new System.Drawing.Point(203, 35);
            this.forceTriangles.Name = "forceTriangles";
            this.forceTriangles.Size = new System.Drawing.Size(162, 17);
            this.forceTriangles.TabIndex = 6;
            this.forceTriangles.Text = "Force Triangle primitives only";
            this.forceTriangles.UseVisualStyleBackColor = true;
            // 
            // CCW
            // 
            this.CCW.AutoSize = true;
            this.CCW.Location = new System.Drawing.Point(203, 58);
            this.CCW.Name = "CCW";
            this.CCW.Size = new System.Drawing.Size(141, 17);
            this.CCW.TabIndex = 7;
            this.CCW.Text = "Force CCW primitive cull";
            this.CCW.UseVisualStyleBackColor = true;
            // 
            // rmpMats
            // 
            this.rmpMats.AutoSize = true;
            this.rmpMats.Checked = true;
            this.rmpMats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rmpMats.Location = new System.Drawing.Point(203, 81);
            this.rmpMats.Name = "rmpMats";
            this.rmpMats.Size = new System.Drawing.Size(104, 17);
            this.rmpMats.TabIndex = 8;
            this.rmpMats.Text = "Remap materials";
            this.rmpMats.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Okay";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(307, 105);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.mdlType);
            this.panel1.Controls.Add(this.fltVerts);
            this.panel1.Controls.Add(this.fltNrms);
            this.panel1.Controls.Add(this.fltUVs);
            this.panel1.Controls.Add(this.addClrs);
            this.panel1.Controls.Add(this.rmpClrs);
            this.panel1.Controls.Add(this.forceTriangles);
            this.panel1.Controls.Add(this.CCW);
            this.panel1.Controls.Add(this.rmpMats);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 140);
            this.panel1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Type:";
            // 
            // mdlType
            // 
            this.mdlType.FormattingEnabled = true;
            this.mdlType.Items.AddRange(new object[] {
            "Character",
            "Stage/Item"});
            this.mdlType.Location = new System.Drawing.Point(244, 10);
            this.mdlType.Name = "mdlType";
            this.mdlType.Size = new System.Drawing.Size(121, 21);
            this.mdlType.TabIndex = 11;
            this.mdlType.SelectedIndexChanged += new System.EventHandler(this.mdlType_SelectedIndexChanged);
            // 
            // Collada
            // 
            this.ClientSize = new System.Drawing.Size(394, 140);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Collada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void addClrs_CheckedChanged(object sender, EventArgs e)
        {
            rmpClrs.Enabled = addClrs.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; 
            Close();
        }

        private void mdlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (mdlType.SelectedIndex == 0)
            //    fltVerts.Checked = fltNrms.Checked = fltUVs.Checked = true;
            //else
            //    fltVerts.Checked = fltNrms.Checked = fltUVs.Checked = false;
        }
    }
}
