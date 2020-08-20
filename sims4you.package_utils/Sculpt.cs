/* TS4 SimRipper, a tool for creating custom content for The Sims 4,
   Copyright (C) 2014  C. Marinetti

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
   The author may be contacted at modthesims.info, username cmarNYC. */

using System.IO;

namespace TS4SimRipper
{
    public class Sculpt
    {
        public uint contextVersion;
        public TGI[] publicKey;
        public TGI[] externalKey;
        public TGI[] BGEOKey;
        public ObjectData[] objectKey;

        public uint version;
        public AgeGender ageGender;
        public SimRegion region;
        public SimSubRegion subRegion;
        public BgeoLinkTag linkTag;

        public Sculpt(BinaryReader br)
        {
            this.contextVersion = br.ReadUInt32();
            uint publicKeyCount = br.ReadUInt32();
            uint externalKeyCount = br.ReadUInt32();
            uint delayLoadKeyCount = br.ReadUInt32();
            uint objectKeyCount = br.ReadUInt32();
            this.publicKey = new TGI[publicKeyCount];
            for (int i = 0; i < publicKeyCount; i++) publicKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.externalKey = new TGI[externalKeyCount];
            for (int i = 0; i < externalKeyCount; i++) externalKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.BGEOKey = new TGI[delayLoadKeyCount];
            for (int i = 0; i < delayLoadKeyCount; i++) BGEOKey[i] = new TGI(br, TGI.TGIsequence.ITG);
            this.objectKey = new ObjectData[objectKeyCount];
            for (int i = 0; i < objectKeyCount; i++) objectKey[i] = new ObjectData(br);
            this.version = br.ReadUInt32();
            this.ageGender = (AgeGender)br.ReadUInt32();
            this.region = (SimRegion)br.ReadUInt32();
            this.subRegion = (SimSubRegion)br.ReadUInt32();
        }

        public class ObjectData
        {
            internal uint position;
            internal uint length;

            internal ObjectData(BinaryReader br)
            {
                this.position = br.ReadUInt32();
                this.length = br.ReadUInt32();
            }

        }
    }
}
