//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using Game.Components;
using Ecs;

namespace Game.Data
{
    class MonsterGenerator
    {
        /// <summary>
        /// This function generates an instance of Enemy using the level and fills it in for an enemy type snake. 
        /// It also returns the model of the enemy using a reference of an array of strings.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        static private Enemy SnakeGenerator(int level, string[] model)
        {
            model[0] = "s";                         //Monster's model
            return new Enemy("Snake",               //Monster's name
                             "Snake? SNAKE!!!!",    //Monster's description
                             level,                 //Level of the monster
                             2 + (3 * level),       //Equation for the monster's health.
                             level,                 //Equation for the monster's armor.
                             level,                 //Equation for the monster's attack.
                             3                      //Time between each move.
                             );
        }

        /// <summary>
        /// This function generates an instance of Enemy using the level and fills it in for an enemy type goblin. 
        /// It also returns the model of the enemy using a reference of an array of strings.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        static private Enemy GoblinGenerator(int level, string[] model)
        {
            model[0] = "g";                             //Monster's model
            return new Enemy("Goblin",                  //Monster name
                             "Just a normal Goblin",    //Monster description
                             level,                     //Level of the monster
                             5 * level,                 //Equation for the monster's health.
                             (level > 1)? level - 1 : 0,//Equation for the monster's armor.
                             2 + level,                 //Equation for the monster's attack.
                             3                          //Time between each move.                          
                             );
        }

        //This function and delegate were inspired by this discussion on Stack Overflow:
        //https://stackoverflow.com/questions/3767942/storing-a-list-of-methods-in-c-sharp
        delegate Enemy spawnGenerator(int l, string[] s);
        /// <summary>
        /// This function takes in a reference to the Random class, a level, and a GameObject
        /// and randomly selects a monster and generates the enemy.
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="level"></param>
        /// <param name="slot"></param>
        static public void Fill(Random rand, int level, GameObject slot)
        {
            string[] model = new string[1];
            model[0] = "!";
            //Adds a Model component to the game object passed in.
            Model mod = (Model)slot.AddComponent(new Model());

            //Generates an array of methods for each monster type.
            spawnGenerator[] generatorArr = new spawnGenerator[]
            {
                (param1, param2) => SnakeGenerator(param1, param2),
                (param1, param2) => GoblinGenerator(param1, param2),
            };
         
            //Using the passed in Random instance, a random monster is picked and the
            //information on the enemy is filled in.
            int value = rand.Next() % generatorArr.Length;
            slot.AddComponent(generatorArr[value](level, model));
            mod.model.Add(model[0]);

            mod.colorModel.Add(new List<String>());
            mod.colorModel[0].Add("\u001b[33m");
        }
    }
}