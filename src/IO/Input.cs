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

        public static ConsoleKeyInfo ReadKey()
        {
            if (currentKey == null && Console.KeyAvailable == true)
            {
                currentKey = new ConsoleKeyWrapper
                {
                    key = Console.ReadKey(true)
                };
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
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
