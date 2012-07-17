using System;

namespace BrawlLib
{
    public static class ExportFilters
    {
        public static string TEX0 =
            "All Image Formats (*.png,*.tga,*.tif,*.tiff,*.bmp,*.jpg,*.jpeg,*.gif,*.tex0)|*.png;*.tga;*.tif;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif;*.tex0|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tif, *.tiff)|*.tif;*.tiff|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif|" +
            "TEX0 Raw Texture (*.tex0)|*.tex0";

        public static string MDL0 =
            "All Model Formats (*.mdl0, *.dae, *.pmd)|*.mdl0;*.dae;*.pmd|" +
            //"All Model Formats (*.mdl0, *.dae)|*.mdl0;*.dae|" +
            "Collada Scene (*.dae)|*.dae|" +
            "MDL0 Raw Model (*.mdl0)|*.mdl0";
            //"Miku Miku Dance Model (*.pmd)|*.pmd";

        public static string CHR0 =
            "CHR0 Raw Animation (*.chr0)|*.chr0";

        public static string PLT0 =
            "PLT0 Raw Palette (*.plt0)|*.plt0";

        public static string PAT0 =
            "PAT0 Raw Texture Pattern (*.pat0)|*.pat0";

        public static string MSBin =
            "MSBin Message List (*.msbin)|*.msbin";

        public static string BRES =
            "BRResource Pack (*.brres)|*.brres";

        public static string RSTM =
            "All Audio Formats (*.brstm, *.wav)|*.brstm;*.wav|" +
            "BRSTM Raw Audio (*.brstm)|*.brstm|" + 
            "Uncompressed PCM (*.wav)|*.wav";

        public static string RWSD =
            "Raw Sound Pack (*.rwsd)|*.rwsd";

        public static string RBNK =
            "Raw Sound Bank (*.rbnk)|*.rbnk";

        public static string RSEQ =
            "Raw Sound Requence (*.rseq)|*.rseq";

        public static string CLR0 =
            "Color Sequence (*.clr0)|*.clr0";

        public static string VIS0 =
            "Visibility Sequence (*.vis0)|*.vis0";

        public static string SRT0 =
            "Texture Animation (*.srt0)|*.srt0";

        public static string SCN0 =
            "Scene Settings (*.scn0)|*.scn0";

        public static string SHP0 =
            "Vertex Set Morph (*.shp0)|*.shp0";

        public static string REFF =
            "REFF (*.breff)|*.breff";

        public static string REFT =
            "REFT (*.breft)|*.breft";

        public static string REFTImage =
            "All Image Formats (*.png,*.tga,*.tif,*.tiff,*.bmp,*.jpg,*.jpeg,*.gif,*.*)|*.png;*.tga;*.tif;*.tiff;*.bmp;*.jpg;*.jpeg,*.gif;*.*|" +
            "Portable Network Graphics (*.png)|*.png|" +
            "Truevision TARGA (*.tga)|*.tga|" +
            "Tagged Image File Format (*.tif, *.tiff)|*.tif;*.tiff|" +
            "Bitmap (*.bmp)|*.bmp|" +
            "Jpeg (*.jpg,*.jpeg)|*.jpg;*.jpeg|" +
            "Gif (*.gif)|*.gif|" +
            "Raw Image Data (*.*)|*.*";

        public static string EFLS =
            "Effect List (*.efls)|*.efls";

        public static string CollisionDef =
            "Collision Definition (*.coll)|*.coll";

        public static string REL =
            "REL (*.rel)|*.rel";

        public static string Polygon =
            "Object (*.obj)|*.obj|" +
            "Raw Data File (*.*)|*.*";

        public static string RAW =
            "Raw Data File (*.*)|*.*";

        public static string MDef =
            "Raw Fighter Moveset (*.moveset)|*.moveset";
        
        public static string WAV =
            "Uncompressed PCM (*.wav)|*.wav";
    }
}
