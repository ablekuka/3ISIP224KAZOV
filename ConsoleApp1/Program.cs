using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {

        static List<string> historyTexts = new List<string>();
        static List<int> historyWords = new List<int>();
        static List<int> historySentences = new List<int>();
        static List<string> historyShortest = new List<string>();
        static List<string> historyLongest = new List<string>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Ввести новый текст");
                Console.WriteLine("2. Вывести статистику по прошлым текстам");
                Console.WriteLine("3. Выйти");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    ProcessText();
                }
                else if (choice == "2")
                {
                    ShowHistory();
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }
        static void ProcessText()
        {
            Console.Clear();
            string text = "";

            while (true)
            {
                Console.WriteLine("Введите текст (не менее 100 символов):");
                text = Console.ReadLine();

                if (text.Length >= 100)
                {
                    break;
                }
                Console.WriteLine($"Ошибка! Длина вашего текста всего {text.Length} симв. Попробуйте снова.\n");
            }

        }
    }
}
