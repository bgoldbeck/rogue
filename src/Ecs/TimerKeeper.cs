//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Ecs
{
    class TimerKeeper
    {
        private static long currentTime = 0;
        private static List<Action> timerCallMethods = new List<Action>();
        private static List<long> timerExpiration = new List<long>();

        public static void Update()
        {
            long currentTime = Time.GetCurrentTime();
            for (int i = 0; i < timerExpiration.Count; ++i)
            {
                if(timerExpiration[i] < currentTime)
                {
                    timerCallMethods[i]();
                    timerCallMethods.RemoveAt(i);
                    timerExpiration.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        public static void AddTimer(float lengthOfTimer, Action actionAtTime)
        {
            lengthOfTimer *= 1000;
            int i = 0;
            for (; i < timerExpiration.Count; ++i)
            {
                if(timerExpiration[i] > lengthOfTimer)
                {
                    break;
                }
            }
            if(i >= timerExpiration.Count)
            {
                timerExpiration.Add((long)lengthOfTimer);
                timerCallMethods.Add(actionAtTime);
            }
            else
            {
                timerExpiration.Insert(i,(long)lengthOfTimer);
                timerCallMethods.Insert(i, actionAtTime);
            }
        }
    }
}
