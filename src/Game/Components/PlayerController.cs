using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;
using Game.Interfaces;

namespace Game.Components
{
    class PlayerController : Component, IMovable
    {
        Player player = null;

        public override void Start()
        {
            player = (Player)this.gameObject.GetComponent(typeof(Player));
            if (player == null)
            {
                Debug.LogError("Could not find player component from the player controller");
            }
            return;
        }

        public override void Update()
        {
            /*
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                
                List<IMovable> movables = gameObject.GetComponents<IMovable>();
                Console.WriteLine("Enter Key " + movables.Count);
                foreach (IMovable movable in movables)
                {
                    movable.Move(-5, -5);
                }
            }
            Console.ReadKey();
            */
            return;
        }

        public override void Render()
        {
            return;
        }

        public void Move(int dx, int dy)
        {
            Console.WriteLine("Player Controller move " + dx + " " + dy);
            return;
        }
    }
}
