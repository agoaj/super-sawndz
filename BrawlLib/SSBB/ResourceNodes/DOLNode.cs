using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.IO;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class DOLNode : ResourceNode
    {
        internal DOLHeader* Header { get { return (DOLHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        [Category("DOLphin Static Module")]
        public string Text0Offset { get { return ((int)Header->Text0Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text1Offset { get { return ((int)Header->Text1Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text2Offset { get { return ((int)Header->Text2Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text3Offset { get { return ((int)Header->Text3Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text4Offset { get { return ((int)Header->Text4Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text5Offset { get { return ((int)Header->Text5Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text6Offset { get { return ((int)Header->Text6Offset).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string Data0Offset { get { return ((int)Header->Data0Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data1Offset { get { return ((int)Header->Data1Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data2Offset { get { return ((int)Header->Data2Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data3Offset { get { return ((int)Header->Data3Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data4Offset { get { return ((int)Header->Data4Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data5Offset { get { return ((int)Header->Data5Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data6Offset { get { return ((int)Header->Data6Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data7Offset { get { return ((int)Header->Data7Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data8Offset { get { return ((int)Header->Data8Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data9Offset { get { return ((int)Header->Data9Offset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data10Offset { get { return ((int)Header->Data10Offset).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string Text0LoadAddr { get { return ((int)Header->Text0LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text1LoadAddr { get { return ((int)Header->Text1LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text2LoadAddr { get { return ((int)Header->Text2LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text3LoadAddr { get { return ((int)Header->Text3LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text4LoadAddr { get { return ((int)Header->Text4LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text5LoadAddr { get { return ((int)Header->Text5LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text6LoadAddr { get { return ((int)Header->Text6LoadAddr).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string Data0LoadAddr { get { return ((int)Header->Data0LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data1LoadAddr { get { return ((int)Header->Data1LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data2LoadAddr { get { return ((int)Header->Data2LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data3LoadAddr { get { return ((int)Header->Data3LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data4LoadAddr { get { return ((int)Header->Data4LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data5LoadAddr { get { return ((int)Header->Data5LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data6LoadAddr { get { return ((int)Header->Data6LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data7LoadAddr { get { return ((int)Header->Data7LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data8LoadAddr { get { return ((int)Header->Data8LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data9LoadAddr { get { return ((int)Header->Data9LoadAddr).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data10LoadAddr { get { return ((int)Header->Data10LoadAddr).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string Text0Size { get { return ((int)Header->Text0Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text1Size { get { return ((int)Header->Text1Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text2Size { get { return ((int)Header->Text2Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text3Size { get { return ((int)Header->Text3Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text4Size { get { return ((int)Header->Text4Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text5Size { get { return ((int)Header->Text5Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Text6Size { get { return ((int)Header->Text6Size).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string Data0Size { get { return ((int)Header->Data0Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data1Size { get { return ((int)Header->Data1Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data2Size { get { return ((int)Header->Data2Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data3Size { get { return ((int)Header->Data3Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data4Size { get { return ((int)Header->Data4Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data5Size { get { return ((int)Header->Data5Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data6Size { get { return ((int)Header->Data6Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data7Size { get { return ((int)Header->Data7Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data8Size { get { return ((int)Header->Data8Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data9Size { get { return ((int)Header->Data9Size).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string Data10Size { get { return ((int)Header->Data10Size).ToString("X"); } }

        [Category("DOLphin Static Module")]
        public string bssOffset { get { return ((int)Header->bssOffset).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string bssSize { get { return ((int)Header->bssSize).ToString("X"); } }
        [Category("DOLphin Static Module")]
        public string EntryPoint { get { return ((int)Header->entryPoint).ToString("X"); } }

        protected override bool OnInitialize()
        {
            _name = Path.GetFileName(_origPath);
            return true;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < 7; i++)
                if (Header->TextOffset(i) > 0)
                    new RawDataNode("Text" + i).Initialize(this, (VoidPtr)Header + Header->TextOffset(i), (int)Header->TextSize(i));

            for (int i = 0; i < 11; i++)
                if (Header->DataOffset(i) > 0)
                    new RawDataNode("Data" + i).Initialize(this, (VoidPtr)Header + Header->DataOffset(i), (int)Header->DataSize(i));
        }
    }
}
