using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UVStudio
{
    public enum ComStatus
    {
        Connect = 1,
        SYSTATE = 2,
        ZeroScale = 3,
        Rddcur = 4,
        RECHDCUR = 5,
        GETWL = 6,
        SETWL = 7,
        SETAMP = 8,
        SETSWL = 9,
        SETFSL = 10, // 0x0000000A
        SETCHP = 11, // 0x0000000B
        SETSLIT = 12, // 0x0000000C
        SETSCANWL = 13, // 0x0000000D
        SETMOSPD = 14, // 0x0000000E
        MEASURE = 15, // 0x0000000F
        CALBGND = 16, // 0x00000010
        ENGMODE = 17, // 0x00000011
        SETLAMP = 18, // 0x00000012
        MSETPDET = 19, // 0x00000013
        FWLDET = 20, // 0x00000014
        LENGDET = 21, // 0x00000015
        AMPDET = 22, // 0x00000016
        MOROLL = 23, // 0x00000017
        DETCUVCH = 24, // 0x00000018
        GETWLE = 25, // 0x00000019
        SETSERIAL = 26, // 0x0000001A
        RDSERIAL = 27, // 0x0000001B
        PREHOT = 28, // 0x0000001C
        STOPMEASURE = 29, // 0x0000001D
        SCANBASE = 30, // 0x0000001E
        END = 31, // 0x0000001F
        WLCALIB = 32, // 0x00000020
        RESETWL = 33, // 0x00000021
        RESETFSL = 34, // 0x00000022
        RESETSLIT = 35, // 0x00000023
        RESETCHP = 36, // 0x00000024
        SETDW = 37, // 0x00000025
        WAVESHOW = 38, // 0x00000026
        WLINIT = 39, // 0x00000027
        WAVETUN = 40, // 0x00000028
        SET_FIVEEIGHTFRAME = 41, // 0x00000029
        SELF_JUST = 42, // 0x0000002A
        BD_RATIO_FLUSH = 43, // 0x0000002B
    }
}
