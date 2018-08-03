//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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
