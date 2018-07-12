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
    public class Player : Actor, IMovable, IDamageable
    {

        public Player(string name, string description, int level, int hp, int armor, int attack)
            : base(name, description, level, hp, armor, attack)
        {
        }

        public static Player MainPlayer()
        {
            Player player = null;
            GameObject go = GameObject.FindWithTag("MainPlayer");
            if (go != null)
            {
                player = (Player)go.GetComponent<Player>();
            }
            return player;
        }

        public override void Start()
        {
            base.Start();
            return;
        }

        public override void Update()
        {
            base.Update();
            return;
        }

        public new void Move(int dx, int dy)
        {
            if (base.Move(dx, dy))
            {
                //HUD.Append("You walked successfully.");
            }
            return;
        }

        public void ApplyDamage(GameObject source, int damage)
        {
            Enemy attacker = (Enemy)source.GetComponent<Enemy>();

            //Minuses the enemie's armor from the damage and makes sure it doesn't go less then 0.
            damage = (damage < armor) ? 0 : damage - armor;

            if (attacker != null)
            {
                HUD.Append(attacker.Name + " attacked for " + damage + " damage.");
            }

            hp -= damage;
            if(hp <= 0)
            {
                OnDeath();
            }
            return;
        }

        public void OnDeath()
        {
            HUD.Append(Name + " has died.");
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
