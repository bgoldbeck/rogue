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
        public static bool calledBack = false;

        internal void Callback1()
        {
            calledBack = true;
            return;
        }

        internal void Callback2()
        {
            calledBack = true;
            return;
        }

        [Fact]
        public void TimerCallsCallback()
        {
            calledBack = false;
            Time.Update();
            TimerKeeper.AddTimer(0.1f, Callback1);
            System.Threading.Thread.Sleep(500);
            Time.Update();
            TimerKeeper.Update();
            Assert.True(calledBack);
            return;
        }

        [Fact]
        public void TimerShouldntCallCallback()
        {
            calledBack = false;
            Time.Update();
            TimerKeeper.AddTimer(1.0f, Callback2);
            System.Threading.Thread.Sleep(10);
            Time.Update();
            TimerKeeper.Update();
            Assert.False(calledBack);
            return;
        }
    }
}
