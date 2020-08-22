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

    public enum BodyType        //used in TS4
    {
        All = 0,
        Hat = 1,
        Hair = 2,
        Head = 3,
        Face = 4,
        Body = 5,
        Top = 6,
        Bottom = 7,
        Shoes = 8,
        Accessories = 9,
        Earrings = 0x0A,
        Glasses = 0x0B,
        Necklace = 0x0C,
        Gloves = 0x0D,
        BraceletLeft = 0x0E,
        BraceletRight = 0x0F,
        LipRingLeft = 0x10,
        LipRingRight = 0x11,
        NoseRingLeft = 0x12,
        NoseRingRight = 0x13,
        BrowRingLeft = 0x14,
        BrowRingRight = 0x15,
        RingIndexLeft = 0x16,
        RingIndexRight = 0x17,
        RingThirdLeft = 0x18,
        RingThirdRight = 0x19,
        RingMidLeft = 0x1A,
        RingMidRight = 0x1B,
        FacialHair = 0x1C,
        Lipstick = 0x1D,
        Eyeshadow = 0x1E,
        Eyeliner = 0x1F,
        Blush = 0x20,
        Facepaint = 0x21,
        Eyebrows = 0x22,
        Eyecolor = 0x23,
        Socks = 0x24,
        Mascara = 0x25,
        ForeheadCrease = 0x26,
        Freckles = 0x27,
        DimpleLeft = 0x28,
        DimpleRight = 0x29,
        Tights = 0x2A,
        MoleLeftLip = 0x2B,
        MoleRightLip = 0x2C,
        TattooArmLowerLeft = 0x2D,
        TattooArmUpperLeft = 0x2E,
        TattooArmLowerRight = 0x2F,
        TattooArmUpperRight = 0x30,
        TattooLegLeft = 0x31,
        TattooLegRight = 0x32,
        TattooTorsoBackLower = 0x33,
        TattooTorsoBackUpper = 0x34,
        TattooTorsoFrontLower = 0x35,
        TattooTorsoFrontUpper = 0x36,
        MoleLeftCheek = 0x37,
        MoleRightCheek = 0x38,
        MouthCrease = 0x39,
        SkinOverlay = 0x3A,
        Fur = 0x3B,
        AnimalEars = 0x3C,
        Tail = 0x3D,
        NoseColor = 0x3E,
        SecondaryEyeColor = 0x3F,
        OccultBrow = 0x40,
        OccultEyeSocket = 0x41,
        OccultEyeLid = 0x42,
        OccultMouth = 0x43,
        OccultLeftCheek = 0x44,
        OccultRightCheek = 0x45,
        OccultNeckScar = 0x46,
        SkinDetailScar = 0x47,
        SkinDetailAcne = 0x48
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
