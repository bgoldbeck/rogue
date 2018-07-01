using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class Input
    {
        public static ConsoleKeyInfo ReadKey()
        {
            return (Console.KeyAvailable == true) ? Console.ReadKey(true) : new ConsoleKeyInfo();
        }
    }
}
