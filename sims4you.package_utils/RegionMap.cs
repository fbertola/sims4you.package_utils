using System.IO;

namespace TS4SimRipper
{
    internal class ObjectData
    {
        uint objPosition;              //absolute position of mesh data block, usually 44
        uint objLength;

        internal ObjectData(BinaryReader br)
        {
            this.objPosition = br.ReadUInt32();
            this.objLength = br.ReadUInt32();
        }

        internal ObjectData(ObjectData objectDataToClone)
        {
            this.objPosition = objectDataToClone.objPosition;
            this.objLength = objectDataToClone.objLength;
        }

        internal void Write(BinaryWriter bw, int meshBlocksSize)
        {
            bw.Write(this.objPosition);
            bw.Write(8 + meshBlocksSize);
        }
    }
}
