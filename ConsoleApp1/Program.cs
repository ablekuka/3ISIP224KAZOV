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
            int sentencesCount = 0;
            int vowelsCount = 0;
            int consonantsCount = 0;

            string vowels = "аеёиоуыэюяАЕЁИОУЫЭЮЯaeiouyAEIOUY";
            string consonants = "бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩbcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ";

            Dictionary<char, int> charFreq = new Dictionary<char, int>();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (c == '.' || c == '!' || c == '?')
                {

                    if (i == text.Length - 1 || (text[i + 1] != '.' && text[i + 1] != '!' && text[i + 1] != '?'))
                    {
                        sentencesCount++;
                    }
                }
                if (char.IsLetter(c))
                {
                    char lowerC = char.ToLower(c);

                    if (charFreq.ContainsKey(lowerC))
                        charFreq[lowerC]++;
                    else
                        charFreq[lowerC] = 1;

                    if (vowels.Contains(c.ToString()))
                        vowelsCount++;
                    else if (consonants.Contains(c.ToString()))
                        consonantsCount++;
                }
            }
            if (sentencesCount == 0 && text.Length > 0)
            {
                sentencesCount = 1;
            }

            char[] separators = { ' ', ',', '.', '!', '?', '-', ';', ':', '(', ')', '"' };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int wordsCount = words.Length;
            string shortestWord = words[0];
            string longestWord = words[0];

            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < shortestWord.Length)
                {
                    shortestWord = words[i];
                }
                if (words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }
            Console.WriteLine("\n--- РЕЗУЛЬТАТ АНАЛИЗА ---");
            Console.WriteLine($"Количество слов: {wordsCount}");
            Console.WriteLine($"Количество предложений: {sentencesCount}");
            Console.WriteLine($"Количество гласных: {vowelsCount}");
            Console.WriteLine($"Количество согласных: {consonantsCount}");
            Console.WriteLine($"Самое короткое слово: {shortestWord}");
            Console.WriteLine($"Самое длинное слово: {longestWord}");

            Console.WriteLine("Частота букв:");
            foreach (var item in charFreq)
            {
                Console.WriteLine($"  Буква '{item.Key}': {item.Value} раз(а)");
            }
        }
    }
}

