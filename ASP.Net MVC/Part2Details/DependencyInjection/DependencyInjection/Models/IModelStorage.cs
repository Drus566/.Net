using System.Collections.Generic;

namespace DependencyInjection.Models
{
    public interface IModelStorage
    {
        IEnumerable<Product> Items { get; }
        //Ключ значения в словаре
        Product this[string key] { get; set; }
        bool ContainsKey(string key);
        void RemoveItem(string key);
    }
}
