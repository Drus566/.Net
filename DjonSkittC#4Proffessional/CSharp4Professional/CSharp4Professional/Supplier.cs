using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp4Professional
{
    class Supplier
    {
        private readonly string name;
        private readonly int supplierId;
        public string Name { get { return name; } }
        public int SupplierID { get { return supplierId; } }

        public Supplier() { }

        public Supplier(string name, int supplierId)
        {
            this.name = name;
            this.supplierId = supplierId;
        }

        public List<Supplier> GetSampleSuppliers()
        {
            return new List<Supplier>
            {
                new Supplier(name: "Solely Sondheim", supplierId: 1),
                new Supplier(name: "CD-by-CD-by-Sondheim", supplierId: 2),
                new Supplier(name: "Barbershop CDs", supplierId: 3)
            };
        }

        public void TestLinq()
        {
            Product product = new Product();
            var filtered = from p in product.GetSampleProducts()
                           join s in GetSampleSuppliers()
                           on p.SupplierID equals s.SupplierID
                           where p.Price > 10
                           orderby s.Name, p.Name
                           select new { SupplierName = s.Name, ProductName = p.Name };
            foreach(var v in filtered)
            {
                Console.WriteLine("Supplier = {0}, Product = {1}",v.SupplierName, v.ProductName);
            }
        }
    }
}
