using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliprojekti
{
    internal class Spell
    {
        private string name;
        private int cost;

        public Spell(string name, int cost)
        {
            this.name = name;
            this.cost = cost;
        }

        public string GetName()
        {
            return name;
        }

        public int GetCost()
        {
            return cost;
        }
    }
}
