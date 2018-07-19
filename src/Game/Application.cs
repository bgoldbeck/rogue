//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

using Game.Components;
using IO;

namespace Game
{
    public class Application
    {
        private bool isRunning = true;
        private ConsoleKey press = Input.ReadKey().Key;
        private int width;
        private int height;

        public void Initialize()
        {
            SetupScreen();

            Time.Initialize();
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            ConsoleUI.Initialize(width, height);

            GameObject gameManager = GameObject.Instantiate("GameManager");
            gameManager.AddComponent(new GameManager(width, height));

            Update();
            Render();
            return;
        }

        private void SetupScreen()
        {
            Console.WriteLine(@"
    __________________________________________
   / ___   __   __       ___   ___        __  \
   │ │  │ │  │ │  │ │  │ │      │  │ │ │ │  │ │
   │ │ /  │  │ │  │ │  │ │      │  │ │ │ │  │ │
   │ │/   │  │ │ __ │  │ │__    │  │ │ │ │  │ │
   │ │\   │  │ │  │ │  │ │      │   V V  │  │ │
   │ │ \  │  │ │  │ │  │ │      │   │ │  │  │ │
   │ │  \ │__│ │__│ │__│ │__    │   │ │  │__│ │
   │                                          │
   `─────────── A PERMADEATH STORY ──────────'
                                          v 0.1");
            Console.WriteLine("\n   Please adjust your window to the desired play size,\n   then press [Enter] to begin the game.\n");
            Console.ReadLine();
            Console.Clear();
            return;
        }

        public int Loop()
        {

            while (isRunning)
            {

                /*dt += Time.deltaTicks;
                Update();
                if (dt >= 1600000)
                {
                    dt = 0;
                    Render();
                }*/

                CheckForResize();
                
                press = Input.ReadKey().Key;
                
                if (press == ConsoleKey.Escape)
                { 
                    isRunning = false;
                }

                if (Input.AnyKey())
                {
                    Update();
                    Render();
                    GameObject.ForceFlush();
                }

                Time.Update();
                Input.Reset();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckForResize()
        {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;

            if (newWidth != width || newHeight != height)
            {
                ConsoleUI.Resize(newWidth, newHeight);
                width = newWidth;
                height = newHeight;

                GameObject.OnResize();
                Render();
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            GameObject.EarlyUpdate();
            GameObject.Update();
            GameObject.LateUpdate();
            return;
        }

        public void Render()
        {
            GameObject.Render();

            ConsoleUI.Render();
            ConsoleUI.ClearBuffer();

            return;
        }
    }
}

