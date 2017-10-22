using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class MassiveOfParametersParams
    {
        public void Addition(params int[] integers)
        {
            int result = 0;
            for(int i = 0; i < integers.Length; i++)
            {
                result += integers[i];
            }
            Console.WriteLine(result);
        }

        public void Addition(int x, params int[] integers)
        {
            int result = 0;
            for(int i = 0; i < integers.Length; i++)
            {
                result += integers[i];
                result += x;
            }
            Console.WriteLine(result);
        }

        public void AdditionMassive(int[] integersM, params int[] integers)
        {
            Console.WriteLine("integersM: " + integersM.Length + "; integers: " + integers.Length);
        }

        public void AddtionString(int x, string y, params string[] strings)
        {
            string s = "";
            foreach(string m in strings)
            {
                s += m;
            }
            Console.WriteLine(s + " int x: " + x + " string y: " + y);
        }
    }
}
