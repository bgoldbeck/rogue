//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Ecs
{
    class TimerKeeper
    {
        private static long currentTime;
        private static List<Action> timerCallMethods = new List<Action>();
        private static List<long> timerExpiration = new List<long>();

        public static void Update()
        {
            currentTime += Time.deltaMs;
            for (int i = 0; i < timerExpiration.Count; ++i)
            {
                if(timerExpiration[i] < currentTime)
                {
                    timerCallMethods[i]();
                    timerCallMethods.RemoveAt(i);
                    timerExpiration.RemoveAt(i);
                    --i;
                }
                else
                {
                    break;
                }
            }
        }

        public static void AddTimer(float lengthOfTimer, Action actionAtTime)
        {
            long timeOfTimer = (1000 * (long)lengthOfTimer) + currentTime;
            int i = 0;
            for (; i < timerExpiration.Count; ++i)
            {
                if(timerExpiration[i] > timeOfTimer)
                {
                    break;
                }
            }
            if(i >= timerExpiration.Count)
            {
                timerExpiration.Add(timeOfTimer);
                timerCallMethods.Add(actionAtTime);
            }
            else
            {
                timerExpiration.Insert(i, timeOfTimer);
                timerCallMethods.Insert(i, actionAtTime);
            }
        }
    }
}
