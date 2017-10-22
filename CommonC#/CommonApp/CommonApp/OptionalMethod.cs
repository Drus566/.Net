using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class OptionalMethod
    {
        public int OptionalParam(int x, int y, int z = 3, int d = 5)
        {
            return x + y + z + d;
        }
    }
}
