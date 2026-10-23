using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum TypeItem { Potion, Weapon, Armor }

    public class Equipment
    {
        public string Name;
        public TypeItem Type;
        public int Bonus; 

        public Equipment(string name, TypeItem type, int bonus)
        {
            Name = name;
            Type = type;
            Bonus = bonus;
        }
    }

   
    public class Character
    {
        public string Name;
        public int MaxHp;
        public int CurrentHp;
        public int BaseAttack;
        public int BaseDefense;

        public Character(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHp = hp;
            CurrentHp = hp;
            BaseAttack = attack;
            BaseDefense = defense;
        }
    }
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
