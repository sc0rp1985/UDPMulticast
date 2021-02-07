using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Utils
    {
        public static ulong mul64hi(ulong x, ulong y)
        {
            ulong accum = ((ulong)(uint)x) * ((ulong)(uint)y);
            accum <<= 32;
            ulong term1 = (x >> 32) * ((ulong)(uint)y);
            ulong term2 = (y >> 32) * ((ulong)(uint)x);
            accum += (uint)term1;
            accum += (uint)term2;
            accum >>= 32;
            accum += (term1 >> 32) + (term2 >> 32);
            accum += (x >> 32) * (y >> 32);
            return accum;
        }

        public static double Average(ulong[] data) 
        {
            ulong sum = 0;
            ulong count = 0;
            for (var i = 0; i < data.Length; i++)
            {
                var di = data[i];
                ulong mul = mul64hi(di, (ulong)i);
                count += di;
                sum += mul;
            }
            return sum / count;
        }
    }
}

