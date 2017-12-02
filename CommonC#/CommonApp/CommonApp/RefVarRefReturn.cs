using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public class RefVarRefReturn
    {
        public void SampleRef()
        {
            int x = 5;
            ref int xRef = ref x;
            Console.WriteLine("xRef: " + xRef + "\nx: " + x);
            xRef = 10;
            Console.WriteLine("xRef: " + xRef + "\nx: " + x);
            x = 25;
            Console.WriteLine("xRef: " + xRef + "\nx: " + x);
        }

        public ref int Find(int number, int[] numbers)
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                if(numbers[i] == number)
                {
                    return ref numbers[i]; 
                }
            }
            throw new IndexOutOfRangeException("Число не найдено");
        }
    }
}
