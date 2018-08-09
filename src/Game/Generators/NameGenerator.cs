#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.IO;
using System.Collections.Generic;

//NOTE: The names.txt file in the solution expert is NOT the one being used. The names.txt
//file being used is in ...\rogue-two\bin\Debug\netcoreapp2.1.

namespace Game.Generators
{
    class NameGenerator
    {
        /// <summary>
        /// Open the name file and returns a name that has a matching first character.
        /// </summary>
        /// <param name="character"> The character the name should start with.</param>
        /// <param name="rand"> The random seed used to choose the name.</param>
        /// <returns> The name chosen from the file.</returns>
        public static String GetNameStartingWith(char character, Random rand)
        {
            String returnedName = "Steve";  
            FileStream textFile = null;  
            List<String> possibleNames = new List<String>();
            String pathName = Path.Combine(Environment.CurrentDirectory, "names.txt");

            //If the names.txt file isn't found, the default name, Steve, is returned.
            if(!File.Exists(pathName))
            {
                return returnedName;
            }

            //If the file opens it randomly grabs the name.
            if ((textFile = File.OpenRead(pathName)) != null)
            {
                StreamReader textRead = new StreamReader(textFile);
                String currentLine = null;
                bool foundStart = false;
                //It reads the file line by line.
                while((currentLine = textRead.ReadLine()) != null)
                {
                    //If the current line has a matching character and adds it to the
                    //possible names list. If this was the first name that matches, it
                    //sets the boolean to say the section has been found.
                    if (currentLine[0].Equals(character))
                    {
                        possibleNames.Add(currentLine);
                        if (!foundStart)
                        {
                            foundStart = true;
                        }
                    }
                    //If the first character doesn't match and the section has been found
                    //it breaks out of the loop.
                    else if(foundStart)
                    {
                        break;
                    }
                }
                textRead.Close();
                textFile.Close();

                //It then uses the passed in random seed to choose a name from the list.
                if(possibleNames.Count != 0)
                {
                    returnedName = possibleNames[rand.Next() % possibleNames.Count];
                }
            }
            return returnedName;
        }
    }
}
