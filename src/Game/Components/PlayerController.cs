//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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
    class PlayerController : Component
    {
        private Player player = null;

        private int dx = 0;
        private int dy = 0;

        public override void Start()
        {       
            return;
        }

        public override void OnEnable()
        {
            player = (Player)this.gameObject.GetComponent(typeof(Player));
            if (player == null)
            {
                Debug.LogError("Could not find player component from the player controller");
            }
        }

        public override void Update()
        {

            ConsoleKey press = Input.ReadKey().Key;
            switch (press)
            {
                case ConsoleKey.UpArrow:
                    dy = 1;
                    break;
                case ConsoleKey.RightArrow:
                    dx = 1;
                    break;
                case ConsoleKey.DownArrow:
                    dy = -1;
                    break;
                case ConsoleKey.LeftArrow:
                    dx = -1;
                    break;
                default:
                    break;
            }
           
            if (dx != 0 || dy != 0)
            {
                List<IMovable> movables = gameObject.GetComponents<IMovable>();
                foreach (IMovable movable in movables)
                {
                    movable.Move(dx, dy);
                }
                dx = dy = 0;
            }
           
            return;
        }

        public override void Render()
        {
            return;
        }

    }
}
