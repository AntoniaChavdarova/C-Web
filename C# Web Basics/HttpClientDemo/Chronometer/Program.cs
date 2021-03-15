using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Chronometer
{
    class Program
    {
        static void Main(string[] args)
        {
            Chronometer stopWatch1 = new Chronometer();

            stopWatch1.Start();
            stopWatch1.Reset();
            Console.WriteLine(stopWatch1.GetTime);
            stopWatch1.Lap();
            stopWatch1.Lap();
            Console.WriteLine(String.Join(Environment.NewLine , stopWatch1.Laps));
            stopWatch1.Lap();
            Thread.Sleep(1000);
            Console.WriteLine(String.Join(Environment.NewLine, stopWatch1.Laps));

             
        }

        
    }
}
