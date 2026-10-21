using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1

    public abstract class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}
public class Student : Person
{
    public List<Course> Courses { get; set; } = new List<Course>();

    public Student(string name, int age) : base(name, age) { }
}



public class Teacher : Person
{
    public Teacher(string name, int age) : base(name, age) { }
}

public class Course
{
    public string Title { get; set; }
    public Teacher Teacher { get; set; }

    public List<Student> Students { get; set; } = new List<Student>();

    public Course(string title, Teacher teacher)
    {
        Title = title;
        Teacher = teacher;
    }
}

{
    internal class Program
    {
    static List<Student> allStudents = new List<Student>();
    static List<Teacher> allTeachers = new List<Teacher>();
    static List<Course> allCourses = new List<Course>();

    static void Main(string[] args)
    {
        Teacher t1 = new Teacher("Иван Иванов", 40);
        allTeachers.Add(t1);
        allStudents.Add(new Student("Алексей", 20));
        allCourses.Add(new Course("C# Базовый", t1));
        while (true)
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Добавить преподавателя");
            Console.WriteLine("3. Создать курс");
            Console.WriteLine("4. Записать студента на курс");
            Console.WriteLine("5. Показать информацию о курсе");
            Console.WriteLine("6. Показать все списки");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Имя студента: ");
                string name = Console.ReadLine();
                Console.Write("Возраст студента: ");
                int age = int.Parse(Console.ReadLine());

                if (age <= 0 || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ошибка: некорректные данные!");
                    continue;
                }

                allStudents.Add(new Student(name, age));
                Console.WriteLine("Студент добавлен.");
            }
            else if (choice == "2")
            {
                Console.Write("Имя преподавателя: ");
                string name = Console.ReadLine();
                Console.Write("Возраст преподавателя: ");
                int age = int.Parse(Console.ReadLine());

                if (age <= 0 || string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ошибка: некорректные данные!");
                    continue;
                }

                allTeachers.Add(new Teacher(name, age));
                Console.WriteLine("Преподаватель добавлен.");
            }
        }
    }
