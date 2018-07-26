using System;
using System.Collections.Generic;
using System.Text;

using Ecs;
using IO;

namespace Game.Components
{
    class MapManager : Component
    {
        private static List<Map> drawnMaps = new List<Map>();
        private static int currentMap = 0;
        private int mapWidth = 80;
        private int mapHeight = 40;
        private int mapLevel = 1;

        public MapManager()
        {

        }

        public MapManager(int newWidth, int newHeight, int newLevel)
        {
            mapWidth = newWidth;
            mapHeight = newHeight;
            mapLevel = newLevel;
        }
        public override void Start()
        {
            if (drawnMaps.Count == 0)
            {
                Map firstMap = new Map(mapWidth, mapHeight, mapLevel);
                drawnMaps.Add(firstMap);
                gameObject.UpdateComponent(firstMap);
            }
            else
            {
                Debug.LogWarning("There should only be one instance of MapManager.");
            }
        }

        public static Map CurrentMap()
        {
            if (drawnMaps.Count > 0 && currentMap < drawnMaps.Count)
            {
                return drawnMaps[currentMap];
            }
            else
            {
                return null;
            }
        }


    }
}
