using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();

            test.TestRefAndVarTypes();

            Console.ReadLine();
        }
    }

    class Test
    {
        //Необязательные параметры
        public void TestOptionalMethod()
        {
            int x, y, z, d;
            x = y = z = d = 1;
            OptionalMethod oM = new OptionalMethod();
            Console.WriteLine(oM.OptionalParam(x, y));
            Console.WriteLine(oM.OptionalParam(x, y, z));
            Console.WriteLine(oM.OptionalParam(x, y, z, d));
            Console.ReadLine();
        }


        //Именованные параметры
        public void TestOptionalMethodNamedParametr()
        {
            int k, j, i;
            k = 2;
            j = 6;
            i = 1;
            OptionalMethod oM = new OptionalMethod();
            Console.WriteLine(oM.OptionalParam(x:k, y:j, z:i, d:3));
            Console.WriteLine(oM.OptionalParam(d:3, z:i, y:j, x:k));
        }

        //тест слова params и массива параметров
        public void TestParams()
        {
            MassiveOfParametersParams m = new MassiveOfParametersParams();
            m.Addition(5,4,3,2,1);
            int[] array = new int[] { 1, 2, 3, 4 };
            m.Addition(array);
            m.Addition();
            m.AdditionMassive(array, 2,3,4,299);
            m.Addition(228, 3,4,56);
            m.AddtionString(1,"s", "g", "f");
            Console.ReadLine();
        }

        public void TestRecursive()
        {
            RecursiveMethod r = new RecursiveMethod();
            int x = 5;
            int result;
            result = r.Factorial(x);
            Console.WriteLine($"Factorial({x}): " + result);
            result = r.Fibonachi(x);
            Console.WriteLine($"Fibonachi({x}): " + result); 
        }

        public void TestEnums()
        {
            Console.WriteLine(Days.Friday + " : " + (int)Days.Friday);
            Console.WriteLine(Days.Monday + " : " + (int)Days.Monday);
            Console.WriteLine(Days.Saturday + " : " + (int)Days.Saturday);
            Console.WriteLine(Days.Sunday + " : " + (int)Days.Sunday);
            Console.WriteLine(Days.Thursday + " : " + (int)Days.Thursday);
            Console.WriteLine(Days.Tuesday + " : " + (int)Days.Tuesday);
            Console.WriteLine(Days.Wednesday + " : " + (int)Days.Wednesday);
            if((int)Days.Monday == 1)
            {
                Console.WriteLine("this days is 1");
                Console.WriteLine(Days.Monday + " : " + (int)Days.Monday);
            }
            else
            {
                Console.WriteLine("Nope");
            }
            Console.WriteLine("\n---------------------------------------\n");
            Console.WriteLine(Time.Afternoon + " : " + (byte)Time.Afternoon);
            Console.WriteLine(Time.Evening + " : " + (byte)Time.Evening);
            Console.WriteLine(Time.Morning + " : " + (byte)Time.Morning);
            Console.WriteLine(Time.Night + " : " + (byte)Time.Night);

            Enums e = new Enums();
            e.MathOp(1, 2, Operation.Add);
        }

        public void TestStruct()
        {
            Book[] books = new Book[3];
            books[0].name = "Война и мир";
            books[0].author = "Л. Н. Толстой";
            books[0].year = 1869;

            books[1].name = "Преступление и наказание";
            books[1].author = "Ф. М. Достоевский";
            books[1].year = 1866;

            books[2].name = "Отцы и дети";
            books[2].author = "И. С. Тургенев";
            books[2].year = 1862;

            foreach (Book b in books)
            {
                b.Info();
            }

            Book book = new Book("Война и мир", "Л. Н. Толстой", 1869);
            book.Info();
        }

        //тест try catch
        public void TestExcep()
        {
            try
            {
                string message = Console.ReadLine();
                if (message.Length > 6)
                {
                    throw new Exception("Длина строки больше 6 символов");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            int x = 1;
            int y = 0;

            try
            {
                int result = x / y;
            }
            catch (Exception ex) when (y == 0)
            {
                Console.WriteLine("y не должен быть равен 0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        public void TestRef()
        {
            RefVarRefReturn r = new RefVarRefReturn();
            r.SampleRef();
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7 };
            ref int numberRef = ref r.Find(4, numbers);
            numberRef = 9;
            Console.WriteLine(numbers[3]);
        }

        public void TestRefAndVarTypes()
        {
            VarAndRefTypes v = new VarAndRefTypes();
            v.SampleOne();
            v.SampleTwo();
            v.SampleThree();
            v.SampleFour();
        }
    }
}
