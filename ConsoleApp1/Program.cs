using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement
{
    public enum Category { Electronics = 1, Food, Clothing, Books }

    public class Product
    {
        private static int _globalIdCounter = 1;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public Category ProductCategory { get; private set; }
        public bool IsInStock { get { return Quantity > 0; } }

        public Product(string name, decimal price, int quantity, Category category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым.");
            if (price <= 0)
                throw new ArgumentException("Цена должна быть больше нуля.");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным.");

            Id = _globalIdCounter++;
            Name = name;
            Price = price;
            Quantity = quantity;
            ProductCategory = category;
        }    
        static void Main(string[] args)
        {
        }
    }
}
