//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using IO;

namespace Game.Components
{
    class Camera : Component
    {
        public int width;  // Width of camera drawing area
        public int height; // Height of camera drawing area
        private Map map = null; // Reference to map component
        //private static Camera camera = null;    // Reference to camera component

        public static Camera CacheInstance()
        {
            Camera camera = null;
            GameObject go = GameObject.FindWithTag("Camera");
            if (go != null)
            {
                camera = (Camera)go.GetComponent<Camera>();
            }
            return camera;
        }

        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Start()
        {
            //if (camera != null && camera != this)
            //{
            //    GameObject.Destroy(this.gameObject);
            //}
            //else
            //{
            //    camera = this;
            //}
            return;
        }

        public override void OnEnable()
        {

            map = MapManager.CurrentMap();
            if (map == null)
            {
                Debug.LogError("Camera could not find a map to render from!");
            }
            
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            if (map == null) return;

            int playerX = gameObject.transform.position.x;
            int playerY = gameObject.transform.position.y;
        
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int xToCheck = playerX - halfWidth + x;
                    int yToCheck = playerY - halfHeight + y;
                    GameObject go = map.PeekObject(xToCheck, yToCheck);
                    if (go != null)
                    {
                        MapTile tile = (MapTile)go.GetComponent(typeof(MapTile));
                        if (tile.disableLighting)
                            ConsoleUI.Write(x, y, tile.character, tile.color);
                        else
                        {
                            Color illuminated = tile.color.Apply(tile.lightLevel);
                            ConsoleUI.Write(x, y, tile.character, illuminated);
                        }
                    }
                }
            }
            return;
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void OnDestroy()
        {
            
            return;
        }

    }
}
