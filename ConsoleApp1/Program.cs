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

{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
