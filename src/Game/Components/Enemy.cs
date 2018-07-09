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
    class Enemy : Actor, IDamageable, IMovable
    {
        private int movementRate = 0;
        private int lastMoved = 0;
        public Enemy():base()
        {
            
        }

        public Enemy(string name, string description, int level, int hp, int armor, int attack, int rate)
            :base(name, description, level, hp, armor, attack)
        {
            movementRate = rate;
        }

        public override void Start()
        {
            base.Start();
            return;
        }

        public override void Update()
        {
            base.Update();

            if (this == null)
            {
                return;
            }
            if (lastMoved >= movementRate)
            {
                Random rand = new Random();
                int dx = 0, dy = 0;
                switch(rand.Next() % 5)
                {
                    case 0:
                        dx = 1;
                        break;
                    case 1:
                        dx = -1;
                        break;
                    case 2:
                        dy = 1;
                        break;
                    case 3:
                        dy = -1;
                        break;
                    default:
                        break;
                }
                Move(dx, dy);
                lastMoved = 0;
            }
            else
            {
                ++lastMoved;
            }
            return;
        }

        public override void Render()
        {
            return;
        }

        public void ApplyDamage(int damage)
        {
            this.hp -= damage;
            // Made it a little easier to add stuff to the log from anywhere in
            // the game.
            HUD.Append("Attacked a " + name + " for " + damage + " damage.");
            if (hp <= 0)
            {
                // Notify other components on this game object of my death.
                // Why? maybe in the future we will play a sound on death, who knows?
                List<IDamageable> damageables = gameObject.GetComponents<IDamageable>();
                foreach (IDamageable damageable in damageables)
                {
                    damageable.OnDeath();
                }
            }
            return;
        }

        public void OnDeath()
        {
            // Players gains exp n stuff for killing, right?
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null)
            {
                Player player = (Player)go.GetComponent<Player>();
                if (player != null)
                {
                    // player.IncreaseExperience(over 9000);
                }
            }
            HUD.Append("Killed a " + name);
            // We need to remove this enemy for the map too, right?
            Map.CacheInstance().PopObject(transform.position.x, transform.position.y);
            GameObject.Destroy(this.gameObject);
            // Spawn items on death?, or maybe a blood splat?
            return;
        }

        public void Move(int dx, int dy)
        {
            int newX = transform.position.x + dx;
            int newY = transform.position.y + dy;
            Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));


            if (collider.HandleCollision(dx, dy, out GameObject found) == Collider.CollisionTypes.None)
            {
                int oldX = transform.position.x;
                int oldY = transform.position.y;
                transform.Translate(dx, dy);
                map.PopObject(oldX, oldY);
                map.AddObject(newX, newY, gameObject);
            }

            return;
        }
        private int CalculateDamage()
        {
            int damage = 1 * this.attack;
            // TODO: Get our damage, based on level of player,
            // The player's attack power, any equipment bonuses, Etc..
            return damage;
        }
    }
}
