//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

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

        protected Collider collider = null;

        public int HitPoints
        {
            get
            {
                return hp;
            }
        }

        public int Armor
        {
            get
            {
                return armor;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public int Attack
        {
            get
            {
                return attack;
            }
        }

        public int Level
        {
            get
            {
                return level;
            }
        }

        public Actor() { }

        public Actor(string name, string description, int level, int hp, int armor, int attack)
        {
            this.name = name;
            this.description = description;
            this.level = level;
            this.hp = hp;
            this.armor = armor;
            this.attack = attack;
        }

        public override void Start()
        {
            collider = (Collider)AddComponent(new Collider());
            return;
        }

        public override void Update()
        {
            return;
        }

    }
}
