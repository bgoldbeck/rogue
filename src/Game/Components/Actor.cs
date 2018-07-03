using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    class Actor : Component
    {
        public String name = "";
        public String description = "";
        protected int hp = 0;
        protected int armor = 0;
        protected int attack = 0;
        protected int level = 0;

        public override void Start()
        {
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
