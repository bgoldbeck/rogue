using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;
using Game.Components;

using ConsoleUI;

class Program
{
    static void Main(string[] args)
    {

        Application game = new Application();
        game.Initialize();
        game.Update();
        int error = game.Loop();
        Console.ReadKey();
        return;
    }
}

