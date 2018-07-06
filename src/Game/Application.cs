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

        public void Initialize()
        {
            SetupScreen();

            Time.Initialize();
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
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
            long dt = 0;

            while (isRunning)
            {

                /*dt += Time.deltaTicks;
                Update();
                if (dt >= 1600000)
                {
                    dt = 0;
                    Render();
                }*/
                

                
                press = Input.ReadKey().Key;
                
                if (press == ConsoleKey.Escape)
                { 
                    isRunning = false;
                }
                

                if (Input.AnyKey())
                {
                    Update();
                    Render();
                }

                Time.Update();
                Input.Reset();
            }

            return 0;
        }

        public void Update()
        {

            Dictionary<int, GameObject> map = GameObject.GetGameObjects();
            foreach (KeyValuePair<int, GameObject> entry in map)
            {
                if (entry.Value.IsActive())
                {
                    entry.Value.Update();
                }

            }

            foreach (KeyValuePair<int, GameObject> entry in map)
            {
                if (entry.Value.IsActive())
                {
                    entry.Value.LateUpdate();
                }

            }

            return;
        }

        public void Render()
        {
            Dictionary<int, GameObject> map = GameObject.GetGameObjects();
            foreach (KeyValuePair<int, GameObject> entry in map)
            {
                if (entry.Value.IsActive())
                {
                    entry.Value.Render();
                }
            }

            ConsoleUI.Render();
            ConsoleUI.ClearBuffer();
            return;
        }
    }
}

