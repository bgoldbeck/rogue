using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;

namespace Game.Components
{
    class PlayerController : Component
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
            return;
        }

        public override void Render()
        {
            return;
        }
    }
}
