using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class NumericInputBox : TextBox
    {
        public float _oldValue;
        private bool check = true;
        public float _value;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;

                if (check == false) float.TryParse(Text, out _oldValue);

                _value = value;

                UpdateText();

                if (check == true) { float.TryParse(Text, out _oldValue); check = false; }

                Apply();
            }
        }

        public NumericInputBox() { UpdateText(); }

        public event EventHandler ValueChanged;

        protected override void OnLostFocus(EventArgs e)
        {
            Apply();
            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            float val;
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Back:
                    break;

                case Keys.Left:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Control)
                        {
                            Text = (val - 90f).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        if (e.Shift)
                        {
                            Text = (val - 180f).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }
                    break;

                case Keys.Right:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Control)
                        {
                            Text = (val + 90f).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        if (e.Shift)
                        {
                            Text = (val + 180f).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }
                    break;

                case Keys.Up:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Shift)
                            Text = (val + 1.0f).ToString();
                        else
                            Text = (val + 0.1f).ToString();
                        Apply();
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Down:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Shift)
                            Text = (val - 1.0f).ToString();
                        else
                            Text = (val - 0.1f).ToString();
                        Apply();
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    if ((this.SelectionStart != 0) || (Text.IndexOf('-') != -1))
                        e.SuppressKeyPress = true;
                    break;

                case Keys.Decimal:
                case Keys.OemPeriod:
                    if (Text.IndexOf('.') != -1)
                        e.SuppressKeyPress = true;
                    break;

                case Keys.Escape:
                    UpdateText();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Enter:
                    Apply();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.X:
                    if (e.Control)
                    {
                        if (float.TryParse(Text, out val))
                        {
                            val = float.NaN;
                            Text = val.ToString();
                            Apply();
                        }
                    }
                    break;

                case Keys.V:
                case Keys.C:
                    if (!e.Control)
                        goto default;
                    break;

                default:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        private void UpdateText()
        {
            if (_value == float.NaN)
                Text = "";
            else
                Text = _value.ToString();
        }

        private void Apply()
        {
            float val = _value;

            if (val.ToString() == Text)
                return;

            if (Text == "")
                val = float.NaN;
            else
                float.TryParse(Text, out val);

            if (_value != val)
            {
                _value = val;
                if (ValueChanged != null)
                    ValueChanged(this, null);
            }
            UpdateText();
        }
    }
}
