using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
    public class VarAndRefTypes
    {
        public void SampleOne()
        {
            State state1 = new State(); // Структура State
            State state2 = new State();
            state2.x = 1;
            state2.y = 2;
            state1 = state2;
            state2.x = 5; // state1.x=1 по-прежнему
            Console.WriteLine(state1.x); // 1
            Console.WriteLine(state2.x); // 5

            Country country1 = new Country(); // Класс Country
            Country country2 = new Country();
            country2.x = 1;
            country2.y = 4;
            country1 = country2;
            country2.x = 7; // теперь и country1.x = 7, так как обе ссылки и country1 и country2 
                            // указывают на один объект в хипе
            Console.WriteLine(country1.x); // 7
            Console.WriteLine(country2.x); // 7

            Console.Read();
        }

        public void SampleTwo()
        {
            State state1 = new State();
            State state2 = new State();

            state2.country = new Country();
            state2.country.x = 5;
            state1 = state2;
            state2.country.x = 8; // теперь и state1.country.x=8, так как state1.country и state2.country
                                  // указывают на один объект в хипе
            Console.WriteLine(state1.country.x); // 8
            Console.WriteLine(state2.country.x); // 8

            Console.Read();
        }

        public void SampleThree()
        {
            Person p = new Person { name = "Tom", age = 23 };
            ChangePerson(p);

            Console.WriteLine(p.name); // Alice
            Console.WriteLine(p.age); // 23

            Console.Read();
        }

        public void SampleFour()
        {
            Person p = new Person { name = "Tom", age = 23 };
            ChangePerson(ref p);

            Console.WriteLine(p.name); // Bill
            Console.WriteLine(p.age); // 45

            Console.Read();
        }

        void ChangePerson(Person person)
        {
            // сработает
            person.name = "Alice";
            // сработает только в рамках данного метода
            person = new Person { name = "Bill", age = 45 };
            Console.WriteLine(person.name); // Bill
        }

        void ChangePerson(ref Person person)
        {
            // сработает
            person.name = "Alice";
            // сработает
            person = new Person { name = "Bill", age = 45 };
        }
    }

    class Person
    {
        public string name;
        public int age;
    }

    struct State
    {
        public int x;
        public int y;
        public Country country;
    }
    class Country
    {
        public int x;
        public int y;
    }
}
