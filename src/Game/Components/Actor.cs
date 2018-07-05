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

        public Actor() { }

        public Actor(string name, string description, int level, int hp, int arm, int attack)
        {
            this.name = name;
            this.description = description;
            this.level = level;
            this.hp = hp;
            this.armor = arm;
            this.attack = attack;
        }

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
