#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace Ecs
{
    // A class for managing the time as the application runs.
    public class Time
    {
        public static long deltaMs = 0;

        private static long current = 0;

        private static long last = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        /// <summary>
        /// Increment the delta time since the last time update was called.
        /// </summary>
        public static void Update()
        {
            current = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            deltaMs = current - last;

            // Reset
            last = current;

            return;
        }

        /// <summary>
        /// Convert a value in milliseconds to seconds.
        /// </summary>
        /// <param name="ms">The milliseconds value to convert.</param>
        /// <returns>
        /// The amount of seconds that were converted from milliseconds.
        /// </returns>
        public static double MillisecondsToSeconds(long ms) => (double)ms / 1000.0;

        /// <summary>
        /// The current time in milliseconds.
        /// </summary>
        /// <returns>
        /// The current time in milliseconds.
        /// </returns>
        public static long GetCurrentTime()
        {
            return current;
        }
    }
}