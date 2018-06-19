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

        public double UnitLifePoints;
        public double UnitManaPoints;

        double UnitMinimumDamage;
        double UnitMaximumDamage;

        int UnitAccuracy;

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

        public void SetDamage (int InputMinimumDamage, int InputMaximumDamage)
        {
            UnitMinimumDamage = InputMinimumDamage;
            UnitMaximumDamage = InputMaximumDamage;
        }

        public void UpdateLifePoints(int InputLife)
        {
            UnitLifePoints = InputLife;
        }
        
        public void UpdateManaPoints(int InputMana)
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
            Unit Player = new Unit();
            Player.SetStats(5, 25, 45, 100);
            Player.SetDamage(3, 7);
            Player.UnitEnergyShield = Player.SetPoints("EnergyShield", 5, 0, 0, 0, 0);
            Player.UnitLifePoints = Player.SetPoints("Life", 38, 0, 0, 0, 0);
            Player.UnitManaPoints = Player.SetPoints("Mana", 10, 0, 0, 0, 0);

            Unit Monster = new Unit();
            Player.SetStats(5, 25, 45, 100);
            Player.SetDamage(3, 7);
            Player.UnitEnergyShield = Player.SetPoints("EnergyShield", 5, 0, 0, 0, 0);
            Player.UnitLifePoints = Player.SetPoints("Life", 38, 0, 0, 0, 0);
            Player.UnitManaPoints = Player.SetPoints("Mana", 10, 0, 0, 0, 0);
        }
    }
}
