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
        public static long deltaMs = 0;

        private static long current = 0;

        private static long last = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public static void Update()
        {
            current = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            deltaMs = current - last;

            // Reset
            last = current;

            return;
        }

        public static double MillisecondsToSeconds(long ms) => (double)ms / 1000.0;

        public static long getCurrentTime()
        {
            return current;
        }
    }
}