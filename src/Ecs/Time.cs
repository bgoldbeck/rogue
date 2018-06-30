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
        private static float startFrameTime = 0f;
        private static Stopwatch stopwatch = new Stopwatch();

        public static long deltaTime = 0;

        public static void Initialize()
        {
            deltaTime = 0;
            stopwatch.Start();
            return;
        }

        public static void Update()
        {
            deltaTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            return;
        }


    }
}
