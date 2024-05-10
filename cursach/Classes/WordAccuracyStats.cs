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

        public override void SetWrongTypedUnits(uint wrongTypedUnits)
        {
            WrongTypedUnits = wrongTypedUnits;
        }

        public override float GetUnitAccuracy() { return UnitAccuracy; }

        public override void CalculateStat(uint CorrectTypedChars)
        {
            if (WrongTypedUnits == 0) { UnitAccuracy = 100.0f; return; }
            UnitAccuracy = (float)(Math.Truncate((100.0f - ((float)WrongTypedUnits / CorrectTypedChars * 100.0f)) * 100.0f) / 100.0f);
        }

    }
}
