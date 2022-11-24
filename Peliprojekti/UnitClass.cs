using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliprojekti
{
    public class UnitClass
    {
        public enum SpellBoost { None, Shield, Heal, Explosion, Charge }

        private string name;
        private string description;
        private ConsoleColor color;

        private int maxHealth;
        private int attackPower;
        private SpellBoost spellBoost; 

        public UnitClass(string name, string desc, int h, int a, SpellBoost b, ConsoleColor color)
        {
            this.name = name;
            this.description = desc;
            this.color = color;

            this.maxHealth = h;
            this.attackPower = a;
            this.spellBoost = b;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public ConsoleColor GetColor()
        {
            return color;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetAttackPower()
        {
            return attackPower;
        }

        public SpellBoost GetSpellBoost()
        {
            return spellBoost;
        }
    }
}
