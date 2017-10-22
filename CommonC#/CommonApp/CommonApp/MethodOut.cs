using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class MethodOut
    {
        public void SampleOut(int x, int y, out int z)
        {
           z = x + y;
        }
    }
}
