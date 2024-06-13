using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class CharacterAccuracyStats : Stats
    {
        protected float UnitAccuracy;
        protected int WrongTypedUnits;

        public CharacterAccuracyStats() : base(" %") { WrongTypedUnits = 0; }

        protected CharacterAccuracyStats(string ValName) : base(ValName) { }

        public virtual void SetWrongTypedUnits(int wrongTypedUnits)
        {
            WrongTypedUnits = wrongTypedUnits;
        }

        public virtual float GetUnitAccuracy() { return UnitAccuracy; }

        public override void CalculateStat(int CorrectTypedChars)
        {
            if (WrongTypedUnits == 0) { UnitAccuracy = 100.0f; return; }
            UnitAccuracy = (float)(Math.Truncate((100.0f - ((float)WrongTypedUnits / CorrectTypedChars * 100.0f)) * 100.0f) / 100.0f);
        }
    }
}
