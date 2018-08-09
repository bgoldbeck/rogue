#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;

namespace IO
{
    public class Input
    {
        private class ConsoleKeyWrapper
        {
            public ConsoleKeyInfo key;
        }

        private static ConsoleKeyWrapper currentKey = null;

        public static void CheckForKeyPress()
        {
            if (Console.KeyAvailable)
            {
                currentKey = new ConsoleKeyWrapper
                {
                    key = Console.ReadKey(true)
                };
            }
        }

        public static ConsoleKeyInfo ReadKey()
        {
            return currentKey != null ? currentKey.key : new ConsoleKeyInfo();
        }

        public static bool AnyKey()
        {
            return currentKey != null;
        }

        public static void Reset()
        {
            currentKey = null;
            return;
        }
    }
}
