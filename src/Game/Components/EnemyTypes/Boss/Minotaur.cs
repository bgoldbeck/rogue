//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Game.Interfaces.Markers;

namespace Game.Components.EnemyTypes
{
    class Minotaur : Enemy, IRage, IXRayVision
    {
        private bool enraged = false;
        public bool HasVision { get; } = false;
        public Minotaur(Random rand, int level, bool isShiny) 
            : base(
                  "Minotaur",                   //Enemy's name
                  "A mythical beast of fury.",  //Enemy's description
                  level,                        //Level of the enemy
                  10 * level,                   //Equation for the enemy's health.
                  level,                        //Equation for the enemy's armor.
                  2 * level,                    //Equation for the enemy's attack. 
                  50 * level,                    //xp given by beating this enemy.
                  isShiny,
                  rand
                  )
        {
            HasVision = isShiny;
        }
        public override void Start()
        {
            base.Start();
            if(isShiny)
            {
                enraged = true;
            }
            mapTile.character = 'M';                    //Enemy's model
            if (isShiny)                            //Color
            {
                mapTile.color.Set(255, 215, 0);
            }
            else
            {
                mapTile.color.Set(110, 85, 20);
            }
            ai.SetRate(1.5f / ((isShiny) ? 2 : 1));                      //Time between each move.
            healthRegen.SetHealthRegen(10.0f / ((isShiny) ? 2 : 1));     //Health regen (seconds for 1 health regen).
            aggro.SetAggroPatience(2.0f);                                //Seconds before it gives up.
            aggro.SetAggroRange(12);                                     //Distance it can see the player at.
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
                    ai.SetRate(1f);
                    healthRegen.SetHealthRegen(7.0f); 
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
