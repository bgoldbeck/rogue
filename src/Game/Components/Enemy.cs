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
    class Enemy : Actor, IDamageable, IMovable, IAggressive
    {
        protected MapTile mapTile;
        protected EnemyAI ai;
        public Transform Target
        {
            get;
            protected set;
        }
        
        public Enemy():base()
        {
            
        }

        public Enemy(string name, string description, int level, int hp, int armor, int attack, int xp)
            :base(name, description, level, hp, armor, attack, xp)
        {
        }

        public override void Start()
        {
            base.Start();
            //Adds a Model component to the game object passed in.
            mapTile = (MapTile)this.gameObject.AddComponent(new MapTile());
            ai = (EnemyAI)this.gameObject.AddComponent(new EnemyAI());
            this.gameObject.AddComponent(new Aggro());
            this.gameObject.AddComponent(new Sound());
            mapTile.SetLightLevelAfterDiscovery(0.1f);
            return;
        }

        /// <summary>
        /// The update method handles the movement of the enemy.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // If enough time has passed since the enemy has moved, it checks if the enemy can
            //  see the player. If the enemy can see the player, it moves towards the play. Otherwise,
            //  it makes a random move.
            
            /*
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

            */

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
            HUD.Append("Attacked " + Name + " for " + damage + " damage.");

            // Play a rudimentary hit sound
            //Console.Beep(400, 100);

            if (hp <= 0)
            {
                // Notify other components on this game object of my death.
                // Why? maybe in the future we will play a sound on death, who knows?
                List<IDamageable> damageables = gameObject.GetComponents<IDamageable>();
                foreach (IDamageable damageable in damageables)
                {
                    damageable.OnDeath(source);
                }
            }
            return;
        }

        public void OnDeath(GameObject source)
        {
            // Killer gains exp n stuff for killing, right?
            
            HUD.Append(source.Name + " killed " + Name + ".");
            
            // We need to remove this enemy for the map too, right?
            Map.CacheInstance().PopObject(transform.position.x, transform.position.y);

            // Transfer inventory items from killed actor
            Inventory killedInventory = (Inventory)gameObject.GetComponent<Inventory>();
            killedInventory.MergeWith((Inventory)source.GetComponent<Inventory>());

            GameObject.Destroy(this.gameObject);

            // Play death sound
            //Console.Beep(200, 100);

            return;
        }

        public void OnMove(int dx, int dy)
        {
            base.TryMove(dx, dy);
            return;
        }

        protected override int CalculateDamage()
        {
            int damage = 1 * this.attack;
            // TODO: Get our damage, based on level of player,
            // The player's attack power, any equipment bonuses, Etc..
            return damage;
        }

        public void OnFailedMove()
        {
            return;
        }

        public void OnAggro(GameObject target)
        {
            Target = target.transform;
            return;
        }

        public void OnDeaggro()
        {
            Target = null;
            return;
        }
    }
}
