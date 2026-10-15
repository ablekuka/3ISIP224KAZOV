using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
    internal class Program
    {
        static List<Book> books = new List<Book>();
        static int idCounter = 1;

        static void Main(string[] args)
        {
            books.Add(new Book { Id = idCounter++, Title = "Война и мир", Author = "Лев Толстой", Genre = "Роман", Year = 1869, Price = 1200 });
            books.Add(new Book { Id = idCounter++, Title = "Капитанская дочка", Author = "Александр Пушкин", Genre = "Повесть", Year = 1836, Price = 500 });
            books.Add(new Book { Id = idCounter++, Title = "Преступление и наказание", Author = "Фёдор Достоевский", Genre = "Роман", Year = 1866, Price = 800 });
            books.Add(new Book { Id = idCounter++, Title = "Мёртвые души", Author = "Николай Гоголь", Genre = "Поэма", Year = 1842, Price = 650 });
            books.Add(new Book { Id = idCounter++, Title = "Руслан и Людмила", Author = "Александр Пушкин", Genre = "Поэма", Year = 1820, Price = 450 });
        }
    }
}
