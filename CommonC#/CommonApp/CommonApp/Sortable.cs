using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class Sortable
    {
        public void Sort()
        {
            int[] nums = new int[7];
            Console.WriteLine("Enter 7 numbers");
            for (int i = 0; i < nums.Length; i++)
            {
                Console.WriteLine($"{i+1}-e number: ");
                nums[i] = Int32.Parse(Console.ReadLine());
            }

            int temp;
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for(int j = i + 1; j < nums.Length; j++)
                {
                    if(nums[i] > nums[j])
                    {
                        temp = nums[i];
                        nums[i] = nums[j];
                        nums[j] = temp;
                    }
                }
            }

            Console.WriteLine("Output sortable massive");
            for(int i = 0; i < nums.Length; i++)
            {
                Console.WriteLine(nums[i]);
            }
            Console.ReadLine();
        }
    }   
}
