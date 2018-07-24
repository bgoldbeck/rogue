//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using Game.Interfaces.Markers;
using IO;

namespace Game.Components
{
    public class Player : Actor, IMovable, IDamageable, IDoorOpener
    {

        public Player(string name, string description, int level, int hp, int armor, int attack)
            : base(name, description, level, hp, armor, attack,0)
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

        public void OnMove(int dx, int dy)
        {
            if (!base.TryMove(dx, dy))
            {
                List<IMovable> movables = gameObject.GetComponents<IMovable>();
                foreach (IMovable movable in movables)
                {
                     movable.OnFailedMove();
                }
            }
          
            return;
        }

        public void OnFailedMove()
        {
            //HUD.Append("You cant walk there.");
            return;
        }

        public void ApplyDamage(GameObject source, int damage)
        {
            base.ApplyDamage();
            //Minuses the  armor from the damage and makes sure it doesn't go less then 0.
            damage = (damage < armor) ? 0 : damage - armor;

            Enemy attacker = (Enemy)source.GetComponent<Enemy>();
            if (attacker != null)
            {
                HUD.Append(attacker.Name + " hit you for " + damage + " damage.");
            }
            else
            {
                Trap trap = (Trap)source.GetComponent<Trap>();
                if (trap != null)
                {
                    HUD.Append("You triggered at trap got hit for " + damage + " damage.");
                }
            }


            hp -= damage;

            // Play damage sound for player
            Console.Beep(375, 100);

            if(hp <= 0)
            {
                OnDeath(source);
            }
            return;
        }

        public void OnDeath(GameObject source)
        {
            HUD.Append(source.Name + " killed " + Name + " to death.");
            GameObject foundStateManager = GameObject.FindWithTag("StateManager");
            if(foundStateManager != null)
            {
                foundStateManager.SendMessage<StateManager>("PlayerIsDead");
            }
            else
            {
                Debug.LogError("State manager not found!");
            }
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
