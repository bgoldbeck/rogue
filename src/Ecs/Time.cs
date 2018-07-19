//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Ecs
{
    class Time
    {
        private static Stopwatch stopwatch = new Stopwatch();

        public static long deltaTicks = 0;
        public static double deltaMs = 0.0;

        public static void Initialize()
        {
            deltaTicks = 0;
            stopwatch.Start();
            return;
        }

        public static void Update()
        {
            deltaTicks = stopwatch.ElapsedTicks;
            deltaMs = deltaTicks / TimeSpan.TicksPerMillisecond; 
            stopwatch.Reset();
            stopwatch.Start();
            return;
        }


    }
}
