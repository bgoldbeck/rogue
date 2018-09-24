#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System.Collections.Generic;

using Ecs;
using IO;

namespace Game.Components
{
    /// <summary>
    /// This class maintains the current list of maps and graphs.
    /// </summary>
    class MapManager : Component
    {
        private static List<GameObject> drawnMaps = new List<GameObject>();   //The list of maps drawn.
        private static List<GameObject> drawnGraphs = new List<GameObject>(); //Ths list of graphs drawn.
        private static int currentMap = 0;  //The current map and graph being used.
        private static int mapWidth = 80;   //The default width of the maps.
        private static int mapHeight = 40;  //The default height of the maps.
        private static int mapLevel = 1;    //The default level of enemies spawned into the map.

        private static MapManager currentManager = null; //The reference of the current MapManager.

        /// <summary>
        /// The default constructor which allows the instant of MapManager to use the default settings.
        /// </summary>
        public MapManager()
        {

        }

        /// <summary>
        /// The constructor that allows the setting for the map to be modified.
        /// </summary>
        /// <param name="newWidth"> The new default width of maps generated.</param>
        /// <param name="newHeight"> The new default height of maps generated.</param>
        /// <param name="newLevel"> The new level of monsters in the maps generated.</param>
        public MapManager(int newWidth, int newHeight, int newLevel)
        {
            mapWidth = newWidth;
            mapHeight = newHeight;
            mapLevel = newLevel;
        }

        /// <summary>
        /// In the start() method, it first checks if it's the only MapManager. If it is
        /// it generates the first map and set it to the current map.
        /// </summary>
        public override void Start()
        {
            //If there is another MapManager, this instance doesn't make any changes.
            if (currentManager == null || currentManager == this)
            {
                //Otherwise, it sets itself as the current MapManager.
                currentManager = this;
                if (drawnMaps.Count == 0)
                {
                    //It then generates the first map and Nabigator
                    Map firstMap = new Map(mapWidth, mapHeight, mapLevel);
                    GameObject mapObject = GameObject.Instantiate();
                    mapObject.AddComponent(firstMap);
                    firstMap.CreateLevel(1);
                    drawnMaps.Add(mapObject);

                    NavigatorMap firstGraph = new NavigatorMap();
                    GameObject graphObject = GameObject.Instantiate();
                    graphObject.AddComponent(firstGraph);
                    drawnGraphs.Add(graphObject);
                }
                else
                {
                    Debug.LogWarning("There should only be one instance of MapManager.");
                }
            }
            
        }

        /// <summary>
        /// This method disables the current map and creates the next map and graph and sets it as the current.
        /// This won't work right now. <----
        /// </summary>
        /*public static void SwitchToNextMap()
        {
            if (currentManager != null)
            {
                // Destroy the current map, and graph components attached this game object.
                //currentManager.gameObject.Destroy(drawnMaps[currentMap]);
                //currentManager.gameObject.Destroy(drawnGraphs[currentMap]);

                //It first grabs the current map and disables it.
                Map mapBeingDisabled = CurrentMap();

                if (mapBeingDisabled != null)
                {
                    mapBeingDisabled.gameObject.SetActiveRecursively(false);
                    ++currentMap;
                }
                
                //Then the next map and graph are created and the current map is set to it.
                Map mapBeingCreated = new Map(mapWidth, mapHeight, mapLevel);
                
                //drawnMaps.Add(mapBeingCreated);

                currentManager.gameObject.AddComponent(mapBeingCreated);

                NavigatorMap graphBeingCreated = new NavigatorMap();

                //drawnGraphs.Add(graphBeingCreated);

                currentManager.gameObject.AddComponent(graphBeingCreated);
            }
        }*/

        /// <summary>
        /// Returns the current map being used.
        /// </summary>
        /// <returns> the current map being used.</returns>
        public static Map CurrentMap()
        {
            //If there isn't an instance of the MapManager, it returns null.
            if (currentManager == null)
            {
                return null;
            }
            //If the currentMap is inside the range of the available maps, it returns
            //the current map.
            if (drawnMaps.Count > 0 && currentMap < drawnMaps.Count)
            {
                return (Map)drawnMaps[currentMap]?.GetComponent(typeof(Map));
            }
            //If the currentMap is outside the range, it returns null.
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the current navigator map being used.
        /// </summary>
        /// <returns> the current navigator map being used.</returns>
        public static NavigatorMap CurrentNavigationMap()
        {
            //If there isn't an instance of the MapManager, it returns null.
            if (currentManager == null)
            {
                return null;
            }
            //If the currentMap is inside the range of the available maps, it returns
            //the current map.
            if (drawnGraphs.Count > 0 && currentMap < drawnGraphs.Count)
            {
                return (NavigatorMap)drawnGraphs[currentMap].GetComponent(typeof(NavigatorMap));
            }
            //If the currentMap is outside the range, it returns null.
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Restores the starting state of the MapManager when the instance of
        /// the map manager is destroyed.
        /// </summary>
        public override void OnDestroy()
        {
            currentMap = 0;
            drawnMaps = new List<GameObject>();
            drawnGraphs = new List<GameObject>();
            currentManager = null;
            return;
        }
    }
}


