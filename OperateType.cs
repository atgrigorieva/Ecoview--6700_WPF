using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVStudio
{
    public enum OperateType
    {
        Remove = 1,
        Save = 2,
        Print = 3,
        ConertTOA = 4,
        Smooth = 5,
        AVG = 6,
        FirstDeri = 7,
        SecDeri = 8,
        ThirdDeri = 9,
        FourthDeri = 10, // 0x0000000A
        SAddR = 11, // 0x0000000B
        SAddS = 12, // 0x0000000C
        SSubR = 13, // 0x0000000D
        SSubS = 14, // 0x0000000E
        SMulR = 15, // 0x0000000F
        SMulS = 16, // 0x00000010
        SDivR = 17, // 0x00000011
        SDivS = 18, // 0x00000012
        NC = 19, // 0x00000013
        ThreeD = 20, // 0x00000014
        Square = 21, // 0x00000015
        ConvertToT = 22, // 0x00000016
        Load = 23, // 0x00000017
    }
}
