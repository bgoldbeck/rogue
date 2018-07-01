using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (currentKey == null)
            {
                currentKey = new ConsoleKeyWrapper
                {
                    key = (Console.KeyAvailable) == true ? Console.ReadKey(true) : new ConsoleKeyInfo()
                };
            }
            return currentKey.key;
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
