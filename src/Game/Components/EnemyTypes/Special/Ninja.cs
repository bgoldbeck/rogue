//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;

using Game.Interfaces;
using Ecs;

namespace Game.Components.EnemyTypes
{
    class Ninja : Enemy, IHidden
    {
        public bool IsHidden { protected set; get; } = true;

        public Ninja(Random rand, int level, bool isShiny)
        : base(
              "Ninja",                      //Enemy's name
              "You see nothing...",         //Enemy's description
              level,                        //Level of the enemy
              4 * level,                    //Equation for the enemy's health.
              (level > 1) ? level - 1 : 0,   //Equation for the enemy's armor.
              3 + level,                    //Equation for the enemy's attack. 
              15 * level,                   //xp given by beating this enemy.
              isShiny,
              rand
              )
        {
        }

        public override void Start()
        {
            base.Start();
            mapTile.character = ' ';                    //Enemy's model
            mapTile.color.Set(10, 10, 10);
            ai.SetRate(0.7f / ((isShiny) ? 2 : 1));                           //Time between each move.
            healthRegen.SetHealthRegen(15.0f / ((isShiny) ? 2 : 1));          //Health regen (seconds for 1 health regen).
            aggro.SetAggroPatience(4.0f);                                //Seconds before it gives up.
            aggro.SetAggroRange(14);                                     //Distance it can see the player at.
        }

        public void Reveal()
        {
            IsHidden = false;
            mapTile.character = 'n';
            if (isShiny)                          
            {
                mapTile.color.Set(255, 215, 0);
            }
            else
            {
                mapTile.color.Set(70, 70, 70);
            }
        }

        public override void Update()
        {
            Player player = Player.MainPlayer();
            if (player != null)
            {
                if (IsHidden)
                {
                    if (Vec2i.Heuristic(transform.position, player.transform.position) < 4)
                        Reveal();
                }
                else
                {
                    if (Vec2i.Heuristic(transform.position, player.transform.position) > 4)
                    {
                        mapTile.character = ' ';
                        mapTile.color.Set(10, 10, 10);
                        IsHidden = true;
                    }
                }
            }
            return;
        }
    }
}
