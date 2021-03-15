using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chronometer
{
    public class Chronometer : Stopwatch , IChronometer
    {
        
        private Stopwatch stopWatch;

        public Chronometer()
        {
            this.stopWatch = new Stopwatch();
            this.Laps = new List<string>();
        }
        public string GetTime => this.stopWatch.Elapsed.ToString();

        public List<string> Laps { get; }

        public string Lap()
        {
            
            var lap = this.GetTime;
            this.Laps.Add(lap);
            this.stopWatch.Stop();
            this.stopWatch.Start();

            return lap;
        }

    

        public void Reset()
        {
            this.stopWatch.Reset();
        }

        public void Start()
        {
            this.stopWatch.Start();
            
         

        }

        public void Stop()
        {
            this.stopWatch.Stop();
        }
    }
}
