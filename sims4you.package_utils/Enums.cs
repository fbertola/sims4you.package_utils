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

using System;

namespace TS4SimRipper
{
    [Flags]
    public enum AgeGender : uint
    {
        None = 0,
        Baby = 0x00000001,
        Toddler = 0x00000002,
        Child = 0x00000004,
        Teen = 0x00000008,
        YoungAdult = 0x00000010,
        Adult = 0x00000020,
        Elder = 0x00000040,
        Male = 0x00001000,
        Female = 0x00002000,
        Unisex = 0x00003000
    }

 
    public enum SimRegion : uint
    {
        EYES = 0,
        NOSE,
        MOUTH,
        CHEEKS,
        CHIN,
        JAW,
        FOREHEAD,

        // Modifier-only face regions
        BROWS = 8,
        EARS,
        HEAD,

        // Other face regions
        FULLFACE = 12,

        // Modifier body regions
        CHEST = 14,
        UPPERCHEST,
        NECK,
        SHOULDERS,
        UPPERARM,
        LOWERARM,
        HANDS,
        WAIST,
        HIPS,
        BELLY,
        BUTT,
        THIGHS,
        LOWERLEG,
        FEET,

        // Other body regions
        BODY,
        UPPERBODY,
        LOWERBODY,
        TAIL,
        FUR,
        FORELEGS,
        HINDLEGS,

        //  ALL = LOWERBODY + 1,     // all
    }

    public enum SimSubRegion
    {
        None = 0,
        EarsUp = 1,
        EarsDown = 2,
        TailLong = 3,
        TailRing = 4,
        TailScrew = 5,
        TailStub = 6
    }

    public enum ResourceTypes : uint
    {
        BlendGeometry = 0x067CAA11U,
        CASP = 0x034AEECB,
        RLE2 = 0x3453CF95U,
        RLES = 0xBA856C78,
        DDS = 0x00B2D882,
        DDSuncompressed = 0xB6C8B6A0,
        BoneDelta = 0x0355E0A6U,
        DeformerMap = 0xDB43E069U,
        HotSpotControl = 0x8B18FF6EU,
        NameMap = 0x0166038CU,
        SimModifier = 0xC5F6763EU,
        CASPreset = 0xEAA32ADDU,
        Sculpt = 0x9D1AB874U,
        Thumbnail = 0x5B282D45U,
        TONE = 0x0354796A,
        PeltLayer = 0x26AF8338,
        Rig = 0x8EAF13DE
    }

    public enum BgeoLinkTag : uint
    {
        NoBGEO = 0,
        UseBGEO = 0x30000001U
    }

}
