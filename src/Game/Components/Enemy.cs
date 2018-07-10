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
        private int movementRate = 3;
        private int lastMoved = 0;
        private Transform target = null;
        private int aggroRange = 10;
        private int lastSeenPlayer = 0;
        private int determination = 3;

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

        /// <summary>
        /// The update method handles the movement of the enemy.
        /// </summary>
        public override void Update()
        {
            base.Update();

            /*If enough time has passed since the enemy has moved, it checks if the enemy can
              see the player. If the enemy can see the player, it moves towards the play. Otherwise,
              it makes a random move.*/
            if (lastMoved >= movementRate)
            {
                PlayerSearch();
                if (target != null)
                {
                    SeekMove();
                }
                else
                {
                    RandomMove();
                }
                lastMoved = 0;
            }
            else
            {
                ++lastMoved;
            }
            return;
        }

        /// <summary>
        /// This method checks the distance the player is away from this enemy.
        /// </summary>
        private void PlayerSearch()
        {
            //If the enemy is close enough to the player, it saves the location it has seen the
            //player at and set the boolean that is has seen the player.
            Player player = (Player)GameObject.FindWithTag("Player").GetComponent(typeof(Player));

            if (Vec2i.Distance(player.transform.position, transform.position) < aggroRange)
            {
                target = player.transform;
            }
            //If the enemy can't see the player but has seen the player before. It checks how long
            //since the last time it has seen the player. If it has been too long, it sets the boolean
            //to false.
            else if(target != null)
            {
                if(++lastSeenPlayer > determination)
                {
                    target = null;
                }
            }
                
        }

        /// <summary>
        /// This method moves the enemy towards a known player.
        /// </summary>
        private void SeekMove()
        {
            if (target == null) return;

            Random rand = new Random();

            //Figures out which direction on the it has to move to head towards the player.
            Vec2i deltaMove = target.position - transform.position;
            if (deltaMove.x != 0)
            { 
                deltaMove.x = deltaMove.x / Math.Abs(deltaMove.x);
            }
            if (deltaMove.y != 0)
            { 
                deltaMove.y = deltaMove.y / Math.Abs(deltaMove.y);
            }

            //It randomly decides whether to try to move on the X-axis or Y-axis.
            bool moveOnX = rand.Next() % 2 == 0 && deltaMove.x != 0;
            deltaMove.x = moveOnX ? deltaMove.x : 0;
            deltaMove.y = moveOnX ? 0 : deltaMove.y;
            
            Move(deltaMove.x, deltaMove.y);
        }

        /// <summary>
        /// This method randomly move the enemy.
        /// </summary>
        private void RandomMove()
        {
            Random rand = new Random();
            int dx = 0, dy = 0;

            //The enemy can randomly move in 5 ways:left, up, right, down, and not move.
            switch (rand.Next() % 5)
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
        }

        public override void Render()
        {
            return;
        }

        public void ApplyDamage(GameObject source, int damage)
        {
            // We don't want enemies attacking other enemies.
            if (source == null || source.GetComponent<Enemy>() == null) { return; }

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

        public new void Move(int dx, int dy)
        {
            base.Move(dx, dy);
            return;
        }

        protected override int CalculateDamage()
        {
            int damage = 1 * this.attack;
            // TODO: Get our damage, based on level of player,
            // The player's attack power, any equipment bonuses, Etc..
            return damage;
        }

    }
}
