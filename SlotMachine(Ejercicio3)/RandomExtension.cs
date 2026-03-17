using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SlotMachine_Ejercicio3_
{
    public static class RandomExtension
    {
        public static bool NextBool(this Random random)
        {
            return random.Next(2) == 0;
        }
    }
}
