

using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Collections;

namespace KirbySqueakSquadRandomizer.Ram
{
    class KirbySqueakSquadConnector : INintendoDSConnector
    {
        // ---------------------------------------------------------
        long previousPowerId;
        public KirbySqueakSquadConnector()
        { }

        public override long GetCurrentFrameCount()
        {
            return 0;
        }

        protected override bool Poll()
        {
            int powerId = _ram.ReadInt32(RamBaseAddress + 0x26188C);
            int hasScroll = _ram.ReadInt32(RamBaseAddress + 0x2618AC);
            BitArray b = new BitArray(new int[] { hasScroll });
            if (powerId != 0)
            {
                if (!b[powerId - 1])
                {
                    if (powerId != 0 && powerId != 26)
                    {
                        Thread.Sleep(1500);
                        _ram.WriteBytes(RamBaseAddress + 0x26188C, 1);
                    }
                    else
                    {
                        previousPowerId = powerId;
                    }
                }
            }
            return true;
        }
    }
}
