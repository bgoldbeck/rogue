using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

using Game.Components;

namespace Game
{
    public class Application
    {
        private bool isRunning = true;

        public void Initialize()
        {
            GameObject player = GameObject.Instantiate("Player");
            player.AddComponent(new Player());
            Transform transform = (Transform)player.GetComponent(typeof(Transform));

            Console.Out.WriteLine("Player x:" + transform.position.x + " Player y:" + transform.position.y);
            Console.Out.WriteLine("Player parent: " + transform.parent);
            return;
        }


        public int Loop()
        {
            while (isRunning)
            {
                Update();
                Render();
                isRunning = false;
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
            return;
        }
    }
}

