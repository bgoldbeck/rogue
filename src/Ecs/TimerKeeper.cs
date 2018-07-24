//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace Ecs
{
    public class TimerKeeper
    {
        
        private static List<Action> timerCallbacks = new List<Action>();
        private static List<long> timers = new List<long>();

        public static void Update()
        {

            List<int> removals = new List<int>();

            for (int i = 0; i < timers.Count; ++i)
            {
                // Take a little off the duration.
                timers[i] -= Time.deltaMs;

                // Duration of timer ran out.
                if (timers[i] <= 0f)
                {
                    removals.Add(i);
                    timerCallbacks[i].Invoke();
                }
            }
            
            foreach (int index in removals)
            {
                timers.RemoveAt(index);
                timerCallbacks.RemoveAt(index);
            }
            return;
        }

        public static void AddTimer(float lengthOfTimer, Action actionAtTime)
        {
            long timerDuration = (1000 * (long)lengthOfTimer);
            
            timers.Add(timerDuration);
            timerCallbacks.Add(actionAtTime);
            return;
        }
    }
}
