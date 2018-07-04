//This file is disabled for not being a feasible answer for storing the data on the monsters.
/*using System;
using Game.Components;

namespace Game.Data
{
    /// <summary>
    /// The struct for the abstract data type of information stored on each monster.
    /// </summary>
    public struct MonsterData
    {
        string name;                //The name of the monster.
        string description;         //The description of the monster.
        static int[] hplevel;       //The hp of the monster based on its level.
        static int[] armorlevel;    //The armor of the monster based on its level.
        static int[] attacklevel;   //The attack of the monster based on its level.
    };

    /// <summary>
    /// The class for the abstract data type of choosing a monster and filling out the
    /// information on the monster based on the given level.
    /// </summary>
    public static class MonsterPicker : IMonsterData
    {
        //The array of monster data for each monster.
        MonsterData[] data =
        {
            //Name    Description   Level:        1  2  3  4  5  6
            {"Snake", "Snake? SNAKE!!!!",       { 1, 2, 3, 4, 5, 6 } //hp
                                        ,       { 1, 2, 3, 4, 5, 6 } //armor
                                        ,       { 1, 2, 3, 4, 5, 6 } //attack
            },
            {"Goblin", "Just a normal goblin.", { 1, 2, 3, 4, 5, 6 } //hp
                                              , { 1, 2, 3, 4, 5, 6 } //armor
                                              , { 1, 2, 3, 4, 5, 6 } //attack
            }
        };

        /// <summary>
        /// Takes in an instance of a random variable and a level and fills in a target
        /// enemy with the information on the monster.
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="level"></param>
        /// <param name="target"></param>
        static void Fill(Random rand, int level, Enemy target)
        {
            int value = rand.Next % this.data.Length;

            target.name = this.data[value].name;
            target.description = this.data[value].description;
            target.level = level;
            target.hp = this.data[value].hplevel[level-1];
            target.armor = this.data[value].armorlevel[level-1];
            target.attack = this.data[value].attacklevel[level-1];
        }
    };
}*/
