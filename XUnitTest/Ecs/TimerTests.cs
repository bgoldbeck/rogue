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

        internal void Callback()
        {
            calledBack = true;
            return;
        }

        [Fact]
        public void TimerCallsCallback()
        {
            Time.Update();
            TimerKeeper.AddTimer(0.1f, Callback);
            System.Threading.Thread.Sleep(500);
            Time.Update();
            TimerKeeper.Update();
            Assert.True(calledBack);
            return;
        }
    }
}
