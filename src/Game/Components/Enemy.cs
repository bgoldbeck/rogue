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
        //private int lastMoved = 0;
        public Enemy()
        {
        }

        public Enemy(string name, string description, int level, int hp, int armor, int attack, int rate)
            :base(name, description, level, hp, armor, attack)
        {
            movementRate = rate;
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
            //GameObject.Destroy(this.gameObject);
            return;
        }

        public void Move(int dx, int dy)
        {
            return;
        }

    }
}
