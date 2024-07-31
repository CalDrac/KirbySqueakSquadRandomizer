

using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace KirbySqueakSquadRandomizer.Ram
{
    class KirbySqueakSquadConnector : INintendoDSConnector
    {
        // ---------------------------------------------------------
        long previousPowerId;
        public Dictionary<int, int> scrollToAbility = new Dictionary<int, int>();
        public KirbySqueakSquadConnector()
        {
            scrollToAbility.Add(1, 0);
            scrollToAbility.Add(2, 1);
            scrollToAbility.Add(3, 2);
            scrollToAbility.Add(4, 3);
            scrollToAbility.Add(5, 4);
            scrollToAbility.Add(6, 5);
            scrollToAbility.Add(7, 8);
            scrollToAbility.Add(8, 9);
            scrollToAbility.Add(9, 10);
            scrollToAbility.Add(10, 11);
            scrollToAbility.Add(11, 13);
            scrollToAbility.Add(12, 6);
            scrollToAbility.Add(13, 7);
            scrollToAbility.Add(14, 14);
            scrollToAbility.Add(15, 12);
            scrollToAbility.Add(16, 15);
            scrollToAbility.Add(17, 17);
            scrollToAbility.Add(18, 19);
            scrollToAbility.Add(19, 16);
            scrollToAbility.Add(20, 18);
            scrollToAbility.Add(21, 20);
            scrollToAbility.Add(22, 21);
            scrollToAbility.Add(23, 22);
        }
        /*
         1 - Fire
2 - Ice
3 - Spark
4 - Beam
5 - Tornado 
6 - Parasol
7 - Hammer   scroll | Cutter  ability
8 - Cupid    scroll | Laser   ability  
9 - Cutter   scroll | Bomb    ability
10- Laser    scroll | Wheel   ability
11- Bomb     scroll | UFO     ability
12- Wheel    scroll | Hammer  ability
13- Hi-jump  scroll | Cupid   ability
14- UFO      scroll | Sleep   ability
15- Sleep    scroll | Hi-jump ability 
16- sword
17- ninja    scroll | throw   ability
18- throw    scroll | magic   ability
19- fighting scroll | ninja   ability
20- magic    scroll | fighter ability
21- animal
22- bubble
23- metal
         */

        public override long GetCurrentFrameCount()
        {
            return 0;
        }



        protected override bool Poll()
        {
            int powerId = _ram.ReadInt32(RamBaseAddress + 0x26188C);
            //int hasScroll = _ram.ReadInt32(RamBaseAddress + 0x2618AC);
            int hasScroll = _ram.ReadUint8(RamBaseAddress + 0x256023); //- 268435455 -> BitArray
            BitArray b = new BitArray(new int[] { hasScroll });
            BitArray bTemp = b.RightShift(4);

            hasScroll = _ram.ReadInt32(RamBaseAddress + 0x256024);
            b = new BitArray(new int[] { hasScroll }).LeftShift(4);
            b[0] = bTemp[0];
            b[1] = bTemp[1];
            b[2] = bTemp[2];
            b[3] = bTemp[3];

            if (powerId != 0)
            {
                if (!b[scrollToAbility[powerId]])
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
