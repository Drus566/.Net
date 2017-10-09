using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp4Professional
{
    class Product
    {
        readonly int supplierId;
        public int SupplierID { get { return supplierId; } }

        readonly string name;
        public string Name { get { return name; } }

        readonly decimal? price;
        public decimal? Price { get { return price; } }

        public Product(string name, decimal price)
        {
            this.name = name;
            this.price = price;
        }

        public Product(string name, decimal price, int supplierId)
        {
            this.name = name;
            this.price = price;
            this.supplierId = supplierId;
        }

        public Product() { }

        //Необязательный параметр price = null (не обязательно = null)
        public Product(string name, decimal? price = null)
        {
            this.name = name;
            this.price = price;
        }

        public List<Product> GetSampleProducts(){
            return new List<Product>
            {
                new Product(name: "West Side Story", price: 9.99m, supplierId: 1),
                new Product(name: "Assassins", price: 14.99m, supplierId: 2),
                new Product(name: "Frogs", price: 10.99m, supplierId: 1),
                new Product(name: "Sweeney Todd", price: 10.99m, supplierId: 3)
            };
        }
        
        //Сортировка по имени
        public void Sort()
        {
            foreach (Product product in GetSampleProducts().OrderBy(p => p.Name))
            {
                Console.WriteLine(product);
            }
        }

        //Выполнение проверки с помощью лямбда выражения
        public void TestSort()
        {
            foreach(Product product in GetSampleProducts().Where(p => p.Price > 10))
            {
                Console.WriteLine(product);
            }
        }

        //Отображение товаров  с неизвестными ценами
        public void TestNull()
        {
            foreach(Product product in GetSampleProducts().Where(p => p.Price == null))
            {
                Console.WriteLine(product.Name);
            }
        }

        //Фильтрация коллекции с помощью Linq
        public void TestLinq()
        {
            //var filtered это переменная которая станет типом IEnumerable<Product>
            var filtered = from Product p in GetSampleProducts()
                           where p.Price > 10
                           select p;
            foreach(Product product in filtered)
            {
                Console.WriteLine(product);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", name, price);
        }
    }
}
