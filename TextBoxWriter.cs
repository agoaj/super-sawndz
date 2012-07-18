using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BrawlSoundConverter
{
	class TextBoxWriter : System.IO.TextWriter
	{
		System.Windows.Forms.TextBox textBox = null;
		public TextBoxWriter( System.Windows.Forms.TextBox box )
		{
			textBox = box;
		}
		public override void Write(char value)
        {
            base.Write(value);
			textBox.AppendText( value.ToString() );
        }
        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
	}
}
