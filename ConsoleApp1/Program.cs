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

    public class Player : Character
    {
        public Equipment Weapon;
        public Equipment Armor;
        public bool IsFrozen = false;

        public Player(string name) : base(name, 100, 15, 10)
        {
            Weapon = new Equipment("Старый нож", TypeItem.Weapon, 2);
            Armor = new Equipment("Обноски", TypeItem.Armor, 2);
        }

        public int GetTotalAttack() { return BaseAttack + Weapon.Bonus; }
        public int GetTotalDefense() { return BaseDefense + Armor.Bonus; }

        public void TakeDamageWithDefense(int incomingDamage, Random rand, bool ignoreArmor)
        {
            if (rand.Next(1, 101) <= 40)
            {
                Console.WriteLine($"{Name} уклонился от атаки!");
                return;
            }

            int finalDamage = incomingDamage;

            if (!ignoreArmor)
            {

                double blockPercent = rand.Next(70, 101) / 100.0;
                int blockedDamage = (int)(GetTotalDefense() * blockPercent);
                finalDamage = incomingDamage - blockedDamage;
                if (finalDamage < 0) finalDamage = 0;
                Console.WriteLine($"Защита заблокировала {blockedDamage} урона.");
            }
            else
            {
                Console.WriteLine("awВраг пробивает броню! Защита проигнорирована.");
            }

            CurrentHp -= finalDamage;
            if (CurrentHp < 0) CurrentHp = 0;
            Console.WriteLine($"Нанесено урона: {finalDamage}. Ваше HP: {CurrentHp}/{MaxHp}");
        }
    }

    public class Enemy : Character
    {
        public Enemy(string name, int hp, int attack, int defense) : base(name, hp, attack, defense) { }

        public virtual void Attack(Player player, Random rand, bool playerIsDefending)
        {
            int damage = BaseAttack;
            Console.WriteLine($"{Name} атакует на {damage} урона.");

            if (playerIsDefending)
                player.TakeDamageWithDefense(damage, rand, false);
            else
                player.CurrentHp -= Math.Max(1, damage - player.GetTotalDefense());
        }
    }

    public class Goblin : Enemy
    {
        public int CritChance = 20;
        public Goblin(Random rand) : base("Гоблин", 40, 12, 4) { }
        public Goblin(string name, int hp, int attack, int defense) : base(name, hp, attack, defense) { }

        public override void Attack(Player player, Random rand, bool playerIsDefending)
        {
            int damage = BaseAttack;
            if (rand.Next(1, 101) <= CritChance)
            {
                damage = (int)(damage * 1.5);
                Console.WriteLine($"{Name} наносит КРИТИЧЕСКИЙ удар!");
            }
            Console.WriteLine($"{Name} атакует на {damage} урона.");

            if (playerIsDefending) player.TakeDamageWithDefense(damage, rand, false);
            else player.CurrentHp -= Math.Max(1, damage - player.GetTotalDefense());
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton(Random rand) : base("Скелет", 35, 14, 2) { }
        public Skeleton(string name, int hp, int attack, int defense) : base(name, hp, attack, defense) { }

        public override void Attack(Player player, Random rand, bool playerIsDefending)
        {
            Console.WriteLine($"{Name} атакует в обход брони на {BaseAttack} урона.");
            if (playerIsDefending) player.TakeDamageWithDefense(BaseAttack, rand, true);
            else player.CurrentHp -= BaseAttack;
        }
    }

    public class Mage : Enemy
    {
        public int FreezeChance = 25;
        public Mage(Random rand) : base("Маг", 30, 16, 3) { }
        public Mage(string name, int hp, int attack, int defense) : base(name, hp, attack, defense) { }

        public override void Attack(Player player, Random rand, bool playerIsDefending)
        {
            Console.WriteLine($"{Name} выпускает магический снаряд на {BaseAttack} урона.");
            if (playerIsDefending) player.TakeDamageWithDefense(BaseAttack, rand, false);
            else player.CurrentHp -= Math.Max(1, BaseAttack - player.GetTotalDefense());

            if (rand.Next(1, 101) <= FreezeChance && player.CurrentHp > 0)
            {
                player.IsFrozen = true;
                Console.WriteLine($"{Name} ЗАМОРОЗИЛ ВАС! Вы пропустите ход.");
            }
        }
    }

 
    public class BossVvg : Goblin
    {
        public BossVvg(Random rand) : base("Босс ВВГ", (int)(40 * 2.0), (int)(12 * 1.5), (int)(4 * 1.2))
        {
            CritChance = 20 + 10;
        }
    }

    public class BossKowalski : Skeleton
    {
        public BossKowalski(Random rand) : base("Босс Ковальский", (int)(35 * 2.5), (int)(14 * 1.3), (int)(2 * 1.4)) { }
    }

    public class BossArchmageCpp : Mage
    {
        public BossArchmageCpp(Random rand) : base("Архимаг C++", (int)(30 * 1.8), (int)(16 * 1.6), (int)(3 * 1.1))
        {
            FreezeChance = 25 + 10;
        }
    }

    public class BossPestovCMinusMinus : Skeleton
    {
        public int FreezeChance = 15;
        public BossPestovCMinusMinus(Random rand) : base("Пестов С--", (int)(35 * 1.3), (int)(14 * 1.8), (int)(2 * 0.6)) { }

        public override void Attack(Player player, Random rand, bool playerIsDefending)
        {
            Console.WriteLine($"{Name} проводит разрушительную атаку на {BaseAttack} урона, игнорируя защиту!");
            if (playerIsDefending) player.TakeDamageWithDefense(BaseAttack, rand, true);
            else player.CurrentHp -= BaseAttack;

            if (rand.Next(1, 101) <= FreezeChance && player.CurrentHp > 0)
            {
                player.IsFrozen = true;
                Console.WriteLine($"{Name} заморозил вас кодом!");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
