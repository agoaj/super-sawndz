using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;

namespace BrawlLib.Modeling
{
    public unsafe class Model
    {
        internal List<Bone> _bones = new List<Bone>();
        internal List<Material> _materials = new List<Material>();
        //internal List<Shader> _shaders = new List<Shader>();
        internal List<Polygon> _polygons = new List<Polygon>();

        public List<Bone> Bones { get { return _bones; } }
        public List<Material> Materials { get { return _materials; } }
        //public List<Shader> Shaders { get { return _shaders; } }
        public List<Polygon> Polygons { get { return _polygons; } }

        public static Model Decode(MDL0Header* header)
        {
            Model model = new Model(header);

            //Create linker
            int version = header->_header._version;
            bint* offsets = (bint*)((byte*)header + 0x10);
            List<MDLResourceType> iList = ModelLinker.IndexBank[version];
            ResourceGroup* pGroup;

            //Read bones
            ExtractBones((ResourceGroup*)((byte*)header + offsets[iList.IndexOf(MDLResourceType.Bones)]));

            //Parse defs
            pGroup = (ResourceGroup*)((byte*)header + offsets[iList.IndexOf(MDLResourceType.Definitions)]);

            return model;
        }

        public Model(MDL0Header* pModel)
        {
            int version = pModel->_header._version;
            bint* offsets = (bint*)((byte*)pModel + 0x10);
            List<MDLResourceType> iList = ModelLinker.IndexBank[version];

            List<Bone> boneList = ExtractBones((ResourceGroup*)((byte*)pModel + offsets[iList.IndexOf(MDLResourceType.Bones)]));
        }

        private static List<Bone> ExtractBones(ResourceGroup* pGroup)
        {
            int count = pGroup->_numEntries;
            List<Bone> list = new List<Bone>(count);

            ResourceEntry* pEntry = &pGroup->_first + 1;
            for (int i = 0; i < count; i++)
                list.Add(new Bone((MDL0Bone*)((byte*)pGroup + pEntry->_dataOffset)));

            return list;
        }
    }
}
