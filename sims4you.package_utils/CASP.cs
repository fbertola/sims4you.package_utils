using System.IO;
using System.Text;

namespace TS4SimRipper
{
    public class CASP
    {
        public uint version;
        public uint offset;
        public int presetCount;
        public string partname;
        public float sortPriority;

        public ushort swatchOrder;
        public uint outfitID;
        public uint materialHash;
        public byte parameterFlags;
        public byte parameterFlags2;
        public ulong excludePartFlags;
        public ulong excludePartFlags2;
        public ulong excludeModifierRegionFlags;
        public int tagCount;

        public uint knitFlags;
        public uint price;
        public uint titleKey;
        public uint partDescKey;
        public byte textureSpace;
        public uint bodyType;
        public uint bodySubType;
        AgeGender ageGender;


        public CASP(BinaryReader br)
        {
            br.BaseStream.Position = 0;

            version = br.ReadUInt32();
            offset = br.ReadUInt32();
            presetCount = br.ReadInt32();
            partname = new BinaryReader(br.BaseStream, Encoding.BigEndianUnicode).ReadString();
            sortPriority = br.ReadSingle();
            swatchOrder = br.ReadUInt16();
            outfitID = br.ReadUInt32();
            materialHash = br.ReadUInt32();
            parameterFlags = br.ReadByte();
            if (this.version >= 39) parameterFlags2 = br.ReadByte();
            excludePartFlags = br.ReadUInt64();
            if (version >= 41)
            {
                excludePartFlags2 = br.ReadUInt64();
            }
            if (version > 36)
            {
                excludeModifierRegionFlags = br.ReadUInt64();
            }
            else
            {
                excludeModifierRegionFlags = br.ReadUInt32();
            }
            tagCount = br.ReadInt32();
            int valueLength = version >= 37 ? 4 : 2;
            for (int i = 0; i < tagCount; i++)
            {
                br.ReadUInt16();
                if (valueLength == 4)
                {
                    br.ReadUInt32();
                }
                else
                {
                    br.ReadUInt16();
                }
            }
            if (version >= 0x2B) knitFlags = br.ReadUInt32();
            price = br.ReadUInt32();
            titleKey = br.ReadUInt32();
            partDescKey = br.ReadUInt32();
            textureSpace = br.ReadByte();
            bodyType = br.ReadUInt32();
            bodySubType = br.ReadUInt32();
            ageGender = (AgeGender)br.ReadUInt32();
        }

        public AgeGender age
        {
            get { return (AgeGender)((uint)this.ageGender & 0xFF); }
        }
        public AgeGender gender
        {
            get { return (AgeGender)((uint)this.ageGender & 0xFF00); }
        }
    }
}

