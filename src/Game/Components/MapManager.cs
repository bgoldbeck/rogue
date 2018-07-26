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
        private static List<NavigatorMap> drawnGraphs = new List<NavigatorMap>();
        private static int currentMap = 0;
        private static int mapWidth = 80;
        private static int mapHeight = 40;
        private static int mapLevel = 1;

        private static MapManager currentManager = null;

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
            if (currentManager == null || currentManager == this)
            {
                currentManager = this;
                if (drawnMaps.Count == 0)
                {
                    Map firstMap = new Map(mapWidth, mapHeight, mapLevel);
                    drawnMaps.Add(firstMap);
                    gameObject.UpdateComponent(firstMap);
                    NavigatorMap firstGraph = new NavigatorMap();
                    drawnGraphs.Add(firstGraph);
                    gameObject.UpdateComponent(firstGraph);
                }
                else
                {
                    Debug.LogWarning("There should only be one instance of MapManager.");
                }
            }
        }
        public static void SwitchToNextMap()
        {
            if (currentManager != null)
            {
                Map mapBeingDisabled = CurrentMap();
                if (mapBeingDisabled != null)
                {
                    mapBeingDisabled.gameObject.ChangeHierarchyActive(false);
                    ++currentMap;
                }
                Map mapBeingCreated = new Map(mapWidth, mapHeight, mapLevel);
                drawnMaps.Add(mapBeingCreated);
                currentManager.gameObject.UpdateComponent(mapBeingCreated);
                NavigatorMap graphBeingCreated = new NavigatorMap();
                drawnGraphs.Add(graphBeingCreated);
                currentManager.gameObject.UpdateComponent(graphBeingCreated);
            }
        }

        public static Map CurrentMap()
        {
            if (currentManager == null)
            {
                Console.Out.WriteLine("Manager was null");
                Console.In.ReadLine();
                return null;
            }
            if (drawnMaps.Count > 0 && currentMap < drawnMaps.Count)
            {
                return drawnMaps[currentMap];
            }
            else
            {
                Console.Out.WriteLine("drawnMaps count is wrong");
                Console.In.ReadLine();
                return null;
            }
        }

        public static NavigatorMap CurrentNavigationMap()
        {
            if (currentManager == null)
            {
                return null;
            }
            if (drawnGraphs.Count > 0 && currentMap < drawnGraphs.Count)
            {
                return drawnGraphs[currentMap];
            }
            else
            {
                return null;
            }
        }
        public override void OnDestroy()
        {
            currentMap = 0;
            drawnMaps = new List<Map>();
            drawnGraphs = new List<NavigatorMap>();
            currentManager = null;
            return;
        }
    }
}


