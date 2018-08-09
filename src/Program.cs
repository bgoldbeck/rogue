#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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

