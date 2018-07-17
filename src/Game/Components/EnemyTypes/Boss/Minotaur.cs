using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Components.EnemyTypes
{
    class Minotaur : Enemy
    {
        private bool enraged = false;
        public Minotaur(Random rand, int level)
            : base(
                  "Minotaur",                   //Enemy's name
                  "A mythical beast of fury.",  //Enemy's description
                  level,                        //Level of the enemy
                  10 * level,                   //Equation for the enemy's health.
                  level,                        //Equation for the enemy's armor.
                  2 * level,                    //Equation for the enemy's attack. 
                  50 * level                    //xp given by beating this enemy.
                  )
        {
        }
        public override void Start()
        {
            base.Start();
            mapTile.character = 'M';                    //Enemy's model
            mapTile.color.Set(110, 85, 20);             //Color
            ai.SetRate(3);                              //Time between each move.
        }
        public override void Update()
        {
            base.Update();
            enraged = enraged || hp < (maxHp / 2);
        }

        protected override int CalculateDamage()
        {
            int damage = enraged? 2 * this.attack : 1 * this.attack;
            // TODO: Get our damage, based on level of player,
            // The player's attack power, any equipment bonuses, Etc..
            return damage;
        }
    }
}
