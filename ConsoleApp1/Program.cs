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
        public void UpdateQuantity(int amount)
        {
            if (Quantity + amount < 0)
                throw new InvalidOperationException("Недостаточно товара на складе.");
            Quantity += amount;
        }
        public override string ToString()
        {
            string stockStatus = IsInStock ? "Есть в наличии" : "Нет на складе";
            return string.Format("[ID: {0}] {1} | Категория: {2} | Цена: {3:C} | Кол-во: {4} шт. ({5})",
                Id, Name, ProductCategory, Price, Quantity, stockStatus);
        }
        private static readonly List<Product> _products = new List<Product>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            SeedData();
            while (true)
            {
                Console.WriteLine("\n--- УЧЁТ ТОВАРОВ В МАГАЗИНЕ ---\n1. Показать все товары\n2. Добавить товар\n3. Удалить товар\n4. Заказать поставку товара\n5. Продать товар\n6. Поиск товаров\n0. Выход");
                Console.Write("Выберите команду: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ShowAllProducts(); break;
                        case "2": AddProduct(); break;
                        case "3": DeleteProduct(); break;
                        case "4": ReplenishProduct(); break;
                        case "5": SellProduct(); break;
                        case "6": SearchProducts(); break;
                        case "0": Console.WriteLine("Программа завершена."); return;
                        default: Console.WriteLine("Неверная команда. Попробуйте еще раз."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }
        private static void SeedData()
        {
            _products.Add(new Product("Смартфон", 45000, 10, Category.Electronics));
            _products.Add(new Product("Молоко", 90, 50, Category.Food));
            _products.Add(new Product("Джинсы", 3500, 15, Category.Clothing));
            _products.Add(new Product("Ноутбук", 80000, 3, Category.Electronics));
            _products.Add(new Product("Роман '1984'", 600, 0, Category.Books));
        }
    }
}
