using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class TypingSpeedStats : Stats
    {
        protected float TypingSpeed { get; set; }    // words per minute
        private Stopwatch stopwatch;
        const float SecondsInAMinute = 60.0f;

        public TypingSpeedStats() : base("w/m")
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public override void CalculateStat(uint NumberOfCorrectlyTypedWords)
        {
            stopwatch.Stop();
            TypingSpeed = ((float)NumberOfCorrectlyTypedWords / stopwatch.ElapsedMilliseconds) * 1000.0f * SecondsInAMinute;
            stopwatch.Start();
        }
    }
}
