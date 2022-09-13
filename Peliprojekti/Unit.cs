using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliprojekti
{
    internal class Unit
    {
        public enum Status { Neutral, Dead };

        private Status status;
        private string name;
        private int attackPower;
        private int hitPoints;

        public Unit(string n, int a, int h)
        {
            name = n;
            attackPower = a;
            hitPoints = h;

            status = Status.Neutral;
        }

        public string GetName()
        {
            return name;
        }

        public int GetAttackPower()
        {
            return attackPower;
        }

        public int GetHitPoints()
        {
            return hitPoints;
        }

        public Status GetStatus()
        {
            return status;
        }

        public void Damage(int dmg)
        {
            hitPoints -= dmg;

            if (hitPoints < 0)
            {
                hitPoints = 0;
                status = Status.Dead;
            }
        }
    }
}
