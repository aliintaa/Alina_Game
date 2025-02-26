using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alina_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
 
        public class Game
        {
            private readonly string[] dungeonMap = new string[10];
            private Player player;

            public Game()
            {
                InitializeDungeonMap();
                player = new Player();
            }

            private void InitializeDungeonMap()
            {
                Random random = new Random();
                string[] events = { "monster", "trap", "chest", "merchant", "empty" };

                for (int i = 0; i < dungeonMap.Length - 1; i++)
                {
                    dungeonMap[i] = events[random.Next(events.Length)];
                }
                dungeonMap[9] = "boss"; // Босс в последней комнате
            }

            public void Start()
            {
                Console.WriteLine("Добро пожаловать в подземелье!");

                for (int i = 0; i < dungeonMap.Length; i++)
                {
                    Console.WriteLine($"\nВы входите в комнату {i + 1}. Это {dungeonMap[i]}!");

                    switch (dungeonMap[i])
                    {
                        case "monster":
                            BattleWithMonster();
                            break;
                        case "trap":
                            TriggerTrap();
                            break;
                        case "chest":
                            OpenChest();
                            break;
                        case "merchant":
                            VisitMerchant();
                            break;
                        case "empty":
                            Console.WriteLine("Комната пустая. Ничего не происходит.");
                            break;
                        case "boss":
                            BattleWithBoss();
                            break;
                    }

                    if (player.Health <= 0)
                    {
                        Console.WriteLine("Вы погибли! Игра окончена.");
                        return;
                    }
                }

                Console.WriteLine("Поздравляем! Вы победили всех врагов!");
            }

            private void BattleWithMonster()
            {
                Monster monster = new Monster();
                Console.WriteLine($"Вы встретили монстра с {monster.Health} HP!");

                while (monster.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine("Выберите действие: (1) Атаковать мечом (2) Атаковать луком (3) Использовать зелье");
                    string choice = Console.ReadLine();

                    int damage = 0;

                    switch (choice)
                    {
                        case "1":
                            damage = player.AttackWithSword();
                            break;
                        case "2":
                            damage = player.AttackWithBow();
                            break;
                        case "3":
                            player.UsePotion();
                            continue;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            continue;
                    }

                    monster.Health -= damage;
                    Console.WriteLine($"Вы нанесли {damage} урона монстру. У него осталось {monster.Health} HP.");

                    if (monster.Health > 0)
                    {
                        int monsterDamage = monster.Attack();
                        player.Health -= monsterDamage;
                        Console.WriteLine($"Монстр атакует! Вы потеряли {monsterDamage} HP. У вас осталось {player.Health} HP.");
                    }
                }

                if (player.Health > 0)
                {
                    Console.WriteLine("Вы победили монстра!");
                }
            }

            private void TriggerTrap()
            {
                Random random = new Random();



                int trapDamage = random.Next(10, 21);
                player.Health -= trapDamage;
                Console.WriteLine($"Вы попали в ловушку и потеряли {trapDamage} HP. У вас осталось {player.Health} HP.");
            }

            private void OpenChest()
            {
                Console.WriteLine("Вы нашли сундук!");
                bool solvedPuzzle = SolveMathPuzzle();

                if (solvedPuzzle)
                {
                    Random random = new Random();
                    int rewardType = random.Next(3); // 0 - potion, 1 - gold, 2 - arrows

                    switch (rewardType)
                    {
                        case 0:
                            player.AddToInventory("Potion");
                            Console.WriteLine("Вы нашли зелье!");
                            break;
                        case 1:
                            player.Gold += 20;
                            Console.WriteLine("Вы нашли золото!");
                            break;
                        case 2:
                            player.Arrows += 3;
                            Console.WriteLine("Вы нашли стрелы!");
                            break;
                    }
                }
            }

            private bool SolveMathPuzzle()
            {
                Random random = new Random();
                int a = random.Next(1, 11);
                int b = random.Next(1, 11);
                int answer = a + b;

                while (true)
                {
                    Console.Write($"Решите загадку: {a} + {b} = ");
                    if (int.TryParse(Console.ReadLine(), out int userAnswer) && userAnswer == answer)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Неправильный ответ, попробуйте снова.");
                    }
                }
            }

            private void VisitMerchant()
            {
                Console.WriteLine("Вы встретили торговца.");
                if (player.Gold >= 30)
                {
                    Console.Write("Хотите купить зелье за 30 золота? (да/нет): ");
                    string choice = Console.ReadLine();

                    if (choice.ToLower() == "да")
                    {
                        player.Gold -= 30;
                        player.AddToInventory("Potion");
                        Console.WriteLine("Вы купили зелье!");
                    }
                    else
                    {
                        Console.WriteLine("Торговец уходит.");
                    }
                }
                else
                {
                    Console.WriteLine("У вас недостаточно золота для покупки.");
                }
            }

            private void BattleWithBoss()
            {
                Monster boss = new Monster(true); // Босс сильнее обычного монстра
                Console.WriteLine($"Вы встретили босса с {boss.Health} HP!");

                while (boss.Health > 0 && player.Health > 0)
                {
                    Console.WriteLine("Выберите действие: (1) Атаковать мечом (2) Атаковать луком (3) Использовать зелье");
                    string choice = Console.ReadLine();

                    int damage = 0;

                    switch (choice)
                    {
                        case "1":
                            damage = player.AttackWithSword();
                            break;
                        case "2":
                            damage = player.AttackWithBow();
                            break;
                        case "3":
                            player.UsePotion();
                            continue;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            continue;
                    }

                    boss.Health -= damage;
                    Console.WriteLine($"Вы нанесли {damage} урона боссу. У него осталось {boss.Health} HP.");

                    if (boss.Health > 0)
                    {
                        int bossDamage = boss.Attack();
                        player.Health -= bossDamage;
                        Console.WriteLine($"Босс атакует! Вы потеряли {bossDamage} HP. У вас осталось {player.Health} HP.");
                    }
                }



                if (player.Health > 0)
                {
                    Console.WriteLine("Поздравляем! Вы победили босса и выиграли игру!");
                }
            }
        }

        public class Player
        {
            public int Health { get; set; } = 100;
            public int Gold { get; set; } = 0;
            public int Potions { get; set; } = 3;
            public int Arrows { get; set; } = 5;
            private string[] inventory = new string[5];
            private int inventoryCount = 0;

            public int AttackWithSword()
            {
                Random random = new Random();
                int damage = random.Next(10, 21);
                return damage;
            }

            public int AttackWithBow()
            {
                if (Arrows > 0)
                {
                    Arrows--;
                    Random random = new Random();
                    int damage = random.Next(5, 16);
                    return damage;
                }
                else
                {
                    Console.WriteLine("У вас закончились стрелы!");
                    return 0;
                }
            }

            public void UsePotion()
            {
                if (Potions > 0)
                {
                    Health += 20;
                    Potions--;
                    Console.WriteLine($"Вы использовали зелье. Здоровье восстановлено до {Health} HP.");
                }
                else
                {
                    Console.WriteLine("У вас нет зелий!");
                }
            }

            public void AddToInventory(string item)
            {
                if (inventoryCount < inventory.Length)
                {
                    inventory[inventoryCount++] = item;
                }
                else
                {
                    Console.WriteLine("Ваш инвентарь полон! Не можете взять новый предмет.");
                }
            }
        }

        public class Monster
        {
            public int Health { get; set; }

            public Monster(bool isBoss = false)
            {
                Random random = new Random();
                Health = isBoss ? random.Next(50, 101) : random.Next(20, 51);
            }

            public int Attack()
            {
                Random random = new Random();
                return random.Next(5, 16);
            }
        }
    }






        