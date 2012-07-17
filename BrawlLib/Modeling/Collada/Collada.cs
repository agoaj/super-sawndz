using System;
using System.IO;
using System.Xml;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;
using System.Collections.Generic;
using BrawlLib.Imaging;
using BrawlLib.Wii.Models;
using System.Windows.Forms;

namespace BrawlLib.Modeling
{
    public unsafe partial class Collada
    {
        static XmlWriterSettings _writerSettings = new XmlWriterSettings() { Indent = true, IndentChars = "\t", NewLineChars = "\r\n", NewLineHandling = NewLineHandling.Replace };
        public static void Serialize(MDL0Node model, string outFile)
        {
            model.Populate();
            model.ApplyCHR(null, 0);

            bool weightNormals;

            DialogResult d = MessageBox.Show("Do you want to export weighted normals?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (d == DialogResult.Cancel)
                return;

            weightNormals = d == DialogResult.Yes;

            using (FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.SequentialScan))
            using (XmlWriter writer = XmlWriter.Create(stream, _writerSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("COLLADA", "http://www.collada.org/2008/03/COLLADASchema");
                writer.WriteAttributeString("version", "1.4.0");

                writer.WriteStartElement("asset");
                writer.WriteStartElement("contributor");
                writer.WriteElementString("authoring_tool", "Brawlbox v0.65c");
                writer.WriteEndElement();
                writer.WriteElementString("up_axis", "Y_UP");
                writer.WriteEndElement();

                //Define images
                WriteImages(model, Path.GetDirectoryName(outFile), writer);

                //Define materials
                WriteMaterials(model, writer);

                //Define effects
                WriteEffects(model, writer);

                //Define geometry
                //Create a geometry object for each polygon
                WriteGeometry(model, writer, weightNormals);

                //Define controllers
                //Each weighted polygon needs a controller, which assigns weights to each vertex.
                WriteControllers(model, writer);

                //Define scenes
                writer.WriteStartElement("library_visual_scenes");
                writer.WriteStartElement("visual_scene");

                //Attach nodes/bones to scene, starting with TopN
                //Specify transform for each node
                //Weighted polygons must use instance_controller
                //Standard geometry uses instance_geometry

                writer.WriteAttributeString("id", "RootNode");
                writer.WriteAttributeString("name", "RootNode");

                //Define bones and geometry instances
                WriteNodes(model, writer);

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteStartElement("scene");
                writer.WriteStartElement("instance_visual_scene");
                writer.WriteAttributeString("url", "#RootNode");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Close();
            }
        }

        private static void WriteImages(MDL0Node model, string path, XmlWriter writer)
        {
            if (model._texList == null)
                return;

            writer.WriteStartElement("library_images");

            foreach (MDL0TextureNode tex in model._texList)
            {
                writer.WriteStartElement("image");
                writer.WriteAttributeString("id", tex.Name + "-image");
                writer.WriteAttributeString("name", tex.Name);
                writer.WriteStartElement("init_from");

                string outPath = String.Format("{0}/{1}.png", path.Replace('\\', '/'), tex.Name);

                //Export image and set full path. Or, cheat...
                writer.WriteString(String.Format("file://{0}", outPath));

                writer.WriteEndElement(); //init_from
                writer.WriteEndElement(); //image
            }

            writer.WriteEndElement(); //library_images
        }

        private static unsafe void WriteMaterials(MDL0Node model, XmlWriter writer)
        {
            ResourceNode node = model._matGroup;
            if (node == null)
                return;

            writer.WriteStartElement("library_materials");

            foreach (MDL0MaterialNode mat in node.Children)
            {
                writer.WriteStartElement("material");
                writer.WriteAttributeString("id", mat._name);
                writer.WriteStartElement("instance_effect");
                writer.WriteAttributeString("url", String.Format("#{0}-fx", mat._name));
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static unsafe void WriteEffects(MDL0Node model, XmlWriter writer)
        {
            ResourceNode node = model._matGroup;
            if (node == null)
                return;

            writer.WriteStartElement("library_effects");

            foreach (MDL0MaterialNode mat in node.Children)
            {
                writer.WriteStartElement("effect");
                writer.WriteAttributeString("id", mat._name + "-fx");
                writer.WriteStartElement("profile_COMMON");
                writer.WriteStartElement("technique");
                writer.WriteAttributeString("sid", "standard");
                writer.WriteStartElement("phong");

                writer.WriteStartElement("diffuse");

                foreach (MDL0MaterialRefNode mr in mat._children)
                {
                    if (mr._texture != null)
                    {
                        writer.WriteStartElement("texture");
                        writer.WriteAttributeString("texture", mr._texture.Name + "-image");
                        writer.WriteAttributeString("texcoord", "TEXCOORD" + (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId));
                        writer.WriteEndElement(); //texture
                    }
                    break; //Only one texture reference allowed, it seems.
                }

                writer.WriteEndElement(); //diffuse

                writer.WriteEndElement(); //phong
                writer.WriteEndElement(); //technique
                writer.WriteEndElement(); //profile
                writer.WriteEndElement(); //effect
            }


            writer.WriteEndElement(); //library
        }

        private static unsafe void WriteGeometry(MDL0Node model, XmlWriter writer, bool weightNorms)
        {
            ResourceNode grp = model._polyGroup;
            if (grp == null)
                return;

            writer.WriteStartElement("library_geometries");

            foreach (MDL0PolygonNode poly in grp.Children)
            {
                PrimitiveManager manager = poly._manager;

                //Geometry
                writer.WriteStartElement("geometry");
                writer.WriteAttributeString("id", poly.Name);
                writer.WriteAttributeString("name", poly.Name);

                //Mesh
                writer.WriteStartElement("mesh");

                //Write vertex data first
                WriteVertices(poly._name, manager._vertices, writer);

                //Face assets
                for (int i = 0; i < 12; i++)
                {
                    if (manager._faceData[i] == null)
                        continue;

                    switch (i)
                    {
                        case 0:
                            break;

                        case 1:
                            WriteNormals(poly._name, (Vector3*)manager._faceData[i].Address, manager._pointCount, writer, weightNorms, manager);
                            break;

                        case 2:
                        case 3:
                            WriteColors(poly._name, (RGBAPixel*)manager._faceData[i].Address, manager._pointCount, i - 2, writer);
                            break;

                        default:
                            WriteUVs(poly._name, (Vector2*)manager._faceData[i].Address, manager._pointCount, i - 4, writer);
                            break;
                    }
                }

                //Vertices
                writer.WriteStartElement("vertices");
                writer.WriteAttributeString("id", poly.Name + "_Vertices");
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "POSITION");
                writer.WriteAttributeString("source", "#" + poly.Name + "_Positions");
                writer.WriteEndElement(); //input
                writer.WriteEndElement(); //vertices

                //Faces
                if (manager._triangles != null)
                    WritePrimitive(poly, manager._triangles, writer);
                if (manager._lines != null)
                    WritePrimitive(poly, manager._lines, writer);
                if (manager._points != null)
                    WritePrimitive(poly, manager._points, writer);

                //Write primitives
                //WritePrimitiveType(poly, GLPrimitiveType.Lines, writer);
                //WritePrimitiveType(poly, GLPrimitiveType.LineStrip, writer);
                //WritePrimitiveType(poly, GLPrimitiveType.Triangles, writer);
                //WritePrimitiveType(poly, GLPrimitiveType.TriangleFan, writer);
                //WritePrimitiveType(poly, GLPrimitiveType.TriangleStrip, writer);

                writer.WriteEndElement(); //mesh
                writer.WriteEndElement(); //geometry
            }

            writer.WriteEndElement();
        }

        private static void WriteVertices(string name, List<Vertex3> vertices, XmlWriter writer)
        {
            bool first = true;

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Positions");

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_PosArr");
            writer.WriteAttributeString("count", (vertices.Count * 3).ToString());

            foreach (Vertex3 v in vertices)
            {
                if (first)
                    first = false;
                else
                    writer.WriteString(" ");

                writer.WriteString(String.Format("{0} {1} {2}", v.WeightedPosition._x, v.WeightedPosition._y, v.WeightedPosition._z));
            }

            writer.WriteEndElement(); //float_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_PosArr");
            writer.WriteAttributeString("count", vertices.Count.ToString());
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }
        private static void WriteNormals(string name, Vector3* pData, int count, XmlWriter writer, bool weight, PrimitiveManager p)
        {
            bool first = true;
            ushort* pIndex = (ushort*)p._indices.Address;
            Vector3 v;

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Normals");

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_NormArr");
            writer.WriteAttributeString("count", (count * 3).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                    first = false;
                else
                    writer.WriteString(" ");

                if (weight && p._vertices[*pIndex]._influence != null)
                    v = p._vertices[*pIndex++]._influence.Matrix.GetRotationMatrix() * *pData++;
                else
                    v = *pData++;

                writer.WriteString(String.Format("{0} {1} {2}", v._x, v._y, v._z));
            }

            writer.WriteEndElement(); //float_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_NormArr");
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "3");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "X");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Y");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "Z");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }
        const float cFactor = 1.0f / 255.0f;
        private static void WriteColors(string name, RGBAPixel* pData, int count, int set, XmlWriter writer)
        {
            bool first = true;

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_Colors" + set.ToString());

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_ColorArr" + set.ToString());
            writer.WriteAttributeString("count", (count * 4).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                    first = false;
                else
                    writer.WriteString(" ");

                writer.WriteString(String.Format("{0} {1} {2} {3}", pData->R * cFactor, pData->G * cFactor, pData->B * cFactor, pData->A * cFactor));
                pData++;
            }

            writer.WriteEndElement(); //int_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_ColorArr" + set.ToString());
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "4");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "R");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "G");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "B");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "A");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static void WriteUVs(string name, Vector2* pData, int count, int set, XmlWriter writer)
        {
            bool first = true;

            //Position source
            writer.WriteStartElement("source");
            writer.WriteAttributeString("id", name + "_UVs" + set.ToString());

            //Array start
            writer.WriteStartElement("float_array");
            writer.WriteAttributeString("id", name + "_UVArr" + set.ToString());
            writer.WriteAttributeString("count", (count * 2).ToString());

            for (int i = 0; i < count; i++)
            {
                if (first)
                    first = false;
                else
                    writer.WriteString(" ");

                //Reverse T component to a top-down form
                writer.WriteString(String.Format("{0} {1}", pData->_x, 1.0 - pData->_y));
                pData++;
            }

            writer.WriteEndElement(); //int_array

            //Technique
            writer.WriteStartElement("technique_common");

            writer.WriteStartElement("accessor");
            writer.WriteAttributeString("source", "#" + name + "_UVArr" + set.ToString());
            writer.WriteAttributeString("count", count.ToString());
            writer.WriteAttributeString("stride", "2");

            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "S");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param
            writer.WriteStartElement("param");
            writer.WriteAttributeString("name", "T");
            writer.WriteAttributeString("type", "float");
            writer.WriteEndElement(); //param

            writer.WriteEndElement(); //accessor
            writer.WriteEndElement(); //technique_common

            writer.WriteEndElement(); //source
        }

        private static unsafe void WritePrimitive(MDL0PolygonNode poly, NewPrimitive prim, XmlWriter writer)
        {
            PrimitiveManager manager = poly._manager;
            int count;
            int elements = 0, stride = 0;
            int set;
            bool first;
            ushort* pData = (ushort*)prim._indices.Address;
            ushort* pVert = (ushort*)poly._manager._indices.Address;

            switch (prim._type)
            {
                case GLPrimitiveType.Triangles:
                    writer.WriteStartElement("triangles");
                    stride = 3;
                    break;

                case GLPrimitiveType.Lines:
                    writer.WriteStartElement("lines");
                    stride = 2;
                    break;

                case GLPrimitiveType.Points:
                    writer.WriteStartElement("points");
                    stride = 1;
                    break;
            }
            count = prim._elementCount / stride;

            if (poly._material != null)
                writer.WriteAttributeString("material", poly._material.Name);

            writer.WriteAttributeString("count", count.ToString());

            for (int i = 0; i < 12; i++)
            {
                if (manager._faceData[i] == null)
                    continue;

                writer.WriteStartElement("input");

                switch (i)
                {
                    case 0:
                        writer.WriteAttributeString("semantic", "VERTEX");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Vertices");
                        break;

                    case 1:
                        writer.WriteAttributeString("semantic", "NORMAL");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Normals");
                        break;

                    case 2:
                    case 3:
                        set = i - 2;
                        writer.WriteAttributeString("semantic", "COLOR");
                        writer.WriteAttributeString("source", "#" + poly._name + "_Colors" + set.ToString());
                        writer.WriteAttributeString("set", set.ToString());
                        break;

                    default:
                        set = i - 4;
                        writer.WriteAttributeString("semantic", "TEXCOORD");
                        writer.WriteAttributeString("source", "#" + poly._name + "_UVs" + set.ToString());
                        writer.WriteAttributeString("set", set.ToString());
                        break;
                }

                writer.WriteAttributeString("offset", elements.ToString());
                writer.WriteEndElement(); //input

                elements++;
            }

            for (int i = 0; i < count; i++)
            {
                writer.WriteStartElement("p");
                first = true;
                for (int x = 0; x < stride; x++)
                {
                    int index = *pData++;
                    for (int y = 0; y < elements; y++)
                    {
                        if (first)
                            first = false;
                        else
                            writer.WriteString(" ");

                        if (y == 0)
                            writer.WriteString(pVert[index].ToString());
                        else
                            writer.WriteString(index.ToString());
                    }
                }
                writer.WriteEndElement(); //p
            }

            writer.WriteEndElement(); //primitive
        }

        private static unsafe void WriteControllers(MDL0Node model, XmlWriter writer)
        {
            if (model._polyList == null)
                return;

            writer.WriteStartElement("library_controllers");

            int g = 0;
            //List<MDL0BoneNode> boneSet = new List<MDL0BoneNode>();

            MDL0BoneNode[] bones = new MDL0BoneNode[model._linker.BoneCache.Length];
            model._linker.BoneCache.CopyTo(bones, 0);

            //foreach (MDL0BoneNode b in model._linker.BoneCache)
            //{
            //    b._nodeIndex = g++;
            //    boneSet.Add(b);
            //}

            List<float> weightSet = new List<float>();
            Matrix m;
            bool first;

            foreach (MDL0PolygonNode poly in model._polyList)
            {
                List<Vertex3> verts = poly._manager._vertices;

                writer.WriteStartElement("controller");
                writer.WriteAttributeString("id", poly.Name + "_Controller");
                writer.WriteStartElement("skin");
                writer.WriteAttributeString("source", "#" + poly.Name);

                writer.WriteStartElement("bind_shape_matrix");

                //Set bind pose matrix
                if (poly._singleBind != null)
                    m = poly._singleBind.Matrix;
                else
                    m = Matrix.Identity;

                float* fPtr = (float*)&m;

                first = true;
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                    {
                        if (first)
                            first = false;
                        else
                            writer.WriteString(" ");
                        writer.WriteValue(fPtr[(x << 2) + y]);
                    }

                writer.WriteEndElement();

                //Get list of used bones and weights

                //int index = 0;
                if (poly._singleBind != null)
                {
                    foreach (BoneWeight w in poly._singleBind.Weights)
                    {
                        //if (!boneSet.Contains(w.Bone))
                        //{
                        //    boneSet.Add(w.Bone);
                        //    w.Bone._nodeIndex = index++;
                        //}
                        if (!weightSet.Contains(w.Weight))
                            weightSet.Add(w.Weight);
                    }
                }
                else
                {
                    foreach (Vertex3 v in verts)
                        foreach (BoneWeight w in v._influence.Weights)
                        {
                            //if (!boneSet.Contains(w.Bone))
                            //{
                            //    boneSet.Add(w.Bone);
                            //    w.Bone._nodeIndex = index++;
                            //}
                            if (!weightSet.Contains(w.Weight))
                                weightSet.Add(w.Weight);
                        }
                }

                //Write joint source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Joints");

                //Node array
                writer.WriteStartElement("Name_array");
                writer.WriteAttributeString("id", poly.Name + "_JointArr");
                //writer.WriteAttributeString("count", boneSet.Count.ToString());
                writer.WriteAttributeString("count", bones.Length.ToString());

                first = true;
                //foreach (MDL0BoneNode b in boneSet)
                foreach (MDL0BoneNode b in bones)
                {
                    if (first)
                        first = false;
                    else
                        writer.WriteString(" ");
                    writer.WriteString(b.Name);
                }
                writer.WriteEndElement(); //Name_array

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", String.Format("#{0}_JointArr", poly.Name));
                //writer.WriteAttributeString("count", boneSet.Count.ToString());
                writer.WriteAttributeString("count", bones.Length.ToString());
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "JOINT");
                writer.WriteAttributeString("type", "Name");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //joint source

                //Inverse matrices source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Matrices");

                writer.WriteStartElement("float_array");
                writer.WriteAttributeString("id", poly.Name + "_MatArr");
                //writer.WriteAttributeString("count", (boneSet.Count * 16).ToString());
                writer.WriteAttributeString("count", (bones.Length * 16).ToString());
                first = true;
                //foreach (MDL0BoneNode b in boneSet)
                foreach (MDL0BoneNode b in bones)
                {
                    m = b.InverseBindMatrix;
                    fPtr = (float*)&m;
                    for (int y = 0; y < 4; y++)
                        for (int x = 0; x < 4; x++)
                        {
                            if (first)
                                first = false;
                            else
                                writer.WriteString(" ");
                            writer.WriteValue(fPtr[(x << 2) + y]);
                        }
                }
                writer.WriteEndElement(); //float_array

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", String.Format("#{0}_MatArr", poly.Name));
                //writer.WriteAttributeString("count", boneSet.Count.ToString());
                writer.WriteAttributeString("count", bones.Length.ToString());
                writer.WriteAttributeString("stride", "16");
                writer.WriteStartElement("param");
                writer.WriteAttributeString("type", "float4x4");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //source

                //Weights source
                writer.WriteStartElement("source");
                writer.WriteAttributeString("id", poly.Name + "_Weights");

                writer.WriteStartElement("float_array");
                writer.WriteAttributeString("id", poly.Name + "_WeightArr");
                writer.WriteAttributeString("count", weightSet.Count.ToString());
                first = true;

                foreach (float f in weightSet)
                {
                    if (first)
                        first = false;
                    else
                        writer.WriteString(" ");
                    writer.WriteValue(f);
                }
                writer.WriteEndElement();

                //Technique
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("accessor");
                writer.WriteAttributeString("source", String.Format("#{0}_WeightArr", poly.Name));
                writer.WriteAttributeString("count", weightSet.Count.ToString());
                writer.WriteStartElement("param");
                writer.WriteAttributeString("type", "float");
                writer.WriteEndElement(); //param
                writer.WriteEndElement(); //accessor
                writer.WriteEndElement(); //technique

                writer.WriteEndElement(); //source

                //Joint bindings
                writer.WriteStartElement("joints");
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "JOINT");
                writer.WriteAttributeString("source", String.Format("#{0}_Joints", poly.Name));
                writer.WriteEndElement(); //input
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "INV_BIND_MATRIX");
                writer.WriteAttributeString("source", String.Format("#{0}_Matrices", poly.Name));
                writer.WriteEndElement(); //input
                writer.WriteEndElement(); //joints

                //Vertex weights, one for each vertex in geometry
                writer.WriteStartElement("vertex_weights");
                writer.WriteAttributeString("count", verts.Count.ToString());
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "JOINT");
                writer.WriteAttributeString("offset", "0");
                writer.WriteAttributeString("source", String.Format("#{0}_Joints", poly.Name));
                writer.WriteEndElement(); //input
                writer.WriteStartElement("input");
                writer.WriteAttributeString("semantic", "WEIGHT");
                writer.WriteAttributeString("offset", "1");
                writer.WriteAttributeString("source", String.Format("#{0}_Weights", poly.Name));
                writer.WriteEndElement(); //input

                writer.WriteStartElement("vcount");
                first = true;
                if (poly._singleBind != null)
                    for (int i = 0; i < verts.Count; i++)
                    {
                        if (first)
                            first = false;
                        else
                            writer.WriteString(" ");
                        writer.WriteString(poly._singleBind.Weights.Length.ToString());
                    }
                else
                    foreach (Vertex3 v in verts)
                    {
                        if (first)
                            first = false;
                        else
                            writer.WriteString(" ");
                        writer.WriteString(v._influence.Weights.Length.ToString());
                    }
                
                writer.WriteEndElement(); //vcount

                writer.WriteStartElement("v");

                first = true;
                if (poly._singleBind != null)
                    for (int i = 0; i < verts.Count; i++)
                        foreach (BoneWeight w in poly._singleBind.Weights)
                        {
                            if (first)
                                first = false;
                            else
                                writer.WriteString(" ");
                            //writer.WriteString(w.Bone._nodeIndex.ToString());
                            writer.WriteString(Array.IndexOf(bones, w.Bone).ToString());
                            writer.WriteString(" ");
                            writer.WriteString(weightSet.IndexOf(w.Weight).ToString());
                        }
                else
                    foreach (Vertex3 v in verts)
                        foreach (BoneWeight w in v._influence.Weights)
                        {
                            if (first)
                                first = false;
                            else
                                writer.WriteString(" ");
                            //writer.WriteString(w.Bone._nodeIndex.ToString());
                            writer.WriteString(Array.IndexOf(bones, w.Bone).ToString());
                            writer.WriteString(" ");
                            writer.WriteString(weightSet.IndexOf(w.Weight).ToString());
                        }
                    
                writer.WriteEndElement(); //v

                writer.WriteEndElement(); //vertex_weights

                writer.WriteEndElement(); //skin
                writer.WriteEndElement(); //controller

                //boneSet.Clear();
                weightSet.Clear();
            }

            writer.WriteEndElement();
        }

        private static unsafe void WriteNodes(MDL0Node model, XmlWriter writer)
        {
            if (model._boneList != null)
                foreach (MDL0BoneNode bone in model._boneList)
                    WriteBone(bone, writer);

            if (model._polyList != null)
                foreach (MDL0PolygonNode poly in model._polyList)
                    if (poly._singleBind == null) //Single bind objects will be written under their bone
                        WritePolyInstance(poly, writer);
        }

        private static unsafe void WriteBone(MDL0BoneNode bone, XmlWriter writer)
        {
            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", bone._name);
            writer.WriteAttributeString("name", bone._name);
            writer.WriteAttributeString("type", "JOINT");

            writer.WriteStartElement("matrix");

            Matrix m = bone._bindState._transform;
            float* p = (float*)&m;
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    if ((x != 0) || (y != 0))
                        writer.WriteValue(" ");
                    writer.WriteValue(p[(x << 2) + y].ToString());
                }

            writer.WriteEndElement(); //matrix

            //Write single-bind geometry
            foreach (MDL0PolygonNode poly in bone._infPolys)
                WritePolyInstance(poly, writer);

            foreach (MDL0BoneNode b in bone.Children)
                WriteBone(b, writer);

            writer.WriteEndElement(); //node
        }

        private static void WritePolyInstance(MDL0PolygonNode poly, XmlWriter writer)
        {
            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", poly.Name);
            writer.WriteAttributeString("name", poly.Name);

            if (poly._singleBind != null)
            {
                //Doesn't create a skin modifier, but works
                writer.WriteStartElement("instance_geometry");
                writer.WriteAttributeString("url", String.Format("#{0}", poly.Name));
            }
            else
            {
                writer.WriteStartElement("instance_controller");
                writer.WriteAttributeString("url", String.Format("#{0}_Controller", poly.Name));
            }

            //writer.WriteStartElement("skeleton");
            //writer.WriteString("#" + poly.Model._linker.BoneCache[0].Name);
            //writer.WriteEndElement();

            if (poly._material != null)
            {
                writer.WriteStartElement("bind_material");
                writer.WriteStartElement("technique_common");
                writer.WriteStartElement("instance_material");
                writer.WriteAttributeString("symbol", poly._material.Name);
                writer.WriteAttributeString("target", "#" + poly._material.Name);

                foreach (MDL0MaterialRefNode mr in poly._material.Children)
                {
                    writer.WriteStartElement("bind_vertex_input");
                    writer.WriteAttributeString("semantic", "TEXCOORD" + (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId)); //Replace with true set id
                    writer.WriteAttributeString("input_semantic", "TEXCOORD");
                    writer.WriteAttributeString("input_set", (mr.TextureCoordId < 0 ? 0 : mr.TextureCoordId).ToString()); //Replace with true set id
                    writer.WriteEndElement(); //bind_vertex_input

                    break; //Only one texture reference allowed, it seems.
                }

                writer.WriteEndElement(); //instance_material
                writer.WriteEndElement(); //technique_common
                writer.WriteEndElement(); //bind_material
            }

            writer.WriteEndElement(); //instance_geometry
            writer.WriteEndElement(); //node
        }
    }
}
