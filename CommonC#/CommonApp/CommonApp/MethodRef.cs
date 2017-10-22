using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class MethodRef
    {
        public void AdditionRef(ref int x, int y)
        {
            x += y;
            Console.WriteLine("X REF: " + x);
        }

        public void Addition(int x, int y)
        {
            x += y;
            Console.WriteLine("X VAL: " + x);
        }
    }
}
