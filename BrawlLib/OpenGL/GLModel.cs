using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;

namespace BrawlLib.OpenGL
{
    public unsafe class GLModel
    {
        public List<GLBone> _bones = new List<GLBone>();
        public List<GLPolygon> _polygons = new List<GLPolygon>();

        public List<GLMaterial> _materials = new List<GLMaterial>();
        public List<GLTexture> _textures = new List<GLTexture>();

        public GLNode[] _nodes;

        public Vector3 _min, _max;
        private bool _enabled = true;
        public string _name;

        public GLModel(MDL0Node node)
        {
            ResourceNode n;
            ResourceGroup* group;

            _min = node.BoxMin;
            _max = node.BoxMax;
            _name = node.Name;

            //Extract textures
            if ((n = node.FindChild("Textures1", false)) != null)
                foreach (MDL0TextureNode tex in n.Children)
                    _textures.Add(new GLTexture(this, tex));

            //Extract materials
            if ((n = node.FindChild("Materials1", false)) != null)
                foreach (MDL0MaterialNode mat in n.Children)
                    _materials.Add(new GLMaterial(this, mat));

            //Extract nodes
            int nodeCount = node.NumNodes;
            _nodes = new GLNode[nodeCount];
            for (int i = 0; i < nodeCount; )
                _nodes[i++] = new GLNode();

            ////Extract polygons
            //if ((n = node.FindChild("Polygons", false)) != null)
            //    foreach (MDL0PolygonNode poly in n.Children)
            //        _polygons.Add(ModelConverter.ExtractPolygon(this, new GLPolygon(this, poly)));

            //if ((group = node.Header->PolygonGroup) != null)
            //    for (int i = 0; i < group->_numEntries; i++)
            //        _polygons.Add(ModelConverter.ExtractPolygon(this, (MDL0Polygon*)group->First[i].DataAddress));

            ////Link Opa
            //MDL0DefNode opaNode = node.FindChild("Definitions/DrawOpa", false) as MDL0DefNode;
            //if (opaNode != null)
            //    foreach (MDL0NodeType4 o in opaNode.Items)
            //        _polygons[o.PolygonId]._materials.Add(_materials[o.MaterialId]);

            ////Link Xlu
            //MDL0DefNode xluNode = node.FindChild("Definitions/DrawXlu", false) as MDL0DefNode;
            //if (xluNode != null)
            //    foreach (MDL0NodeType4 o in xluNode.Items)
            //        _polygons[o.PolygonId]._materials.Add(_materials[o.MaterialId]);

            ////Link bones
            //if ((n = node.FindChild("Bones", false)) != null)
            //{
            //    foreach (MDL0BoneNode bone in n.Children)
            //        _bones.Add(ParseBone(bone, opaNode));
            //}

            //MDL0DefNode mixNode = node.FindChild("Definitions/NodeMix", false) as MDL0DefNode;
            //if (mixNode != null)
            //    foreach (object o in mixNode.Items)
            //        if (o is MDL0Node3Class)
            //            _nodes[((MDL0Node3Class)o)._id].Link(this, o as MDL0Node3Class);


            //Cache textures
            foreach (TEX0Node tex in node.RootNode.FindChildrenByType(null, ResourceType.TEX0))
            {
                foreach (GLTexture glt in _textures)
                {
                    if (glt._name == tex.Name)
                    {
                        glt.Attach(tex);
                    }
                }
            }

            Rebuild();
        }


        private void Rebuild()
        {

            //Build bones
            foreach (GLBone bone in _bones)
                bone.Rebuild();

            //Build nodes
            foreach (GLNode node in _nodes)
                if (node != null)
                    node.Rebuild();

            //Build polygons
            foreach (GLPolygon poly in _polygons)
                poly.Rebuild();
        }

        private GLBone ParseBone(MDL0BoneNode node, MDL0DefNode opaNode)
        {
            GLBone bone = new GLBone(node);
            int nid = bone._nodeId;
            //if (_nodes[nid] == null)
            //    _nodes[nid] = new GLNode(this);
            _nodes[nid]._bone = bone;

            //Link materials/polygons
            //if(opaNode != null)
            //    foreach (MDL0NodeType4 o in opaNode.Items)
            //    {
            //        if (o.BoneIndex == bone._index)
            //        {
            //            bone._polygons.Add(_polygons[o.PolygonId]);
            //        }
            //    }

            foreach (MDL0BoneNode n in node.Children)
            {
                GLBone b = ParseBone(n, opaNode);
                bone._children.Add(b);
                b._parent = bone;
            }

            return bone;
        }

        //public void AttachTexture(string texName, Bitmap bmp)
        //{
        //    foreach (GLTexture tex in _textures)
        //        tex.Attach(bmp, texName);
        //}

        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            //Render polygons
            foreach (GLPolygon poly in _polygons)
                poly.Render(context);
        }

        internal unsafe void Unbind(GLContext context)
        {
            foreach (GLTexture tex in _textures)
                tex.Unbind(context);
        }
    }
}
