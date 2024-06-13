using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public abstract class Stats
    {
        public string StatUnitName { get; }

        public Stats(string statUnitName) {  StatUnitName = statUnitName; }

        public abstract void CalculateStat(int val);
    }
}
