//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

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

        public override void EarlyUpdate()
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
                case ConsoleKey.C:
                    ConsoleUI.ToggleColor();
                    break;
#if DEBUG
                case ConsoleKey.H:
                    player.BoostHitPoints(10);
                    break;
#endif
                //case ConsoleKey.V:
                //    Enemy.TogglePathVisibility();
                //    break;
                default:
                    break;
            }
           
            if (dx != 0 || dy != 0)
            {
                
                gameObject.SendMessage<IMovable>("OnMove", new object[] { dx, dy });

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
