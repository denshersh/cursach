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

        public TypingSpeedStats() : base(" w/m")
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

        public override void CalculateStat(uint NumberOfCorrectEnteredWords)
        {
            stopwatch.Stop();
            TypingSpeed = (float)NumberOfCorrectEnteredWords / (stopwatch.ElapsedMilliseconds / 1000.0f) * SecondsInAMinute;
            stopwatch.Start();

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
