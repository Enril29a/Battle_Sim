using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleSim
{
     class Unit
     {
        //Character Stats
        int UnitLevel;

        public double UnitStrength;
        public double UnitDexterity;
        public double UnitIntelligence;

        public double UnitArmour;
        public double UnitEvasion;
        public double UnitEnergyShield;

        public double UnitAccuracy;

        public double UnitLifePoints;
        public double UnitManaPoints;

        public double UnitMinimumDamage;
        public double UnitMaximumDamage;

        //Variable Set
        public void SetStats(int InputLevel, double InputStrength, double InputDexterity, double InputIntelligence)
        {
            UnitLevel = InputLevel;
            UnitStrength = InputStrength;
            UnitDexterity = InputDexterity;
            UnitIntelligence = InputIntelligence;
        }

        public double SetPoints (string InputType, int InputFlat, int InputIncreased, int InputReduced, int InputMore, int InputLess)
        {
            if (InputType == "Life") { return (38 + (12 * UnitLevel) + (UnitStrength / 2) + InputFlat) * (1 + ((InputIncreased / 100) - (InputReduced / 100))) * (1 + ((InputMore / 100) - (InputLess / 100))); }
            else if (InputType == "Mana") { return (34 + (6 * UnitLevel) + (UnitIntelligence / 2) + InputFlat) * (1 + ((InputIncreased / 100) - (InputReduced / 100))) * (1 + ((InputMore / 100) - (InputLess / 100))); }
            else if (InputType == "EnergyShield") { return (InputFlat) * (1 + (((2 * (UnitIntelligence / 10)) / 100) - (InputReduced / 100))) * (1 + ((InputMore / 100) - (InputLess / 100))); }
            else return -1;
        }

        public void SetDamage (double InputMinimumDamage, double InputMaximumDamage)
        {
            UnitMinimumDamage = InputMinimumDamage;
            UnitMaximumDamage = InputMaximumDamage;
        }

        public void UpdateLifePoints(double InputLife)
        {
            UnitLifePoints = InputLife;
        }
        
        public void UpdateManaPoints(double InputMana)
        {
            UnitManaPoints = InputMana;
        }
        
     }

    class service
    {
        public double ServiceTakePoints(double Points, double InputPointsTaken)
        {
            return Points -= InputPointsTaken;
        }

        public double ServiceGainPoints(double Points, double InputPointsGain)
        {
            return Points += InputPointsGain;
        }

        public double ServiceCalculateHitChance(double InputAttackerAccuracy, double InputTargetEvasion)
        {
            //Calculate hit chance
            return 1 - (InputAttackerAccuracy / (InputAttackerAccuracy + Math.Pow((InputTargetEvasion * 0.25), 0.8)));
        }

    }

    class Battle
    {
        static void Main(string[] args)
        {
            //define player/monster, fill stats
            Unit Player = new Unit();
            Player.SetStats(5, 32, 14, 14);
            Player.SetDamage(3, 7);
            Player.UnitEnergyShield = Player.SetPoints("EnergyShield", 5, 0, 0, 0, 0);
            Player.UnitLifePoints = Player.SetPoints("Life", 38, 0, 0, 0, 0);
            Player.UnitManaPoints = Player.SetPoints("Mana", 10, 0, 0, 0, 0);
            Player.UnitAccuracy = 5000;
            Player.UnitEvasion = 800;

            Unit Monster = new Unit();
            Monster.SetStats(5, 10, 10, 10);
            Monster.SetDamage(4, 4);
            Monster.UnitEnergyShield = Monster.SetPoints("EnergyShield", 5, 0, 0, 0, 0);
            Monster.UnitLifePoints = Monster.SetPoints("Life", 38, 0, 0, 0, 0);
            Monster.UnitManaPoints = Monster.SetPoints("Mana", 10, 0, 0, 0, 0);
            Monster.UnitAccuracy = 90000;
            Monster.UnitEvasion = 500;

            Console.WriteLine("Player");
            Console.WriteLine("Strength - " + Player.UnitStrength + " | Dexterity - " + Player.UnitDexterity + " | Intelligence - " + Player.UnitIntelligence);
            Console.WriteLine("Life - " + Player.UnitLifePoints + " | Mana - " + Player.UnitManaPoints + " | Energy Shield - " + Player.UnitEnergyShield);
            Console.WriteLine();
            Console.WriteLine("Monster");
            Console.WriteLine("Strength - " + Monster.UnitStrength + " | Dexterity - " + Monster.UnitDexterity + " | Intelligence - " + Monster.UnitIntelligence);
            Console.WriteLine("Life - " + Monster.UnitLifePoints + " | Mana - " + Monster.UnitManaPoints + " |  Energy Shield - " + Monster.UnitEnergyShield);
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Battle Start");

            //declare variables and service object for battle
            Random Random = new Random();
            double PlayerEntropy = Random.Next(0, 99);
            double MonsterEntropy = Random.Next(0, 99);
            service ServiceObject = new service();
            double PlayerHitChance = (100 * ServiceObject.ServiceCalculateHitChance(Player.UnitAccuracy, Monster.UnitEvasion));
            double MonsterHitChance = (100 * ServiceObject.ServiceCalculateHitChance(Monster.UnitAccuracy, Player.UnitEvasion));

            ///
            //Need to ask about how to convert while building, or loop through this, hassle for more than 2 unit objects. Need damage to be int so I can use the random.next function. Parameters are int, but cannot convert in parameter list.
            ///
            int PlayerMinimumDamage = Convert.ToInt32(Player.UnitMinimumDamage);
            int PlayerMaximumDamage = Convert.ToInt32(Player.UnitMaximumDamage);
            int MonsterMinimumDamage = Convert.ToInt32(Monster.UnitMinimumDamage);
            int MonsterMaximumDamage = Convert.ToInt32(Monster.UnitMaximumDamage);
            double Damage;


            Console.WriteLine("Player entropy rolled " + PlayerEntropy);
            Console.WriteLine("Monster entropy rolled " + MonsterEntropy);

            //Start Battle
            for (int Counter = 1; Counter <= 10000; Counter++)
            {
                //Check initiative
                int Initiative = Random.Next(1, 10);
                //Less than 5, player attacks first
                if (Initiative <= 5)
                {
                    //Add hitchance to Entropy
                    PlayerEntropy += PlayerHitChance;
                    //Check hit
                    if(PlayerEntropy > 100)
                    {
                        PlayerEntropy -= 100;
                        Damage = Random.Next(PlayerMinimumDamage,PlayerMaximumDamage);
                        Monster.UpdateLifePoints(ServiceObject.ServiceTakePoints(Monster.UnitLifePoints, Damage));
                        Console.WriteLine("The player attacked the monster for " + Damage + " Damage in round " + Counter);
                    }

                    MonsterEntropy += MonsterHitChance;
                    if (MonsterEntropy > 100)
                    {
                        MonsterEntropy -= 100;
                        Damage = Random.Next(MonsterMinimumDamage, MonsterMaximumDamage);
                        Player.UpdateLifePoints(ServiceObject.ServiceTakePoints(Player.UnitLifePoints, Damage));
                        Console.WriteLine("The monster attacked the player for " + Damage + " Damage in round " + Counter);
                    }
                    
                }
                //Monster attacks first
                else
                {
                    MonsterEntropy += MonsterHitChance;
                    if (MonsterEntropy > 100)
                    {
                        MonsterEntropy -= 100;
                        Damage = Random.Next(MonsterMinimumDamage, MonsterMaximumDamage);
                        Player.UpdateLifePoints(ServiceObject.ServiceTakePoints(Player.UnitLifePoints, Damage));
                        Console.WriteLine("The monster attacked the player for " + Damage + " Damage in round " + Counter);
                    }

                    PlayerEntropy += PlayerHitChance;
                    if (PlayerEntropy > 100)
                    {
                        PlayerEntropy -= 100;
                        Damage = Random.Next(PlayerMinimumDamage, PlayerMaximumDamage);
                        Monster.UpdateLifePoints(ServiceObject.ServiceTakePoints(Monster.UnitLifePoints, Damage));
                        Console.WriteLine("The player attacked the monster for " + Damage + " Damage in round " + Counter);
                    }
                                        
                }
                
                //Check if dead
                if(Player.UnitLifePoints < 0)
                {
                    Console.WriteLine("The player is dead, the monster has won");
                    break;
                }
                else if (Monster.UnitLifePoints < 0)
                {
                    Console.WriteLine("The monster is dead, the player has won");
                    break;
                }


            }
            Console.ReadKey();

        }
    }
}
