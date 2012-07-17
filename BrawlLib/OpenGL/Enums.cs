using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.OpenGL
{
    public enum GLBlendFactor : uint
    {
        ZERO = 0,
        ONE = 1,
        SRC_COLOR = 0x0300,
        ONE_MINUS_SRC_COLOR = 0x0301,
        SRC_ALPHA = 0x0302,
        ONE_MINUS_SRC_ALPHA = 0x0303,
        DST_ALPHA = 0x0304,
        ONE_MINUS_DST_ALPHA = 0x0305,
        DST_COLOR = 0x0306,
        ONE_MINUS_DST_COLOR = 0x0307,
        SRC_ALPHA_SATURATE = 0x0308
    }

    public enum GLUQuadricDrawStyle : uint
    {
        GLU_POINT = 100010,
        GLU_LINE = 100011,
        GLU_FILL = 100012,
        GLU_SILHOUETTE = 100013
    }

    public enum GLUQuadricOrientation : uint
    {
        Outside = 100020,
        Inside = 100021
    }

    public enum GLListMode : uint
    {
        COMPILE = 0x1300,
        COMPILE_AND_EXECUTE = 0x1301
    }

    public enum GLShadingModel : uint
    {
        FLAT = 0x1D00,
        SMOOTH = 0x1D01
    }

    public enum GLFrontFaceDirection : uint
    {
        CW = 0x0900,
        CCW = 0x0901
    }

    public enum GLLightTarget : uint
    {
        Light0 = 0x4000,
        Light1 = 0x4001,
        Light2 = 0x4002,
        Light3 = 0x4003,
        Light4 = 0x4004,
        Light5 = 0x4005,
        Light6 = 0x4006,
        Light7 = 0x4007
    }

    public enum GLLightParameter : uint
    {
        AMBIENT = 0x1200,
        DIFFUSE = 0x1201,
        SPECULAR = 0x1202,
        POSITION = 0x1203,
        SPOT_DIRECTION = 0x1204,
        SPOT_EXPONENT = 0x1205,
        SPOT_CUTOFF = 0x1206,
        CONSTANT_ATTENUATION = 0x1207,
        LINEAR_ATTENUATION = 0x1208,
        QUADRATIC_ATTENUATION = 0x1209
    }

    public enum GLMaterialParameter : uint
    {
        AMBIENT = 0x1200,
        DIFFUSE = 0x1201,
        SPECULAR = 0x1202,
        EMISSION = 0x1600,
        AMBIENT_AND_DIFFUSE = 0x1602
    }

    public enum GLMultiTextureTarget : uint
    {
        TEXTURE0 = 0x84C0,
        TEXTURE1 = 0x84C1,
        TEXTURE2 = 0x84C2,
        TEXTURE3 = 0x84C3,
        TEXTURE4 = 0x84C4,
        TEXTURE5 = 0x84C5,
        TEXTURE6 = 0x84C6,
        TEXTURE7 = 0x84C7,
        TEXTURE8 = 0x84C8,
        TEXTURE9 = 0x84C9,
        TEXTURE10 = 0x84CA,
        TEXTURE11 = 0x84CB,
        TEXTURE12 = 0x84CC,
        TEXTURE13 = 0x84CD,
        TEXTURE14 = 0x84CE,
        TEXTURE15 = 0x84CF,
        TEXTURE16 = 0x84D0,
        TEXTURE17 = 0x84D1,
        TEXTURE18 = 0x84D2,
        TEXTURE19 = 0x84D3,
        TEXTURE20 = 0x84D4,
        TEXTURE21 = 0x84D5,
        TEXTURE22 = 0x84D6,
        TEXTURE23 = 0x84D7,
        TEXTURE24 = 0x84D8,
        TEXTURE25 = 0x84D9,
        TEXTURE26 = 0x84DA,
        TEXTURE27 = 0x84DB,
        TEXTURE28 = 0x84DC,
        TEXTURE29 = 0x84DD,
        TEXTURE30 = 0x84DE,
        TEXTURE31 = 0x84DF,
        ACTIVE_TEXTURE = 0x84E0,
        CLIENT_ACTIVE_TEXTURE = 0x84E1,
        MAX_TEXTURE_UNITS = 0x84E2
    }

    public enum TextureGenParameter : uint
    {
        TEXTURE_GEN_MODE = 0x2500,
        OBJECT_PLANE = 0x2501,
        EYE_PLANE = 0x2502
    }

    public enum TextureGenMode : uint
    {
        EYE_LINEAR = 0x2400,
        OBJECT_LINEAR = 0x2401,
        SPHERE_MAP = 0x2402,
        NORMAL_MAP = 0x8511,
        REFLECTION_MAP = 0x8512,
    }

    public enum TextureCoordName : uint
    {
        S = 0x2000,
        T = 0x2001,
        R = 0x2002,
        Q = 0x2003,
    }

    public enum GLFunction : uint
    {
        NEVER = 0x0200,
        LESS = 0x0201,
        EQUAL = 0x0202,
        LEQUAL = 0x0203,
        GREATER = 0x0204,
        NOTEQUAL = 0x0205,
        GEQUAL = 0x0206,
        ALWAYS = 0x0207
    }

    public enum GLHintTarget : uint
    {
        PERSPECTIVE_CORRECTION_HINT = 0x0C50,
        POINT_SMOOTH_HINT = 0x0C51,
        LINE_SMOOTH_HINT = 0x0C52,
        POLYGON_SMOOTH_HINT = 0x0C53,
        FOG_HINT = 0x0C54
    }

    public enum GLHintMode : uint
    {
        DONT_CARE = 0x1100,
        FASTEST = 0x1101,
        NICEST = 0x1102
    }

    public enum GLAccumOp : uint
    {
        Accum = 0x0100,
        Load = 0x0101,
        Return = 0x0102,
        Mult = 0x0103,
        Add = 0x0104
    }

    public enum GLAlphaFunc : uint
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207
    }

    public enum GLPrimitiveType : uint
    {
        Points = 0x0000,
        Lines = 0x0001,
        LineLoop = 0x0002,
        LineStrip = 0x0003,
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        Quads = 0x0007,
        QuadStrip = 0x0008,
        Polygon = 0x0009
    }

    public enum GLClearMask : uint
    {
        DepthBuffer = 0x00000100,
        StencilBuffer = 0x00000400,
        ColorBuffer = 0x00004000
    }

    public enum GLMatrixMode : uint
    {
        ModelView = 0x1700,
        Projection = 0x1701,
        Texture = 0x1702
    }

    public enum GLFace : uint
    {
        Front = 0x404,
        Back = 0x405,
        FrontAndBack = 0x408
    }

    public enum GLPolygonMode : uint
    {
        Point = 0x1B00,
        Line = 0x1B01,
        Fill = 0x1B02
    }

    public enum GLTextureTarget : uint
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        Texture3D = 0x806F,
        TextureCubeMap = 0x8513
    }

    public enum GLTexImageTarget : uint
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        ProxyTexture1D = 0x8063,
        ProxyTexture2D = 0x8064,

        Texture3D = 0x806F,
        TextureCubeMap = 0x8513
    }

    public enum GLTextureParameter : uint
    {
        MagFilter = 0x2800,
        MinFilter = 0x2801,
        WrapS = 0x2802,
        WrapT = 0x2803,
        MinLOD = 0x813A,
        MaxLOD = 0x813B,
        BaseLevel = 0x813C,
        MaxLevel = 0x813D,
        LODBias = 0x8501,
    }

    public enum GLTextureWrapMode : uint
    {
        CLAMP = 0x2900,
        REPEAT = 0x2901,
        CLAMP_TO_EDGE = 0x812F,
        CLAMP_TO_BORDER = 0x812D,
        MIRRORED_REPEAT = 0x8370,
        MIRROR_CLAMP_TO_BORDER_EXT = 0x8912,
        MIRROR_CLAMP_ATI = 0x8742,
        MIRROR_CLAMP_TO_EDGE_EXT = 0x8743,
        MIRROR_CLAMP_EXT = 0x8742,
        MIRRORED_REPEAT_IBM = 0x8370,
    }

    public enum GLTextureFilter : uint
    {
        NEAREST = 0x2600,
        LINEAR = 0x2601,
        NEAREST_MIPMAP_NEAREST = 0x2700,
        LINEAR_MIPMAP_NEAREST = 0x2701,
        NEAREST_MIPMAP_LINEAR = 0x2702,
        LINEAR_MIPMAP_LINEAR = 0x2703
    }

    public enum GLInternalPixelFormat : uint
    {
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
        R3_G3_B2 = 0x2A10,
        GL_ALPHA = 0x1906,
        ALPHA4 = 0x803B,
        ALPHA8 = 0x803C,
        ALPHA12 = 0x803D,
        ALPHA16 = 0x803E,
        LUMINANCE4 = 0x803F,
        LUMINANCE8 = 0x8040,
        LUMINANCE12 = 0x8041,
        LUMINANCE16 = 0x8042,
        LUMINANCE4_ALPHA4 = 0x8043,
        LUMINANCE6_ALPHA2 = 0x8044,
        LUMINANCE8_ALPHA8 = 0x8045,
        LUMINANCE12_ALPHA4 = 0x8046,
        LUMINANCE12_ALPHA12 = 0x8047,
        LUMINANCE16_ALPHA16 = 0x8048,
        INTENSITY = 0x8049,
        INTENSITY4 = 0x804A,
        INTENSITY8 = 0x804B,
        INTENSITY12 = 0x804C,
        INTENSITY16 = 0x804D,
        RGB = 0x1906,
        RGB4 = 0x804F,
        RGB5 = 0x8050,
        RGB8 = 0x8051,
        RGB10 = 0x8052,
        RGB12 = 0x8053,
        RGB16 = 0x8054,
        RGBA2 = 0x8055,
        RGBA4 = 0x8056,
        RGB5_A1 = 0x8057,
        RGBA8 = 0x8058,
        RGB10_A2 = 0x8059,
        RGBA = 0x1907,
        RGBA12 = 0x805A,
        RGBA16 = 0x805B
    }

    public enum GLErrorCode : uint
    {
        NO_ERROR = 0,
        INVALID_ENUM = 0x0500,
        INVALID_VALUE = 0x0501,
        INVALID_OPERATION = 0x0502,
        STACK_OVERFLOW = 0x0503,
        STACK_UNDERFLOW = 0x0504,
        OUT_OF_MEMORY = 0x0505
    }

    public enum GLPixelDataFormat : uint
    {
        COLOR_INDEX = 0x1900,
        STENCIL_INDEX = 0x1901,
        DEPTH_COMPONENT = 0x1902,
        RED = 0x1903,
        GREEN = 0x1904,
        BLUE = 0x1905,
        ALPHA = 0x1906,
        RGB = 0x1907,
        RGBA = 0x1908,
        LUMINANCE = 0x1909,
        LUMINANCE_ALPHA = 0x190A,
        BGR = 0x80E0,
        BGRA = 0x80E1
    }

    public enum GLPixelDataType : uint
    {
        BITMAP = 0x1A00,

        BYTE = 0x1400,
        UNSIGNED_BYTE = 0x1401,
        SHORT = 0x1402,
        UNSIGNED_SHORT = 0x1403,
        INT = 0x1404,
        UNSIGNED_INT = 0x1405,
        FLOAT = 0x1406,

        UNSIGNED_BYTE_3_3_2 = 0x8032,
        UNSIGNED_SHORT_4_4_4_4 = 0x8033,
        UNSIGNED_SHORT_5_5_5_1 = 0x8034,
        UNSIGNED_INT_8_8_8_8 = 0x8035,
        UNSIGNED_INT_10_10_10_2 = 0x8036,
        UNSIGNED_BYTE_2_3_3_REV = 0x8362,
        UNSIGNED_SHORT_5_6_5 = 0x8363,
        UNSIGNED_SHORT_5_6_5_REV = 0x8364,
        UNSIGNED_SHORT_4_4_4_4_REV = 0x8365,
        UNSIGNED_SHORT_1_5_5_5_REV = 0x8366,
        UNSIGNED_INT_8_8_8_8_REV = 0x8367,
        UNSIGNED_INT_2_10_10_10_REV = 0x8368
    }

    public enum GLElementType : uint
    {
        UNSIGNED_BYTE = 0x1401,
        UNSIGNED_SHORT = 0x1403,
        UNSIGNED_INT = 0x1405
    }

    public enum GLDataType : uint
    {
        SByte = 0x1400,
        Byte = 0x1401,
        Short = 0x1402,
        UShort = 0x1403,
        Int = 0x1404,
        UInt = 0x1405,
        Float = 0x1406,
        Double = 0x140A,
    }

    public enum GLArrayType : uint
    {
        VERTEX_ARRAY = 0x8074,
        NORMAL_ARRAY = 0x8075,
        COLOR_ARRAY = 0x8076,
        INDEX_ARRAY = 0x8077,
        TEXTURE_COORD_ARRAY = 0x8078,
        EDGE_FLAG_ARRAY = 0x8079
    }

    public enum GLTexEnvTarget : uint
    {
        TextureEnvironment = 0x2300,
        FilterControl = 0x8500,
        PointSprite = 0x8861
    }

    public enum GLTexEnvParam : uint
    {
        TEXTURE_ENV_MODE = 0x2200,
        TEXTURE_ENV_COLOR = 0x2201,
        TEXTURE_LOD_BIAS = 0x8501,
        COMBINE_RGB = 0x8571,
        COMBINE_ALPHA = 0x8572,

        GL_SRC0_RGB = 0x8580,
        GL_SRC1_RGB = 0x8581,
        GL_SRC2_RGB = 0x8582,
        GL_SRC0_ALPHA = 0x8588,
        GL_SRC1_ALPHA = 0x8589,
        GL_SRC2_ALPHA = 0x858A,
        //GL_OPERAND0_RGB,
        //GL_OPERAND1_RGB,
        //GL_OPERAND2_RGB,
        //GL_OPERAND0_ALPHA,
        //GL_OPERAND1_ALPHA,
        //GL_OPERAND2_ALPHA,
        //GL_RGB_SCALE,
        //GL_ALPHA_SCALE,
        //GL_COORD_REPLACE
    }

    public enum GLTexEnvMode : uint
    {
        MODULATE = 0x2100,
        DECAL = 0x2101,
        BLEND = 0x2102,
        REPLACE = 0x2103
    }

    public enum GLGetMode : uint
    {
        MATRIX_MODE = 0x0BA0,
        NORMALIZE = 0x0BA1,
        VIEWPORT = 0x0BA2,
        MODELVIEW_STACK_DEPTH = 0x0BA3,
        PROJECTION_STACK_DEPTH = 0x0BA4,
        TEXTURE_STACK_DEPTH = 0x0BA5,
        MODELVIEW_MATRIX = 0x0BA6,
        PROJECTION_MATRIX = 0x0BA7,
        TEXTURE_MATRIX = 0x0BA8,
        ALPHA_TEST = 0x0BC0,
        ALPHA_TEST_FUNC = 0x0BC1,
        ALPHA_TEST_REF = 0x0BC2,
        ALPHA_BITS = 0x0D55,
    }

    public enum GLEnableCap : uint
    {
        Fog = 0x0B60,
        Lighting = 0x0B50,
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        LineStipple = 0x0B24,
        PolygonStipple = 0x0B42,
        CullFace = 0x0B44,
        AlphaTest = 0x0BC0,
        Blend = 0x0BE2,
        IndexLogicOp = 0x0BF1,
        ColorLogicOp = 0x0BF2,
        Dither = 0x0BD0,
        StencilTest = 0x0B90,
        DepthTest = 0x0B71,
        ClipPlane0 = 0x3000,
        ClipPlane1 = 0x3001,
        ClipPlane2 = 0x3002,
        ClipPlane3 = 0x3003,
        ClipPlane4 = 0x3004,
        ClipPlane5 = 0x3005,
        Light0 = 0x4000,
        Light1 = 0x4001,
        Light2 = 0x4002,
        Light3 = 0x4003,
        Light4 = 0x4004,
        Light5 = 0x4005,
        Light6 = 0x4006,
        Light7 = 0x4007,
        TEXTURE_GEN_S = 0x0C60,
        TEXTURE_GEN_T = 0x0C61,
        TEXTURE_GEN_R = 0x0C62,
        TEXTURE_GEN_Q = 0x0C63,
        //use GetPName MAP1_VERTEX_3
        //use GetPName MAP1_VERTEX_4
        //use GetPName MAP1_COLOR_4
        //use GetPName MAP1_INDEX
        //use GetPName MAP1_NORMAL
        //use GetPName MAP1_TEXTURE_COORD_1
        //use GetPName MAP1_TEXTURE_COORD_2
        //use GetPName MAP1_TEXTURE_COORD_3
        //use GetPName MAP1_TEXTURE_COORD_4
        //use GetPName MAP2_VERTEX_3
        //use GetPName MAP2_VERTEX_4
        //use GetPName MAP2_COLOR_4
        //use GetPName MAP2_INDEX
        //use GetPName MAP2_NORMAL
        //use GetPName MAP2_TEXTURE_COORD_1
        //use GetPName MAP2_TEXTURE_COORD_2
        //use GetPName MAP2_TEXTURE_COORD_3
        //use GetPName MAP2_TEXTURE_COORD_4
        POINT_SMOOTH = 0x0B10,
        LINE_SMOOTH	= 0x0B20,
        POLYGON_SMOOTH = 0x0B41,
        //use GetPName SCISSOR_TEST
        COLOR_MATERIAL = 0x0B57,
        FRAGMENT_PROGRAM_ARB = 0x8804,
        VERTEX_PROGRAM_ARB = 0x8620,
        //use GetPName NORMALIZE
        //use GetPName AUTO_NORMAL
        //use GetPName POLYGON_OFFSET_POINT
        //use GetPName POLYGON_OFFSET_LINE
        //use GetPName POLYGON_OFFSET_FILL
        //use GetPName VERTEX_ARRAY
        //use GetPName NORMAL_ARRAY
        //use GetPName COLOR_ARRAY
        //use GetPName INDEX_ARRAY
        //use GetPName TEXTURE_COORD_ARRAY
        //use GetPName EDGE_FLAG_ARRAY
    }

    public enum AssemblyProgramTargetArb : int
    {
        VertexProgram = 0x8620,
        FragmentProgram = 0x8804,
        GeometryProgramNv = 0x8C26,
    }

    public enum ShaderParameter : uint
    {
        ShaderType = 0x8B4F,
        DeleteStatus = 0x8B80,
        CompileStatus = 0x8B81,
        InfoLogLength = 0x8B84,
        ShaderSourceLength = 0x8B88,
    }

    public enum ShaderType : uint
    {
        FragmentShader = 0x8B30,
        VertexShader = 0x8B31,
        GeometryShader = 0x8DD9,
        GeometryShaderExt = 0x8DD9,
    }

    public enum FogMode : uint
    {
        Exp = 0x0800,
        Exp2 = 0x0801,
        Linear = 0x2601,
        FogFuncSgis = 0x812A,
        FogCoord = 0x8451,
        FragmentDepth = 0x8452,
    }

    public enum FogParameter : uint
    {
        FogIndex = 0x0B61,
        FogDensity = 0x0B62,
        FogStart = 0x0B63,
        FogEnd = 0x0B64,
        FogMode = 0x0B65,
        FogColor = 0x0B66,
        FogOffsetValueSgix = 0x8199,
        FogCoordSrc = 0x8450,
    }

    public enum FogPointerType : uint
    {
        Float = 0x1406,
        Double = 0x140A,
        HalfFloat = 0x140B,
    }
}
