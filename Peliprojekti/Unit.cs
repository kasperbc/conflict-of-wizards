using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliprojekti
{
    internal class Unit
    {
        public enum Status { Neutral, Dead, Shield, Charged };

        private Status status;
        private string name;
        private UnitClass uClass;

        private float attackMultiplier;
        private int hitPoints;
        private int mana;

        private bool isPlayer;

        public Unit(string n, UnitClass c, bool p)
        {
            name = n;
            uClass = c;
            isPlayer = p;

            hitPoints = c.GetMaxHealth();

            status = Status.Neutral;
            mana = 3;
            attackMultiplier = 1;

            if (n == "random")
            {
                name = GetRandomName();
            }
        }


        public string GetName()
        {
            return name;
        }

        public int GetAttackPower()
        {
            return uClass.GetAttackPower();
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        public Status GetStatus()
        {
            return status;
        }

        public void SetStatus(Status value)
        {
            status = value;
        }

        public int GetMana()
        {
            return mana;
        }

        public void AddMana(int value)
        {
            mana += value;

            mana = Math.Clamp(mana, 0, 3);
        }

        public void Damage(int dmg)
        {
            hitPoints -= dmg;

            if (hitPoints <= 0)
            {
                hitPoints = 0;
                status = Status.Dead;
            }
            else if (hitPoints >= uClass.GetMaxHealth())
            {
                hitPoints = uClass.GetMaxHealth();
            }
        }

        public void SetAttackMultiplier(float value)
        {
            attackMultiplier = value;
        }

        public float GetAttackMultiplier()
        {
            return attackMultiplier;
        }

        public int GetMaxHealth()
        {
            return uClass.GetMaxHealth();
        }

        public UnitClass.SpellBoost GetSpellBoost()
        {
            return uClass.GetSpellBoost();
        }

        string GetRandomName()
        {
            string[] nameList = File.ReadAllLines(Environment.CurrentDirectory + "/randomnames.txt");

            Random r = new Random();
            string randomName = "Jessey";
            while(true)
            {
                randomName = nameList[r.Next(nameList.Length)];

                if (randomName.Length == 6)
                {
                    break;
                }
            }

            return randomName;
        }

        public void CopyUnit(Unit u)
        {
            hitPoints = u.GetHitPoints();
            mana = u.GetMana();
            status = u.GetStatus();
            isPlayer = u.IsPlayer();
            uClass = u.GetUnitClass();
        }

        public Unit Clone()
        {
            Unit clone = new Unit(name, uClass, isPlayer);

            return clone;
        }

        public bool IsPlayer()
        {
            return isPlayer;
        }

        public UnitClass GetUnitClass()
        {
            return uClass;
        }
    }
}
