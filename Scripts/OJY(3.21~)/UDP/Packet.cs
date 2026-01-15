using System;
using System.Runtime.InteropServices;

namespace UDP_Car
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct ToServerPacket
    {
        //운용모드
        //public int ControlCommand;
        public int FrameCount;
        public int Command;
        public float Heave_Acc;
        public float Roll_Pos;
        public float Pitch_Pos;
        public int Crash_Dir;
        public int Crash_Amp;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct ToClientPacket
    {
        public int state;

    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct ToPlayerPacket
    {
        #region 인원 수 
        public int playerCount;
        #endregion
    }
}

