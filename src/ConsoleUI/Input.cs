using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class Input
    {
        public static ConsoleKeyInfo ReadKey()
        {
            return (Console.KeyAvailable == true) ? Console.ReadKey() : new ConsoleKeyInfo();
        }
    }
}
