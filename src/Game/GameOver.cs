//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Text;

using IO;

namespace Game
{
    class GameOver
    {
        private List<String> logo = new List<String>();
        private int width;
        private int height;
        private ConsoleKey press = Input.ReadKey().Key;

        public GameOver(int width, int height)
        {
            this.width = width;
            this.height = height;

            // This is the logo to display centered in the screen. Width of first row
            // determines width of the logo.
            logo.Add(@" ___________________________________________ ");
            logo.Add(@"/                __       __                \");
            logo.Add(@"|               │  │     │  │               |");
            logo.Add(@"│               │  │     │  │               |");
            logo.Add(@"│               │ __     │ __               |");
            logo.Add(@"│               │  │     │  │               |");
            logo.Add(@"│               │  │     │  │               |");
            logo.Add(@"│               │__│.    │__│.              |");
            logo.Add(@"|                                           |");
            logo.Add(@"|                                           |");
            logo.Add(@"`──────────────────────────────────────────' ");
            logo.Add(@"Press [enter] to restart or [esc] to quit. ");
        }

        public void Run()
        {
            do
            {
                if (WindowResized())
                    Reset(Console.WindowWidth, Console.WindowHeight);
                ConsoleUI.Render();
                ConsoleUI.Render();
                press = Input.ReadKey().Key;
            } while (press != ConsoleKey.Enter);
        }

        private bool WindowResized()
        {
            if (this.width != Console.WindowWidth || this.height != Console.WindowHeight)
                return true;
            return false;
        }

        private void Reset(int width, int height)
        {
            this.width = width;
            this.height = height;
            ConsoleUI.Initialize(width, height);
        }
    }
}
