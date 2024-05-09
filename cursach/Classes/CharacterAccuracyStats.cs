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
        protected uint WrongTypedUnits { get; set; }

        public CharacterAccuracyStats() : base(" % char accuracy") { WrongTypedUnits = 0; }

        protected CharacterAccuracyStats(string ValName) : base(ValName) { }

        public virtual float GetUnitAccuracy() { return UnitAccuracy; }

        public override void CalculateStat(uint CorrectTypedChars)
        {
            if (WrongTypedUnits == 0) { UnitAccuracy = 100.0f; return; }
            UnitAccuracy = WrongTypedUnits / CorrectTypedChars;
        }
    }
}
