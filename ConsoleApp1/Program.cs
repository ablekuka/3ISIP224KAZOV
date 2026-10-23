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
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Random rand = new Random();

            Console.WriteLine("=== МИНИ-РОГАЛИК ===");
            Console.Write("Введите имя героя: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name)) name = "Герой";

            Player player = new Player(name);
            int turn = 0;

            while (player.CurrentHp > 0)
            {
                turn++;
                Console.WriteLine($"\n--- ХОД {turn} ---");
                Console.WriteLine($"Вы: {player.Name} | HP: {player.CurrentHp}/{player.MaxHp} | Атака: {player.GetTotalAttack()} | Защита: {player.GetTotalDefense()}");

                if (turn % 10 == 0)
                {
                    Console.WriteLine("\nПОЯВИЛСЯ БОСС!");

                    Enemy boss;
                    int bossType = rand.Next(1, 5);
                    if (bossType == 1) boss = new BossVvg(rand);
                    else if (bossType == 2) boss = new BossKowalski(rand);
                    else if (bossType == 3) boss = new BossArchmageCpp(rand);
                    else boss = new BossPestovCMinusMinus(rand);

                    Battle(player, boss, rand);
                }
                else
                {
                    if (rand.Next(1, 101) <= 50)
                    {
                        OpenChest(player, rand);
                    }
                    else
                    {
                        Enemy enemy;
                        int enemyType = rand.Next(1, 4);
                        if (enemyType == 1) enemy = new Goblin(rand);
                        else if (enemyType == 2) enemy = new Skeleton(rand);
                        else enemy = new Mage(rand);

                        Battle(player, enemy, rand);
                    }
                }

                if (player.CurrentHp > 0)
                {
                    Console.WriteLine("\nНажмите любую клавишу для перехода к следующему ходу...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine($"\nИгра окончена! Вы погибли на {turn} ходу.");
            Console.ReadKey();



            void Battle(Player p, Enemy e, Random r)
            {
                Console.WriteLine($"" +
                    $"" +
                    $"Вы встретили: {e.Name} (HP: {e.MaxHp}, Атака: {e.BaseAttack})");

                while (p.CurrentHp > 0 && e.CurrentHp > 0)
                {
                    bool playerIsDefending = false;

                    if (p.IsFrozen)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"{p.Name} заморожен и пропускает свой ход!");
                        Console.ResetColor();
                        p.IsFrozen = false;
                    }
                    else
                    {
                        Console.WriteLine("\nВыберите действие:\n1. Атака\n2. Защита");
                        string input = Console.ReadLine();

                        if (input == "2")
                        {
                            playerIsDefending = true;
                            Console.WriteLine("Вы встали в защитную стойку.");
                        }
                        else
                        {
                            int dmg = p.GetTotalAttack() - e.BaseDefense;
                            if (dmg < 1) dmg = 1;
                            e.CurrentHp -= dmg;
                            if (e.CurrentHp < 0) e.CurrentHp = 0;
                            Console.WriteLine($"⚔Вы ударили {e.Name} на {dmg} урона. У него осталось {e.CurrentHp} HP.");
                        }
                    }

                    if (e.CurrentHp > 0)
                    {
                        e.Attack(p, r, playerIsDefending);
                    }
                }

                if (p.CurrentHp > 0)
                {
                    Console.WriteLine($"Победа над {e.Name}!");
                }
            }

            void OpenChest(Player p, Random r)
            {
                Console.WriteLine("\nВы нашли сундук!");

                int chestType = r.Next(1, 4);

                if (chestType == 1)
                {
                    p.CurrentHp = p.MaxHp;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Вы нашли Зелье Лечения! HP полностью восстановлено.");
                    Console.ResetColor();
                }
                else if (chestType == 2)
                {
                    Equipment newWeapon = new Equipment("Ржавый меч", TypeItem.Weapon, r.Next(5, 20));
                    Console.WriteLine($" Найдено: {newWeapon.Name} (Атака: +{newWeapon.Bonus})");
                    Console.WriteLine($"У вас сейчас: {p.Weapon.Name} (Атака: +{p.Weapon.Bonus})");
                    Console.Write("Взять его? (1 - Да, другое - Выбросить): ");

                    if (Console.ReadLine() == "1")
                    {
                        p.Weapon = newWeapon;
                        Console.WriteLine("Вы экипировали новое оружие!");
                    }
                }
                else
                {
                    Equipment newArmor = new Equipment("Кожаный жилет", TypeItem.Armor, r.Next(4, 15));
                    Console.WriteLine($"Найдена броня: {newArmor.Name} (Защита: +{newArmor.Bonus})");
                    Console.WriteLine($"У вас сейчас: {p.Armor.Name} (Защита: +{p.Armor.Bonus})");
                    Console.Write("Взять ее? (1 - Да, другое - Выбросить): ");

                    if (Console.ReadLine() == "1")
                    {
                        p.Armor = newArmor;
                        Console.WriteLine("Вы экипировали новую броню!");
                    }
                }
            }
        }
    }
}
