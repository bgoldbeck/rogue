//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Threading;
using Xunit;
using Ecs;
using System.Collections.Generic;

namespace XUnitTests
{
    public class TimerTests
    {
        public static bool calledBack1 = false;
        public static bool calledBack2 = false;

        internal void Callback1()
        {
            calledBack1 = true;
            return;
        }

        internal void Callback2()
        {
            calledBack2 = true;
            return;
        }

        [Fact]
        public void TimerCallsCallback()
        {
            calledBack1 = false;
            Time.Update();
            TimerKeeper.AddTimer(0.1f, Callback1);
            System.Threading.Thread.Sleep(500);
            Time.Update();
            TimerKeeper.Update();
            Assert.True(calledBack1);
            return;
        }

        [Fact]
        public void TimerShouldntCallCallback()
        {
            calledBack2 = false;
            Time.Update();
            TimerKeeper.AddTimer(1.0f, Callback2);
            System.Threading.Thread.Sleep(10);
            Time.Update();
            TimerKeeper.Update();
            Assert.False(calledBack2);
            return;
        }
    }
}
