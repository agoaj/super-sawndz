using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using BrawlLib.Wii.Models;

namespace BrawlLib.Modeling
{
    #region PMD Importer & Exporter
    public class PMDModel
    {
        #region Main Importer
        public static MDL0Node ImportModel(string filepath)
        {
            filepath = Path.GetFullPath(filepath);
            if (!File.Exists(filepath))
                throw new FileNotFoundException("PMD model file " + filepath + " not found.");

            MDL0Node model = null;
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                BinaryReader reader = new BinaryReader(fs);
                string magic = encoder.GetString(reader.ReadBytes(3));
                if (magic != "Pmd")
                    throw new FileLoadException("Model format not recognized.");

                float version = BitConverter.ToSingle(reader.ReadBytes(4), 0);
                if (version == 1.0f)
                {
                    model = new MDL0Node();
                    model.InitGroups();
                }
                else
                    throw new Exception("Version " + version.ToString() + " models are not supported.");

                if (model != null)
                {
                    Read(reader, CoordinateType.LeftHanded); //Will flip model backwards if right handed
                    PMD2MDL0(model);
                    model.Rebuild(true);
                }
                fs.Close();
            }
            return model;
        }
        #endregion

        #region Main Exporter
        public static void Export(MDL0Node model, string filename)
        {
            filename = Path.GetFullPath(filename);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                BinaryWriter writer = new BinaryWriter(fs);
                writer.Write(PMDModel.encoder.GetBytes("Pmd"));
                writer.Write(1.0f); //Version
                PMDModel.MDL02PMD(model);
                PMDModel.Write(writer);
                fs.Close();
            }
        }
        #endregion

        #region Data Handlers
        internal static Encoding encoder = Encoding.GetEncoding("shift-jis");
        internal static string GetString(byte[] bytes)
        {
            int i;
            for (i = 0; i < bytes.Length; i++)
                if (bytes[i] == 0)
                    break;
            if (i < bytes.Length)
                return encoder.GetString(bytes, 0, i);
            return encoder.GetString(bytes);
        }
        internal static byte[] GetBytes(string input, long size)
        {
            byte[] result = new byte[size];
            for (long i = 0; i < size; i++)
                result[i] = 0;
            if (input == "")
                return result;
            byte[] strs = encoder.GetBytes(input);
            for (long i = 0; i < strs.LongLength; i++)
                if (i < result.LongLength)
                    result[i] = strs[i];
            if (result.LongLength <= strs.LongLength)
                return result;
            result[strs.LongLength] = 0;
            for (long i = strs.LongLength + 1; i < result.Length; i++)
                result[i] = 0xFD;
            return result;
        }
        public enum CoordinateType
        {
            //MMD standard coordinate system
            RightHanded = 1,
            //XNA standard coordinate system
            LeftHanded = -1,
        }
        #endregion

        #region Members and Properties

        public static float Version
        {
            get { return 1.0f; }
        }

        public static ModelHeader Header { get; set; }

        public static ModelVertex[] Vertexes { get; set; }

        public static UInt16[] FaceVertexes { get; set; }

        public static ModelMaterial[] Materials { get; set; }

        public static ModelBone[] Bones { get; set; }

        public static ModelIK[] IKs { get; set; }

        public static ModelSkin[] Skins { get; set; }//表情リスト

        public static UInt16[] SkinIndex { get; set; }

        public static ModelBoneDispName[] BoneDispNames { get; set; }//ボーン枠用枠名リスト

        public static ModelBoneDisp[] BoneDisps { get; set; }//ボーン枠用表示リスト

        public static bool Expansion { get; set; }

        public static bool ToonExpansion { get; set; }

        public static string[] ToonFileNames { get; protected set; }//トゥーンテクスチャリスト(拡張)、10個固定
        const int NumToonFileName = 10;

        public static bool PhysicsExpansion { get; set; }

        public static ModelRigidBody[] RigidBodies { get; set; }//物理演算、剛体リスト(拡張)

        public static ModelJoint[] Joints { get; set; }//物理演算、ジョイントリスト(拡張)

        public static CoordinateType Coordinate { get; protected set; }

        static float CoordZ { get { return (float)Coordinate; } }

        #endregion

        #region Main Data Reader & Writer
        public static void Read(BinaryReader reader, CoordinateType coordinate)
        {
            Coordinate = coordinate;

            Header = new ModelHeader();
            Header.Read(reader);

            //Read Vertices
            UInt32 num_vertex = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Vertexes = new ModelVertex[num_vertex];
            for (UInt32 i = 0; i < num_vertex; i++)
            {
                Vertexes[i] = new ModelVertex();
                Vertexes[i].Read(reader, CoordZ);
            }

            //Read Primitives
            UInt32 face_vert_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            FaceVertexes = new UInt16[face_vert_count];
            for (UInt32 i = 0; i < face_vert_count; i++)
                FaceVertexes[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            
            //Read Materials
            UInt32 material_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            Materials = new ModelMaterial[material_count];
            for (UInt32 i = 0; i < material_count; i++)
            {
                Materials[i] = new ModelMaterial();
                Materials[i].Read(reader);
            }

            //Read Bones
            UInt16 bone_count = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            Bones = new ModelBone[bone_count];
            for (UInt16 i = 0; i < bone_count; i++)
            {
                Bones[i] = new ModelBone();
                Bones[i].Read(reader, CoordZ);
            }

            //Read IK Bones
            UInt16 ik_count = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            IKs = new ModelIK[ik_count];
            for (UInt16 i = 0; i < ik_count; i++)
            {
                IKs[i] = new ModelIK();
                IKs[i].Read(reader);
            }

            //Read Face Morphs
            UInt16 skin_count = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            Skins = new ModelSkin[skin_count];
            for (UInt16 i = 0; i < skin_count; i++)
            {
                Skins[i] = new ModelSkin();
                Skins[i].Read(reader, CoordZ);
            }

            //Read face morph indices
            byte skin_disp_count = reader.ReadByte();
            SkinIndex = new UInt16[skin_disp_count];
            for (byte i = 0; i < SkinIndex.Length; i++)
                SkinIndex[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);

            //Read bone morph names
            byte bone_disp_name_count = reader.ReadByte();
            BoneDispNames = new ModelBoneDispName[bone_disp_name_count];
            for (byte i = 0; i < BoneDispNames.Length; i++)
            {
                BoneDispNames[i] = new ModelBoneDispName();
                BoneDispNames[i].Read(reader);
            }

            //Read bone morphs
            UInt32 bone_disp_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            BoneDisps = new ModelBoneDisp[bone_disp_count];
            for (UInt32 i = 0; i < BoneDisps.Length; i++)
            {
                BoneDisps[i] = new ModelBoneDisp();
                BoneDisps[i].Read(reader);
            }

            //Read English strings, if there are any.
            try
            {
                Expansion = (reader.ReadByte() != 0);
                if (Expansion)
                {
                    Header.ReadExpansion(reader);
                    for (UInt16 i = 0; i < bone_count; i++)
                    {
                        Bones[i].ReadExpansion(reader);
                    }
                    for (UInt16 i = 0; i < skin_count; i++)
                    {
                        if (Skins[i].SkinType != 0)
                            Skins[i].ReadExpansion(reader);
                    }
                    for (byte i = 0; i < BoneDispNames.Length; i++)
                    {
                        BoneDispNames[i].ReadExpansion(reader);
                    }
                    if (reader.BaseStream.Position >= reader.BaseStream.Length)
                        ToonExpansion = false;
                    else
                    {
                        ToonExpansion = true;
                        ToonFileNames = new string[NumToonFileName];
                        for (int i = 0; i < ToonFileNames.Length; i++)
                        {
                            ToonFileNames[i] = GetString(reader.ReadBytes(100));
                        }
                    }
                    if (reader.BaseStream.Position >= reader.BaseStream.Length)
                        PhysicsExpansion = false;
                    else
                    {
                        PhysicsExpansion = true;
                        UInt32 rididbody_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                        RigidBodies = new ModelRigidBody[rididbody_count];
                        for (UInt32 i = 0; i < rididbody_count; i++)
                        {
                            RigidBodies[i] = new ModelRigidBody();
                            RigidBodies[i].ReadExpansion(reader, CoordZ);
                        }
                        UInt32 joint_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                        Joints = new ModelJoint[joint_count];
                        for (UInt32 i = 0; i < joint_count; i++)
                        {
                            Joints[i] = new ModelJoint();
                            Joints[i].ReadExpansion(reader, CoordZ);
                        }
                    }
                }
            }
            catch { Console.WriteLine("This file does not contain English strings."); }
        }
        public static void Write(BinaryWriter writer)
        {
            //通常ヘッダ書きだし(英語ヘッダはBoneIndexの後(ミクなら0x00071167)に書かれている
            if (Header != null)
                Header.Write(writer);
            //頂点リスト書きだし
            if (Vertexes == null)
                writer.Write((UInt32)0);
            else
            {
                writer.Write((UInt32)Vertexes.LongLength);
                for (UInt32 i = 0; i < Vertexes.LongLength; i++)
                {
                    if (Vertexes[i] == null)
                        throw new ArgumentNullException("Vertexes[" + i.ToString() + "] is null!");
                    Vertexes[i].Write(writer, CoordZ);
                }
            }
            //面リスト書きだし
            if (FaceVertexes == null)
                writer.Write((UInt32)0);
            else
            {
                writer.Write((UInt32)FaceVertexes.LongLength);
                for (UInt32 i = 0; i < FaceVertexes.LongLength; i++)
                {
                    writer.Write(FaceVertexes[i]);
                }
            }
            //材質リスト書きだし
            if (Materials == null)
                writer.Write((UInt32)0);
            else
            {
                writer.Write((UInt32)Materials.LongLength);
                for (UInt32 i = 0; i < Materials.LongLength; i++)
                {
                    if (Materials[i] == null)
                        throw new ArgumentNullException("Materials[" + i.ToString() + "] is null!");
                    Materials[i].Write(writer);
                }
            }
            //ボーンリスト書きだし
            if (Bones == null)
                writer.Write((UInt16)0);
            else
            {
                writer.Write((UInt16)Bones.Length);
                for (UInt16 i = 0; i < Bones.Length; i++)
                {
                    if (Bones[i] == null)
                        throw new ArgumentNullException("Bones[" + i.ToString() + "] is null!");
                    Bones[i].Write(writer, CoordZ);
                }
            }
            //IKリスト書きだし
            if (IKs == null)
                writer.Write((UInt16)0);
            else
            {
                writer.Write((UInt16)IKs.Length);
                for (UInt16 i = 0; i < IKs.Length; i++)
                {
                    if (IKs[i] == null)
                        throw new ArgumentNullException("IKs[" + i.ToString() + "] is null!");
                    IKs[i].Write(writer);
                }
            }
            //表情リスト書きだし
            if (Skins == null)
                writer.Write((UInt16)0);
            else
            {
                writer.Write((UInt16)Skins.Length);
                for (UInt16 i = 0; i < Skins.Length; i++)
                {
                    if (Skins[i] == null)
                        throw new ArgumentNullException("Skins[" + i.ToString() + "] is null!");
                    Skins[i].Write(writer, CoordZ);
                }
            }
            //表情枠用表示リスト書きだし
            if (SkinIndex == null)
                writer.Write((byte)0);
            else
            {
                writer.Write((byte)SkinIndex.Length);

                for (byte i = 0; i < SkinIndex.Length; i++)
                {
                    writer.Write(SkinIndex[i]);
                }
            }
            //ボーン枠用枠名リスト
            if (BoneDispNames == null)
                writer.Write((byte)0);
            else
            {
                writer.Write((byte)BoneDispNames.Length);
                for (byte i = 0; i < BoneDispNames.Length; i++)
                {
                    if (BoneDispNames[i] == null)
                        throw new ArgumentNullException("BoneDispNames[" + i.ToString() + "] is null!");
                    BoneDispNames[i].Write(writer);
                }
            }
            //ボーン枠用表示リスト
            if (BoneDisps == null)
                writer.Write((UInt32)0);
            else
            {
                writer.Write((UInt32)BoneDisps.Length);
                for (UInt32 i = 0; i < BoneDisps.Length; i++)
                {
                    if (BoneDisps[i] == null)
                        throw new ArgumentNullException("BoneDisps[" + i.ToString() + "] is null!");
                    BoneDisps[i].Write(writer);
                }
            }
            //英語表記フラグ
            writer.Write((byte)(Expansion ? 1 : 0));
            if (Expansion)
            {
                //英語ヘッダ
                Header.WriteExpansion(writer);
                //ボーンリスト(英語)
                if (Bones != null)
                {
                    for (UInt16 i = 0; i < Bones.Length; i++)
                    {
                        Bones[i].WriteExpansion(writer);
                    }
                }
                //スキンリスト(英語)
                if (Skins != null)
                {
                    for (UInt16 i = 0; i < Skins.Length; i++)
                    {
                        if (Skins[i].SkinType != 0)//baseのスキンには英名無し
                            Skins[i].WriteExpansion(writer);
                    }
                }
                //ボーン枠用枠名リスト(英語)
                if (BoneDispNames != null)
                {
                    for (byte i = 0; i < BoneDispNames.Length; i++)
                    {
                        BoneDispNames[i].WriteExpansion(writer);
                    }
                }
                if (ToonExpansion)
                {
                    //トゥーンテクスチャリスト
                    for (int i = 0; i < ToonFileNames.Length; i++)
                    {
                        writer.Write(GetBytes(ToonFileNames[i], 100));
                    }
                    if (PhysicsExpansion)
                    {
                        //剛体リスト
                        if (RigidBodies == null)
                            writer.Write((UInt32)0);
                        else
                        {
                            writer.Write((UInt32)RigidBodies.LongLength);
                            for (long i = 0; i < RigidBodies.LongLength; i++)
                            {
                                if (RigidBodies[i] == null)
                                    throw new ArgumentNullException("RididBodies[" + i.ToString() + "] is null!");
                                RigidBodies[i].WriteExpansion(writer, CoordZ);
                            }
                        }
                        //ジョイントリスト
                        if (Joints == null)
                            writer.Write((UInt32)0);
                        else
                        {
                            writer.Write((UInt32)Joints.LongLength);
                            for (long i = 0; i < Joints.LongLength; i++)
                            {
                                if (Joints[i] == null)
                                    throw new ArgumentNullException("Joints[" + i.ToString() + "] is null!");
                                Joints[i].WriteExpansion(writer, CoordZ);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region PMD to MDL0
        public static unsafe void PMD2MDL0(MDL0Node model)
        {
            List<MDL0BoneNode> BoneCache = new List<MDL0BoneNode>();

            int index = 0;
            if (!String.IsNullOrWhiteSpace(Header.ModelNameEnglish))
                model.Name = Header.ModelNameEnglish;
            else
                model.Name = Header.ModelName;

            if (!String.IsNullOrWhiteSpace(Header.CommentEnglish))
                MessageBox.Show(Header.CommentEnglish);
            else
                MessageBox.Show(Header.Comment);

            ModelBone prev = null;
            foreach (ModelBone b in Bones)
            {
                MDL0BoneNode bone = new MDL0BoneNode();

                if (!String.IsNullOrWhiteSpace(b.BoneNameEnglish))
                    bone._name = b.BoneNameEnglish;
                else
                    bone._name = b.BoneName;

                bone._entryIndex = index++;

                if (b.ParentBoneIndex != ushort.MaxValue)
                {
                    prev = Bones[b.ParentBoneIndex];
                    foreach (MDL0BoneNode v in model._boneGroup._children)
                        AssignParent(v, b, bone, model, prev);
                }
                else
                {
                    bone.Parent = model._boneGroup;
                    //bone._bindState = new FrameState(new Vector3(1), new Vector3(0), new Vector3(b.BoneHeadPos[0], b.BoneHeadPos[1], b.BoneHeadPos[2]));
                    //bone._bindMatrix = bone._inverseBindMatrix = Matrix.Identity;
                }
                bone.RecalcBindState();
                //bone.GetBindState();
                BoneCache.Add(bone);
                model._influences.AddOrCreateInf(new Influence(bone));
            }

            model._version = 9;
            model._unk4 = model._unk5 = 1;
            model._isImport = true;

            index = 0;
            foreach (ModelMaterial m in Materials)
            {
                MDL0MaterialNode mn = new MDL0MaterialNode();
                mn.Name = "Material" + index++;

                MDL0MaterialRefNode mr = new MDL0MaterialRefNode();
                mr.Name = Path.GetFileNameWithoutExtension(m.TextureFileName);

                if (mr.Name != String.Empty)
                    (mr._texture = model.FindOrCreateTexture(mr.Name))._references.Add(mr);
                else
                    mr.Name = "Diffuse: [R] " + (m.DiffuseColor[0]*255) + " [G] " + (m.DiffuseColor[1]*255) + " [B] " + (m.DiffuseColor[2]*255);
                
                mr._parent = mn;
                mn._children.Add(mr);
                mn._parent = model._matGroup;
                model._matList.Add(mn);
            }

            //To do: Seperate meshes by the texture/color they use and create facedata for diffuse colors.
            PrimitiveManager manager = new PrimitiveManager();
            MDL0PolygonNode p = new MDL0PolygonNode() { _manager = manager, _material = (MDL0MaterialNode)model._matList[0] };
            p._manager._vertices = new List<Vertex3>();
            p.Name = "Mesh";
            p._parent = model._polyGroup;

            p._manager._indices = new UnsafeBuffer(FaceVertexes.Length * 2);
            p._manager._faceData[1] = new UnsafeBuffer(FaceVertexes.Length * 12);
            p._manager._faceData[4] = new UnsafeBuffer(FaceVertexes.Length * 8);

            ushort* Indices = (ushort*)p._manager._indices.Address;
            Vector3* Normals = (Vector3*)p._manager._faceData[1].Address;
            Vector2* UVs = (Vector2*)p._manager._faceData[4].Address;

            manager._triangles = new NewPrimitive(FaceVertexes.Length, OpenGL.GLPrimitiveType.Triangles);
            manager._triangles._indices = new UnsafeBuffer(FaceVertexes.Length * 2);
            ushort* pTri = (ushort*)manager._triangles._indices.Address;

            Influence inf;
            BoneWeight weight1, weight2 = new BoneWeight(null);
            foreach (ModelVertex m in Vertexes)
            {
                weight1 = new BoneWeight();
                weight1.Weight = (float)m.BoneWeight / 100f; //Convert from percentage to decimal
                weight1.Bone = BoneCache[m.BoneNum[0]];
                if (m.BoneNum[1] != m.BoneNum[0])
                {
                    weight2 = new BoneWeight();
                    weight2.Weight = (float)m.BoneWeight / 100f; //Convert from percentage to decimal
                    weight2.Bone = BoneCache[m.BoneNum[1]];
                }
                if (weight2.Bone != null)
                    inf = new Influence(new BoneWeight[] { weight1, weight2 });
                else
                    inf = new Influence(new BoneWeight[] { weight1 });
                
                Vector3 t = new Vector3();
                Vertex3 v;
                t._x = m.Pos[0];
                t._y = m.Pos[1];
                t._z = m.Pos[2];
                if (inf._weights.Length > 1)
                {
                    inf = model._influences.AddOrCreate(inf);
                    v = new Vertex3(Matrix.Identity * t, inf);
                }
                else
                {
                    MDL0BoneNode bone = inf._weights[0].Bone;
                    v = new Vertex3(bone._inverseBindMatrix * Matrix.Identity * t, bone);
                }

                p._manager._vertices.Add(new Vertex3(new Vector3(m.Pos[0], m.Pos[1], m.Pos[2]), inf, new Vector3(m.NormalVector[0], m.NormalVector[1], m.NormalVector[2]), null, new Vector2[] { new Vector2(m.UV[0], m.UV[1]) }));
            }

            index = 0;
            p._manager._pointCount = FaceVertexes.Length;
            foreach (ushort i in FaceVertexes)
            {
                *Indices++ = i;
                *pTri++ = (ushort)index++;
                *Normals++ = p._manager._vertices[i]._normal;
                *UVs++ = p._manager._vertices[i].UV[0];
            }
            model._polyList.Add(p);

            model.CleanGroups();
        }
        public static void AssignParent(MDL0BoneNode v, ModelBone b, MDL0BoneNode bone, MDL0Node model, ModelBone prev)
        {
            if (v._entryIndex == b.ParentBoneIndex)
            {
                bone._parent = v;
                Vector3 p1 = new Vector3(prev.BoneHeadPos[0], prev.BoneHeadPos[1], prev.BoneHeadPos[2]);
                Vector3 p2 = new Vector3(b.BoneHeadPos[0], b.BoneHeadPos[1], b.BoneHeadPos[2]);
                Vector3 angles = new Vector3();
                angles = Matrix.AxisAngleMatrix(p1, p2).GetAngles();
                float distance = p1.DistanceTo(p2);

                bone._bindState = new FrameState(new Vector3(1), angles, p2 - p1);
                
                v._children.Add(bone);
            }
            else //Parent not found, continue searching children.
                foreach (MDL0BoneNode x in v._children)
                    AssignParent(x, b, bone, model, prev);
        }
        #endregion

        #region MDL0 to PMD
        public static unsafe void MDL02PMD(MDL0Node model)
        {
            Header = new ModelHeader();
            Header.ModelName = model.Name;

            //To do: Add the ability to change the comment
            Header.Comment = "PMD model written by Brawlbox v0.65.";

            foreach (MDL0MaterialNode m in model._matList)
            {
                ModelMaterial mat = new ModelMaterial();
                mat.TextureFileName = m.Children[0].Name;
                
            }

            Bones = new ModelBone[model._linker.BoneCache.Length];
            for (int i = 0; i < model._linker.BoneCache.Length; i++)
            {
                ModelBone bone = new ModelBone();
                MDL0BoneNode mBone = (MDL0BoneNode)model._linker.BoneCache[i];

                if (!(mBone.Parent is MDL0GroupNode))
                {
                    bone.BoneHeadPos[0] = mBone._bindState._translate._x + ((MDL0BoneNode)mBone.Parent)._bindState._translate._x;
                    bone.BoneHeadPos[1] = mBone._bindState._translate._y + ((MDL0BoneNode)mBone.Parent)._bindState._translate._y;
                    bone.BoneHeadPos[2] = mBone._bindState._translate._z + ((MDL0BoneNode)mBone.Parent)._bindState._translate._z;
                }
                else
                {
                    bone.BoneHeadPos[0] = mBone._bindState._translate._x;
                    bone.BoneHeadPos[1] = mBone._bindState._translate._y;
                    bone.BoneHeadPos[2] = mBone._bindState._translate._z;
                }

                bone.BoneName = mBone.Name;

                bone.BoneType = 0;
                bone.ParentBoneIndex = (ushort)model._linker.BoneCache.ToList<ResourceNode>().IndexOf(mBone.Parent);

                Bones[i] = bone;
            }
        }
        #endregion
    }
    #endregion

    #region PMD Classes

    #region Model Header
    public class ModelHeader
    {
        public string ModelName { get; set; }

        public string Comment { get; set; }

        public string ModelNameEnglish { get; set; }

        public string CommentEnglish { get; set; }

        public ModelHeader()
        {
            ModelName = "";
            Comment = "";
            ModelNameEnglish = null;
            CommentEnglish = null;
        }

        internal void Read(BinaryReader reader)
        {
            ModelName = PMDModel.GetString(reader.ReadBytes(20));
            Comment = PMDModel.GetString(reader.ReadBytes(256));
        }

        internal void ReadExpansion(BinaryReader reader)
        {
            ModelNameEnglish = PMDModel.GetString(reader.ReadBytes(20));
            CommentEnglish = PMDModel.GetString(reader.ReadBytes(256));
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(ModelName, 20));
            writer.Write(PMDModel.GetBytes(Comment, 256));
        }

        internal void WriteExpansion(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(ModelNameEnglish, 20));
            writer.Write(PMDModel.GetBytes(CommentEnglish, 256));
        }
    }
    #endregion

    #region Model Vertex
    public class ModelVertex
    {
        public float[] Pos { get; private set; } // x, y, z // 座標

        public float[] NormalVector { get; private set; } // nx, ny, nz // 法線ベクトル

        public float[] UV { get; private set; } // u, v // UV座標 // MMDは頂点UV

        public UInt16[] BoneNum { get; private set; } // ボーン番号1、番号2 // モデル変形(頂点移動)時に影響

        public byte BoneWeight { get; private set; } // ボーン1に与える影響度 // min:0 max:100 // ボーン2への影響度は、(100 - bone_weight)

        public byte EdgeFlag { get; private set; } // 0:通常、1:エッジ無効 // エッジ(輪郭)が有効の場合

        public ModelVertex()
        {
            Pos = new float[3];
            NormalVector = new float[3];
            UV = new float[2];
            BoneNum = new UInt16[2];
            BoneWeight = 0;
            EdgeFlag = 0;
        }

        internal void Read(BinaryReader reader, float CoordZ)
        {
            Pos = new float[3];
            NormalVector = new float[3];
            UV = new float[2];
            BoneNum = new UInt16[2];
            for (int i = 0; i < Pos.Length; i++)
                Pos[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < NormalVector.Length; i++)
                NormalVector[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < UV.Length; i++)
                UV[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < BoneNum.Length; i++)
                BoneNum[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            BoneWeight = reader.ReadByte();
            EdgeFlag = reader.ReadByte();
            Pos[2] = Pos[2] * CoordZ;
            NormalVector[2] = NormalVector[2] * CoordZ;
        }

        internal void Write(BinaryWriter writer, float CoordZ)
        {
            Pos[2] = Pos[2] * CoordZ;
            NormalVector[2] = NormalVector[2] * CoordZ;
            for (int i = 0; i < Pos.Length; i++)
                writer.Write(Pos[i]);
            for (int i = 0; i < NormalVector.Length; i++)
                writer.Write(NormalVector[i]);
            for (int i = 0; i < UV.Length; i++)
                writer.Write(UV[i]);
            for (int i = 0; i < BoneNum.Length; i++)
                writer.Write(BoneNum[i]);
            writer.Write(BoneWeight);
            writer.Write(EdgeFlag);
        }
    }
    #endregion

    #region Model Material
    public class ModelMaterial
    {
        public float[] DiffuseColor { get; private set; } // dr, dg, db // 減衰色

        public float Alpha { get; set; }

        public float Specularity { get; set; }

        public float[] SpecularCcolor { get; private set; } // sr, sg, sb // 光沢色
        
        public float[] MirrorColor { get; private set; } // mr, mg, mb // 環境色(ambient)

        public byte ToonIndex { get; set; } // toon??.bmp // 0.bmp:0xFF, 1(01).bmp:0x00 ・・・ 10.bmp:0x09

        public byte EdgeFlag { get; set; } // 輪郭、影

        public UInt32 FaceVertCount { get; set; } // 面頂点数 // インデックスに変換する場合は、材質0から順に加算

        public string TextureFileName { get; set; } //20byte分char テクスチャファイル名 // 20バイトぎりぎりまで使える(終端の0x00は無くても動く)

        public ModelMaterial()
        {
            DiffuseColor = new float[3];
            SpecularCcolor = new float[3];
            MirrorColor = new float[3];
        }

        internal void Read(BinaryReader reader)
        {
            DiffuseColor = new float[3];
            SpecularCcolor = new float[3];
            MirrorColor = new float[3];
            for (int i = 0; i < DiffuseColor.Length; i++)
                DiffuseColor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Alpha = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Specularity = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < SpecularCcolor.Length; i++)
                SpecularCcolor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < MirrorColor.Length; i++)
                MirrorColor[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            ToonIndex = reader.ReadByte();
            EdgeFlag = reader.ReadByte();
            FaceVertCount = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            TextureFileName = PMDModel.GetString(reader.ReadBytes(20));
        }

        internal void Write(BinaryWriter writer)
        {
            for (int i = 0; i < DiffuseColor.Length; i++)
                writer.Write(DiffuseColor[i]);
            writer.Write(Alpha);
            writer.Write(Specularity);
            for (int i = 0; i < SpecularCcolor.Length; i++)
                writer.Write(SpecularCcolor[i]);
            for (int i = 0; i < MirrorColor.Length; i++)
                writer.Write(MirrorColor[i]);
            writer.Write(ToonIndex);
            writer.Write(EdgeFlag);
            writer.Write(FaceVertCount);
            writer.Write(PMDModel.GetBytes(TextureFileName, 20));
        }
    }
    #endregion

    #region Model Bone
    public class ModelBone
    {
        public string BoneName { get; set; } //20byte分char ボーン名

        public UInt16 ParentBoneIndex { get; set; } // 親ボーン番号(ない場合は0xFFFF)

        public UInt16 TailPosBoneIndex { get; set; } // tail位置のボーン番号(チェーン末端の場合は0xFFFF) // 親：子は1：多なので、主に位置決め用
        
        public byte BoneType { get; set; } // ボーンの種類

        public UInt16 IKParentBoneIndex { get; set; } // IKボーン番号(影響IKボーン。ない場合は0)

        public float[] BoneHeadPos { get; private set; } // x, y, z // ボーンのヘッドの位置

        public string BoneNameEnglish { get; set; }////20byte分char ボーン名(英語、拡張(無い場合はnull))

        public ModelBone()
        {
            BoneHeadPos = new float[3];
        }

        internal void Read(BinaryReader reader, float CoordZ)
        {
            BoneHeadPos = new float[3];
            BoneName = PMDModel.GetString(reader.ReadBytes(20));
            ParentBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            TailPosBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            BoneType = reader.ReadByte();
            IKParentBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            for (int i = 0; i < BoneHeadPos.Length; i++)
                BoneHeadPos[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            //英名拡張はReadではnullにする(あるならReadEngilishで上書きされる)
            BoneNameEnglish = null;
            BoneHeadPos[2] = BoneHeadPos[2] * CoordZ;
        }

        //英名拡張分読み込み
        internal void ReadExpansion(BinaryReader reader)
        {
            BoneNameEnglish = PMDModel.GetString(reader.ReadBytes(20));
        }

        internal void Write(BinaryWriter writer, float CoordZ)
        {
            BoneHeadPos[2] = BoneHeadPos[2] * CoordZ;
            writer.Write(PMDModel.GetBytes(BoneName, 20));
            writer.Write(ParentBoneIndex);
            writer.Write(TailPosBoneIndex);
            writer.Write(BoneType);
            writer.Write(IKParentBoneIndex);
            for (int i = 0; i < BoneHeadPos.Length; i++)
                writer.Write(BoneHeadPos[i]);
        }
        
        internal void WriteExpansion(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(BoneNameEnglish, 20));
        }
    }
    public class ModelBoneDisp
    {
        public UInt16 BoneIndex { get; set; } // 枠用ボーン番号

        public byte BoneDispFrameIndex { get; set; }  // 表示枠番号

        internal void Read(BinaryReader reader)
        {
            BoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            BoneDispFrameIndex = reader.ReadByte();
        }
        internal void Write(BinaryWriter writer)
        {
            writer.Write(BoneIndex);
            writer.Write(BoneDispFrameIndex);
        }
    }
    public class ModelBoneDispName
    {
        public string BoneDispName { get; set; }//ボーン枠用枠名

        public string BoneDispNameEnglish { get; set; }//ボーン枠用枠名(英語、拡張)
        internal void Read(BinaryReader reader)
        {
            BoneDispName = PMDModel.GetString(reader.ReadBytes(50));
            BoneDispNameEnglish = null;
        }
        internal void ReadExpansion(BinaryReader reader)
        {
            BoneDispNameEnglish = PMDModel.GetString(reader.ReadBytes(50));
        }
        internal void Write(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(BoneDispName, 50));
        }

        internal void WriteExpansion(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(BoneDispNameEnglish, 50));
        }
    }
    #endregion

    #region Model Joint
    public class ModelJoint
    {
        public string Name { get; set; } // 諸データ：名称 // 右髪1(char*20)

        public UInt32 RigidBodyA { get; set; } // 諸データ：剛体A

        public UInt32 RigidBodyB { get; set; } // 諸データ：剛体B

        public float[] Position { get; private set; } //float*3 諸データ：位置(x, y, z) // 諸データ：位置合せでも設定可

        public float[] Rotation { get; private set; } //float*3 諸データ：回転(rad(x), rad(y), rad(z))

        public float[] ConstrainPosition1 { get; private set; } //float*3 制限：移動1(x, y, z)

        public float[] ConstrainPosition2 { get; private set; } //float*3 制限：移動2(x, y, z)

        public float[] ConstrainRotation1 { get; private set; } //float*3 制限：回転1(rad(x), rad(y), rad(z))

        public float[] ConstrainRotation2 { get; private set; } //float*3 制限：回転2(rad(x), rad(y), rad(z))

        public float[] SpringPosition { get; private set; } //float*3 ばね：移動(x, y, z)

        public float[] SpringRotation { get; private set; } //float*3 ばね：回転(rad(x), rad(y), rad(z))

        public ModelJoint()
        {
            Position = new float[3];
            Rotation = new float[3];
            ConstrainPosition1 = new float[3];
            ConstrainPosition2 = new float[3];
            ConstrainRotation1 = new float[3];
            ConstrainRotation2 = new float[3];
            SpringPosition = new float[3];
            SpringRotation = new float[3];
        }

        internal void ReadExpansion(BinaryReader reader, float CoordZ)
        {
            Name = PMDModel.GetString(reader.ReadBytes(20));
            RigidBodyA = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            RigidBodyB = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            for (int i = 0; i < Position.Length; i++)
                Position[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < Rotation.Length; i++)
                Rotation[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < ConstrainPosition1.Length; i++)
                ConstrainPosition1[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < ConstrainPosition2.Length; i++)
                ConstrainPosition2[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < ConstrainRotation1.Length; i++)
                ConstrainRotation1[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < ConstrainRotation2.Length; i++)
                ConstrainRotation2[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < SpringPosition.Length; i++)
                SpringPosition[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < SpringRotation.Length; i++)
                SpringRotation[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Position[2] *= CoordZ;
            //メモ：右手→左手では位置が変換される際に一緒に回転成分が変換されるため、回転の変換は必要ない
            //ただし、ジョイントの回転(使用してないっぽい)は変換しておく
            Rotation[0] *= CoordZ;
            Rotation[1] *= CoordZ;
            //ConstrainRotation1[0] *= CoordZ;
            //ConstrainRotation1[1] *= CoordZ;
            //ConstrainRotation2[0] *= CoordZ;
            //ConstrainRotation2[1] *= CoordZ;
            //SpringPosition[2] *= CoordZ;
            //SpringRotation[0] *= CoordZ;
            //SpringRotation[1] *= CoordZ;
        }

        internal void WriteExpansion(BinaryWriter writer, float CoordZ)
        {
            Position[2] *= CoordZ;
            Rotation[0] *= CoordZ;
            Rotation[1] *= CoordZ;
            /*ConstrainRotation1[0] *= CoordZ;
            ConstrainRotation1[1] *= CoordZ;
            ConstrainRotation2[0] *= CoordZ;
            ConstrainRotation2[1] *= CoordZ;
            SpringPosition[2] *= CoordZ;
            SpringRotation[0] *= CoordZ;
            SpringRotation[1] *= CoordZ;*/
            writer.Write(PMDModel.GetBytes(Name, 20));
            writer.Write(RigidBodyA);
            writer.Write(RigidBodyB);
            for (int i = 0; i < Position.Length; i++)
                writer.Write(Position[i]);
            for (int i = 0; i < Rotation.Length; i++)
                writer.Write(Rotation[i]);
            for (int i = 0; i < ConstrainPosition1.Length; i++)
                writer.Write(ConstrainPosition1[i]);
            for (int i = 0; i < ConstrainPosition2.Length; i++)
                writer.Write(ConstrainPosition2[i]);
            for (int i = 0; i < ConstrainRotation1.Length; i++)
                writer.Write(ConstrainRotation1[i]);
            for (int i = 0; i < ConstrainRotation2.Length; i++)
                writer.Write(ConstrainRotation2[i]);
            for (int i = 0; i < SpringPosition.Length; i++)
                writer.Write(SpringPosition[i]);
            for (int i = 0; i < SpringRotation.Length; i++)
                writer.Write(SpringRotation[i]);
        }
    }
    #endregion

    #region Model IK
    public class ModelIK
    {
        public UInt16 IKBoneIndex { get; set; } // IKボーン番号

        public UInt16 IKTargetBoneIndex { get; set; } // IKターゲットボーン番号 // IKボーンが最初に接続するボーン
        //byte ik_chain_length;//読み込んでるが、ik_child_bone_indexで参照可のため、メンバにしない

        public UInt16 Iterations { get; set; } // 再帰演算回数 // IK値1

        public float AngleLimit { get; set; } // IKの影響度 // IK値2

        public UInt16[] IKChildBoneIndex { get; set; } // IK影響下のボーン番号-サイズはik_chain_length

        internal void Read(BinaryReader reader)
        {
            IKBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            IKTargetBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            byte ik_chain_length = reader.ReadByte();
            Iterations = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            AngleLimit = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            IKChildBoneIndex = new UInt16[ik_chain_length];
            for (int i = 0; i < ik_chain_length; i++)
                IKChildBoneIndex[i] = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
        }

        internal void Write(BinaryWriter writer)
        {
            writer.Write(IKBoneIndex);
            writer.Write(IKTargetBoneIndex);
            writer.Write((byte)IKChildBoneIndex.Length);
            writer.Write(Iterations);
            writer.Write(AngleLimit);
            for (int i = 0; i < IKChildBoneIndex.Length; i++)
                writer.Write(IKChildBoneIndex[i]);
        }
    }
    #endregion

    #region Model Skin Etc
    public class ModelSkinVertexData
    {
        //base時＝表情用の頂点の番号(頂点リストにある番号)
        //base以外=表情用の頂点の番号(baseの番号。skin_vert_index)

        public UInt32 SkinVertIndex { get; set; }

        //base時=x, y, z // 表情用の頂点の座標(頂点自体の座標)
        //base以外=x, y, z // 表情用の頂点の座標オフセット値(baseに対するオフセット)

        public float[] SkinVertPos { get; private set; } // 

        public ModelSkinVertexData()
        {
            SkinVertPos = new float[3];
        }

        internal void Read(BinaryReader reader, float CoordZ)
        {
            SkinVertIndex = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            for (int i = 0; i < SkinVertPos.Length; i++)
                SkinVertPos[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            SkinVertPos[2] *= CoordZ;
        }

        internal void Write(BinaryWriter writer, float CoordZ)
        {
            SkinVertPos[2] *= CoordZ;
            writer.Write(SkinVertIndex);
            for (int i = 0; i < SkinVertPos.Length; i++)
                writer.Write(SkinVertPos[i]);
        }
    }

    public class ModelSkin //Face & Other Morphs
    {
        public string SkinName { get; set; } //　表情名(char[20])

        //public UInt32 skin_vert_count { get; set; } // 表情用の頂点数-SkinVertDatasのLengthで参照

        public byte SkinType { get; set; } // 表情の種類(byte) // 0：base、1：まゆ、2：目、3：リップ、4：その他

        public ModelSkinVertexData[] SkinVertDatas { get; set; } // 表情用の頂点のデータ(16Bytes/vert) *skin_vert_count

        public string SkinNameEnglish { get; set; }//表示名(char[20]、英語)(拡張)

        internal void Read(BinaryReader reader, float CoordZ)
        {
            SkinName = PMDModel.GetString(reader.ReadBytes(20));
            UInt32 skin_vert_count = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
            SkinType = reader.ReadByte();
            SkinVertDatas = new ModelSkinVertexData[skin_vert_count];
            for (int i = 0; i < SkinVertDatas.Length; i++)
            {
                SkinVertDatas[i] = new ModelSkinVertexData();
                SkinVertDatas[i].Read(reader, CoordZ);
            }
            SkinNameEnglish = null;
        }

        internal void ReadExpansion(BinaryReader reader)
        {
            SkinNameEnglish = PMDModel.GetString(reader.ReadBytes(20));
        }

        internal void Write(BinaryWriter writer, float CoordZ)
        {
            writer.Write(PMDModel.GetBytes(SkinName, 20));
            writer.Write((UInt32)SkinVertDatas.Length);
            writer.Write(SkinType);
            for (int i = 0; i < SkinVertDatas.Length; i++)
            {
                SkinVertDatas[i].Write(writer, CoordZ);
            }
        }

        internal void WriteExpansion(BinaryWriter writer)
        {
            writer.Write(PMDModel.GetBytes(SkinNameEnglish, 20));
        }
    }
    #endregion

    #region Model Rigid Body
    public class ModelRigidBody
    {
        public string Name { get; set; } // 諸データ：名称 // 頭(20byte char)

        public UInt16 RelatedBoneIndex { get; set; } // 諸データ：関連ボーン番号 // 03 00 == 3 // 頭

        public byte GroupIndex { get; set; } // 諸データ：グループ // 00
        
        public UInt16 GroupTarget { get; set; } // 諸データ：グループ：対象 // 0xFFFFとの差 // 38 FE

        public byte ShapeType { get; set; } // 形状：タイプ(0:球、1:箱、2:カプセル) // 00 // 球

        public float ShapeWidth { get; set; } // 形状：半径(幅) // CD CC CC 3F // 1.6

        public float ShapeHeight { get; set; } // 形状：高さ // CD CC CC 3D // 0.1

        public float ShapeDepth { get; set; } // 形状：奥行 // CD CC CC 3D // 0.1

        public float[] Position { get; protected set; } //float*3 位置：位置(x, y, z)

        public float[] Rotation { get; protected set; } //float*3 位置：回転(rad(x), rad(y), rad(z))

        public float Weight { get; set; } // 諸データ：質量 // 00 00 80 3F // 1.0

        public float LinerDamping { get; set; } // 諸データ：移動減 // 00 00 00 00

        public float AngularDamping { get; set; } // 諸データ：回転減 // 00 00 00 00

        public float Restitution { get; set; } // 諸データ：反発力 // 00 00 00 00

        public float Friction { get; set; } // 諸データ：摩擦力 // 00 00 00 00

        public byte Type { get; set; } // 諸データ：タイプ(0:Bone追従、1:物理演算、2:物理演算(Bone位置合せ)) // 00 // Bone追従

        public ModelRigidBody()
        {
            Position = new float[3];
            Rotation = new float[3];
        }

        internal void ReadExpansion(BinaryReader reader, float CoordZ)
        {
            Name = PMDModel.GetString(reader.ReadBytes(20));
            RelatedBoneIndex = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            GroupIndex = reader.ReadByte();
            GroupTarget = BitConverter.ToUInt16(reader.ReadBytes(2), 0);
            ShapeType = reader.ReadByte();
            ShapeWidth = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            ShapeHeight = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            ShapeDepth = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < Position.Length; i++)
                Position[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            for (int i = 0; i < Rotation.Length; i++)
                Rotation[i] = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Weight = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            LinerDamping = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            AngularDamping = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Restitution = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Friction = BitConverter.ToSingle(reader.ReadBytes(4), 0);
            Type = reader.ReadByte();
            Position[2] *= CoordZ;
            //メモ：右手→左手では位置が変換される際に一緒に回転成分が変換されるため、回転の変換は必要ない……のだが
            //剛体はモデルと違い、位置と回転情報だけなので、回転を変換する必要がある
            Rotation[0] *= CoordZ;
            Rotation[1] *= CoordZ;
        }

        internal void WriteExpansion(BinaryWriter writer, float CoordZ)
        {
            Position[2] *= CoordZ;
            Rotation[0] *= CoordZ;
            Rotation[1] *= CoordZ;
            writer.Write(PMDModel.GetBytes(Name, 20));
            writer.Write(RelatedBoneIndex);
            writer.Write(GroupIndex);
            writer.Write(GroupTarget);
            writer.Write(ShapeType);
            writer.Write(ShapeWidth);
            writer.Write(ShapeHeight);
            writer.Write(ShapeDepth);
            for (int i = 0; i < Position.Length; i++)
                writer.Write(Position[i]);
            for (int i = 0; i < Rotation.Length; i++)
                writer.Write(Rotation[i]);
            writer.Write(Weight);
            writer.Write(LinerDamping);
            writer.Write(AngularDamping);
            writer.Write(Restitution);
            writer.Write(Friction);
            writer.Write(Type);
        }
    }
    #endregion

    #endregion
}
