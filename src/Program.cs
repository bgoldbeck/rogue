//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game;
using Game.Components;

using IO;

class Program
{
    static void Main(string[] args)
    {

        Application game = new Application();
        game.Initialize();
        game.Update();
        game.Render();
        game.Render(); //Need to for some reason?
        int error = game.Loop();
        
        
        return;
    }
}

