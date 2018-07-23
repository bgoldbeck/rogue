using System;
using System.Collections.Generic;
using System.Text;

using Game.Interfaces.Markers;

namespace Game.Components.EnemyTypes
{
    class Minotaur : Enemy, IRage
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
            ai.SetRate(1500);                              //Time between each move.
        }
        public override void Update()
        {
            base.Update();
            if (!enraged)
            {
                if(hp < (maxHp / 2))
                {
                    HUD.Append("The Minotaur has been enraged!");
                    enraged = true;
                    attack *= 2;
                    ai.SetRate(500);
                }
            }
        }

        public bool isRaging
        {
            get
            {
                return enraged;
            }
        }

    }
}
