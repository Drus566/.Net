using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonApp
{
   struct Book
    {
        public string name;
        public string author;
        public int year;

        public Book(string n, string a, int y)
        {
            name = n;
            author = a;
            year = y;
        }

        public void Info()
        {
            Console.WriteLine($"Книга '{name}' (автор {author}) была издана в {year} году");
        }
    }
}
