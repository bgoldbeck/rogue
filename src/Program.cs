//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using Game;

class Program
{    
    static void Main(string[] args)
    {
        Application game = new Application();
        game.Initialize();
        int error = game.Loop();
          
        return;
    }
}

