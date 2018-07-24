//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

/*We decided not to use this keep track of the times because it intefered with the update()
 * cycle causing each timer needing a work around*/
namespace Ecs
{
    public class TimerKeeper
    {
        
        private static Dictionary<Action, long> timers = new Dictionary<Action, long>();

        public static void Update()
        {

            List<Action> removals = new List<Action>();

            foreach (Action key in timers.Keys.ToList())
            {
                // Take a little off the duration.
                timers[key] -= Time.deltaMs;

                // Duration of timer ran out.
                if (timers[key] <= 0f)
                {
                    removals.Add(key);
                }
            }
            
            foreach (Action action in removals)
            {
                timers.Remove(action);
                action.Invoke();
            }
            return;
        }

        public static void AddTimer(float lengthOfTimer, Action actionAtTime)
        {
            if (timers.ContainsKey(actionAtTime)) { return;  }

            long timerDuration = (1000 * (long)lengthOfTimer);

            timers.Add(actionAtTime, timerDuration);

            //timers.Add(timerDuration);
            //timerCallbacks.Add(actionAtTime);
            return;
        }
    }
}
