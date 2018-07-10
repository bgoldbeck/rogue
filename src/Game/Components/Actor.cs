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
    public class Actor : Component, IMovable
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

        public Actor() :base()
        {
        }

        public Actor(string name, string description, int level, int hp, int armor, int attack) :base()
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
            collider = (Collider)this.AddComponent(new Collider());
            return;
        }

        public override void Update()
        {
            return;
        }

        protected virtual int CalculateDamage()
        {
            return 0;
        }

        public bool Move(int dx, int dy)
        {
            bool moved = false;
            int newX = transform.position.x + dx;
            int newY = transform.position.y + dy;
            Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));

            // It checks the map to see if there is any collisions if the enemy moves to that square.
            if (collider.HandleCollision(dx, dy, out GameObject found) == Collider.CollisionTypes.None)
            {
                //If there is none, it moves the actor into the new square and updates the map.
                int oldX = transform.position.x;
                int oldY = transform.position.y;
                transform.Translate(dx, dy);
                map.PopObject(oldX, oldY);
                map.AddObject(newX, newY, gameObject);
                moved = true;
            }
            else
            {
                // If there is a collision, the actor doesn't move. If this collision is with a damageable, a
                // damage calculation is performed to calculate the amount of damage done to the player.
                if (found != null)
                {
                    // It's possible that we collided with something interactable.
                    List<IInteractable> interactables = found.GetComponents<IInteractable>();
                    foreach (IInteractable interactable in interactables)
                    {
                        interactable.Interact(this.gameObject);
                    }

                    // It's also possible that we collided with something damageable.
                    List<IDamageable> damageables = found.GetComponents<IDamageable>();
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.ApplyDamage(this.gameObject, CalculateDamage());
                    }
                }
            }
            return moved;
        }

    }
}
