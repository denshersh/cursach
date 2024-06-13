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
        protected float TypingSpeed;    // words per minute
        private Stopwatch stopwatch;
        const float SecondsInAMinute = 60.0f;

        public TypingSpeedStats() : base(" ch/m")
        {
            stopwatch = new Stopwatch();
        }

        public void StartMonitoringSpeed()
        {
            stopwatch.Start();
        }

        public void StopMonitoringSpeed()
        {
            stopwatch.Stop();
        }

        public override void CalculateStat(int NumberOfCorrectEnteredWords)
        {
            TypingSpeed = (float)Math.Truncate(((float)NumberOfCorrectEnteredWords / (stopwatch.ElapsedTicks / 10000000.0f) * SecondsInAMinute) * 10.0f) / 10.0f;
            stopwatch.Reset();
        }

        public float GetTypingSpeed()
        {
            return TypingSpeed;
        }

        public float GetStopWatchElapsedTime()
        {
            return stopwatch.ElapsedMilliseconds / 1000.0f;
        }
    }
}
