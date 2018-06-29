using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

namespace Game.Components
{
    class Player : Component
    {
        public String name = "";

        public override void Start()
        {
            Console.Out.WriteLine("Player started " + name);
            return;
        }

        public override void Update()
        {
            //System.out.println("Player updated");
            return;
        }

        public override void Render()
        {
            //System.out.println("Player rendered");
            return;
        }
    }
}
