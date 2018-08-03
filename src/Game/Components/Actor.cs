//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;

using Ecs;
using Game.Interfaces;

namespace Game.Components
{
    public class Actor : Component
    {
        //public String name = "";
        public String description = "";
        protected int maxHp = 0;
        protected int hp = 0;
        protected int armor = 0;
        protected int attack = 0;
        protected int level = 0;
        protected int xp = 0;
        protected List<Item> inventory = new List<Item>();

        protected Collider collider = null;
        protected Health healthRegen = null;

        public new String Name { get; set; }

        public void BoostHitPoints(int amount)
        {
            hp += amount;
        }

        public int HitPoints
        {
            get
            {
                return hp;
            }
        }

        public int MaxHitPoints
        {
            get
            {
                return maxHp;
            }
        }

        public int Armor
        {
            get
            {
                return armor;
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

        public int Xp
        {
            get
            {
                return xp;
            }
        }

        public Actor() : base()
        {
        }

        public Actor(string name, string description, int level, int hp, int armor, int attack, int xp) : base()
        {
            this.Name = name;
            this.description = description;
            this.level = level;
            this.hp = hp;
            this.maxHp = hp;
            this.armor = armor;
            this.attack = attack;
            this.xp = xp;
        }

        public override void Start()
        {
            this.gameObject.Name = this.Name;
            collider = (Collider)this.AddComponent(new Collider());
            healthRegen = (Health)this.AddComponent(new Health());
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

        public void GiveXp(int givenXp)
        {
            xp += givenXp;
        }

        public void RegenHP()
        {
            if(hp < maxHp)
            {
                ++hp;
            }
        }

        public bool TryMove(int dx, int dy)
        {
            bool moved = false;
            Map map = MapManager.CurrentMap();
            if (map == null)
            {
                return false;
            }
            Collider.CollisionTypes type = collider.HandleCollision(dx, dy, out GameObject found);
            // It checks the map to see if there is any collisions if the enemy moves to that square.
            if (type == Collider.CollisionTypes.None)
            {
                //If there is none, it moves the actor into the new square and updates the map.

                map.PopObject(transform.position.x, transform.position.y);
                moved = map.AddObject(transform.position.x + dx, transform.position.y + dy, gameObject);
                MapManager.CurrentNavigationMap().UpdatePositions(transform.position, new Vec2i(transform.position.x + dx, transform.position.y + dy));

                if (moved)
                {
                    transform.Translate(dx, dy);
                }
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
                        if (interactable.IsInteractable)
                        {
                            interactable.OnInteract(this.gameObject, this);
                        }
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

        public void ApplyDamage()
        {
            gameObject.SendMessage<IRegen>("InBattle");
        }

    }
}
