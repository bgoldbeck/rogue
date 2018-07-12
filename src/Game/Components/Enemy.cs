//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using IO;

namespace Game.Components
{
    class Enemy : Actor, IDamageable, IMovable
    {
        public Transform target = null;

        public Enemy():base()
        {
            
        }

        public Enemy(string name, string description, int level, int hp, int armor, int attack)
            :base(name, description, level, hp, armor, attack)
        {
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
            Aggro search = (Aggro)GetComponent<Aggro>();
            EnemyAI ai = (EnemyAI)GetComponent<EnemyAI>();
            if(search == null)
            {
                Debug.LogError("Enemy didn't have an Aggro component.");
                return;
            }
            if (ai == null)
            {
                Debug.LogError("Enemy didn't have an Enemy AI component.");
                return;
            }

            search.TargetSearch();
            ai.MakeMove();

            return;
        }

        public override void Render()
        {
            return;
        }

        public void ApplyDamage(GameObject source, int damage)
        {
            // We don't want enemies attacking other enemies.
            if (source == null || source.GetComponent<Enemy>() != null) { return; }

            //Minuses the enemie's armor from the damage and makes sure it doesn't go less then 0.
            damage = (damage < armor) ? 0 : damage - armor;

            this.hp -= damage;
            // Made it a little easier to add stuff to the log from anywhere in
            // the game.
            HUD.Append("Attacked a " + Name + " for " + damage + " damage.");
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
            HUD.Append("Killed a " + Name);
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
