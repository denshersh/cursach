using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class WordAccuracyStats : CharacterAccuracyStats
    {

        public WordAccuracyStats() : base(" % words accuracy") { }

        public override float GetUnitAccuracy() { return UnitAccuracy; }

        public override void CalculateStat(uint CorrectTypedChars)
        {
            if (WrongTypedUnits == 0) { UnitAccuracy = 100.0f; return; }
            UnitAccuracy = WrongTypedUnits / CorrectTypedChars;
        }

    }
}
