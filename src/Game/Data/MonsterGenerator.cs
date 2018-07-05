using System;
using Game.Components;
using Ecs;

namespace Game.Data
{

    class MonsterGenerator
    {
        static private Enemy SnakeGenerator(int level, string model)
        {
            model = "s";                             //Monster's model
            return new Enemy("Snake",                //Monster's name
                             "Snake? SNAKE!!!!",    //Monster's description
                             level,                 //Level of the monster
                             2 + (3 * level),       //Equation for the monster's health.
                             level,                 //Equation for the monster's armor.
                             level                  //Equation for the monster's attack.
                             );
        }
        static private Enemy GoblinGenerator(int level, string model)
        {
            model = "g";                                //Monster's model
            return new Enemy("Goblin",                  //Monster name
                             "Just a normal Goblin",    //Monster description
                             level,                     //Level of the monster
                             5 * level,                 //Equation for the monster's health.
                             (level > 1)? level - 1 : 0,//Equation for the monster's armor.
                             2 + level                  //Equation for the monster's attack.
                            );
        }

        //This function was inspired by this discussion on Stack Overflow:
        //https://stackoverflow.com/questions/3767942/storing-a-list-of-methods-in-c-sharp

        delegate Enemy spawnGenerator(int l, string s);
        static public void Fill(Random rand, int level, GameObject slot)
        {
            string model = "";
            Model mod = (Model)slot.AddComponent(new Model());

            spawnGenerator[] generatorArr = new spawnGenerator[]
            {
                (param1, param2) => SnakeGenerator(param1, param2),
                (param1, param2) => GoblinGenerator(param1, param2),
            };
         
            int value = rand.Next() % generatorArr.Length;

            slot.AddComponent(generatorArr[value](level, model));
            mod.model.Add(model);
        }
    }
}