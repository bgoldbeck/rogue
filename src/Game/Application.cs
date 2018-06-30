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

            ConsoleUI.Initialize(140, 50);
            Time.Initialize();

            GameObject map = GameObject.Instantiate("Map");
            map.AddComponent(new Map());
            map.AddComponent(new Model());

            GameObject player = GameObject.Instantiate("Player");
            player.AddComponent(new Player());
            player.AddComponent(new Model());

            Model playerModel = (Model)player.GetComponent(typeof(Model));
            playerModel.model.Clear();
            playerModel.model.Add("$");

            
            player.transform.Translate(5, 5);
            
            //Console.Out.WriteLine("Player x:" + player.transform.position.x + " Player y:" + player.transform.position.y);
            //Console.Out.WriteLine("Player parent: " + transform.parent);
            return;
        }


        public int Loop()
        {
            long dt = 0;

            while (isRunning)
            {

                dt += Time.deltaTicks;
                Update();
                if (dt >= 1600000)
                {
                    dt = 0;
                    Render();
                }
                if (Input.ReadKey().Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
                Time.Update();
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

