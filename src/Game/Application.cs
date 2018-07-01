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

        public void Initialize()
        {
            SetupScreen();

            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            ConsoleUI.Initialize(width, height);

            Time.Initialize();

            GameObject map = GameObject.Instantiate("Map");
            map.AddComponent(new Map(width, height));
            //The map seems to add it's own model when starting. map.AddComponent(new Model());

            GameObject player = GameObject.Instantiate("Player");
            player.AddComponent(new Player());
            player.AddComponent(new PlayerController());
            player.AddComponent(new Model());

            Model playerModel = (Model)player.GetComponent(typeof(Model));
            playerModel.model.Clear();
            playerModel.model.Add("$");

            
            player.transform.Translate(5, 5);
            
            //Console.Out.WriteLine("Player x:" + player.transform.position.x + " Player y:" + player.transform.position.y);
            //Console.Out.WriteLine("Player parent: " + transform.parent);
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
                

                
                ConsoleKey press = Input.ReadKey().Key;
                
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

            Dictionary<String, GameObject> map = GameObject.GetGameObjects();
            foreach (KeyValuePair<string, GameObject> entry in map)
            {
                if (entry.Value.IsActive())
                {
                    entry.Value.Update();
                }

            }

            
            return;
        }

        public void Render()
        {
            Dictionary<String, GameObject> map = GameObject.GetGameObjects();
            foreach (KeyValuePair<string, GameObject> entry in map)
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

