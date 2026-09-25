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
        private static void ShowAllProducts()
        {
            if (_products.Count == 0) { Console.WriteLine("Список товаров пуст."); return; }
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
        }
        private static void PrintCategories()
        {
            foreach (var cat in Enum.GetValues(typeof(Category)))
                Console.WriteLine((int)cat + ". " + cat);
        }
        private static void AddProduct()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            decimal price = ReadDecimal("Введите цену товара: ");
            int quantity = ReadInt("Введите начальное количество: ");

            Console.WriteLine("Выберите категорию:");
            PrintCategories();
            int catChoice = ReadInt("Номер категории: ");

            if (!Enum.IsDefined(typeof(Category), catChoice))
            {
                Console.WriteLine("Некорректная категория. Товар не добавлен.");
                return;
            }

            _products.Add(new Product(name, price, quantity, (Category)catChoice));
            Console.WriteLine("Товар успешно добавлен!");
        }

        private static void DeleteProduct()
        {
            int id = ReadInt("Введите ID товара для удаления: ");
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) { Console.WriteLine("Товар с таким ID не найден."); return; }
            _products.Remove(product);
            Console.WriteLine("Товар успешно удален.");
        }
        private static void ReplenishProduct()
        {
            int id = ReadInt("Введите ID товара для поставки: ");
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) { Console.WriteLine("Товар с таким ID не найден."); return; }

            int amount = ReadInt("Введите количество поставляемого товара: ");
            if (amount <= 0) { Console.WriteLine("Количество для поставки должно быть больше нуля."); return; }

            product.UpdateQuantity(amount);
            Console.WriteLine("Поставка успешно оформлена!");
        }

        private static void SellProduct()
        {
            int id = ReadInt("Введите ID товара для продажи: ");
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) { Console.WriteLine("Товар с таким ID не найден."); return; }

            int amount = ReadInt("Введите количество для продажи: ");
            if (amount <= 0) { Console.WriteLine("Количество для продажи должно быть больше нуля."); return; }
            if (product.Quantity < amount) { Console.WriteLine("Невозможно продать. На складе всего " + product.Quantity + " шт."); return; }

            product.UpdateQuantity(-amount);
            Console.WriteLine("Продажа успешно совершена!");
        }

        private static void SearchProducts()
        {
            Console.WriteLine("Критерии поиска:\n1. По ID\n2. По названию\n3. По категории");
            string choice = Console.ReadLine();

            IEnumerable<Product> results = null;

            if (choice == "1")
            {
                int id = ReadInt("Введите ID: ");
                results = _products.Where(p => p.Id == id);
            }
            else if (choice == "2")
            {
                Console.Write("Введите название (или часть названия): ");
                string nameQuery = Console.ReadLine();
                string safeQuery = nameQuery != null ? nameQuery.ToLower() : "";
                results = _products.Where(p => p.Name != null && p.Name.ToLower().Contains(safeQuery));
            }
            else if (choice == "3")
            {
                results = FindByCategory();
            }

            if (results == null) { Console.WriteLine("Неверный критерий."); return; }

            var listResults = results.ToList();
            if (listResults.Count == 0)
            {
                Console.WriteLine("Товары не найдены.");
            }
            else
            {
                Console.WriteLine("\nРезультаты поиска:");
                foreach (var item in listResults)
                {
                    Console.WriteLine(item);
                }
            }
        }

        private static IEnumerable<Product> FindByCategory()
        {
            Console.WriteLine("Выберите категорию:");
            PrintCategories();
            int catChoice = ReadInt("Номер категории: ");
            return _products.Where(p => (int)p.ProductCategory == catChoice);
        }

        private static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (int.TryParse(Console.ReadLine(), out int result)) return result;
                Console.WriteLine("Ошибка ввода! Введите целое число.");
            }
        }

        private static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);
                if (decimal.TryParse(Console.ReadLine(), out decimal result)) return result;
                Console.WriteLine("Ошибка ввода! Введите числовое значение.");
            }
        }
    }
}
