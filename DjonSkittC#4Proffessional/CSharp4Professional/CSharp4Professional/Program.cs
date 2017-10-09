using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp4Professional
{
    class Program
    {
        static void Main(string[] args)
        {
            Product p = new Product();
            Supplier s = new Supplier();
            s.TestLinq();
            Console.ReadLine();
        }
    }
}
