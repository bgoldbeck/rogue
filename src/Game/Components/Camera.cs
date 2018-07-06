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
        private int width;
        private int height;
        private const float lightRadius = 10.0f;
        private const float totalLightIntensity = 5.0f;
        private const int lightRays = 20;
        private const float lightSpeed = .1f;

        public Camera(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            Map map = (Map)GameObject.FindWithTag("Map").GetComponent(typeof(Map));
            int playerX = gameObject.transform.position.x;
            int playerY = gameObject.transform.position.y;
            SpreadLight(map, playerX, playerY);

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
                        Color illuminated = tile.color.Apply(tile.lightLevel);
                        ConsoleUI.Write(x, y, tile.character, illuminated);
                    }
                }
            }
            return;
        }

        private void SpreadLight(Map map, int x, int y)
        {
            float angleDiff = 2 * (float)Math.PI / lightRays;
            float singleRayIntensity = totalLightIntensity / lightRays; 
            for (int i = 0; i < lightRays; ++i)
            {
                float xSpeed = lightSpeed * (float)Math.Cos(i * angleDiff);
                float ySpeed = lightSpeed * (float)Math.Sin(i * angleDiff);
                Ray ray = new Ray(x + .5f, y + .5f, xSpeed, ySpeed, singleRayIntensity);
                while (ray.IsWithinRadius())
                {
                    ray.Advance();
                    GameObject go = ray.Collides(gameObject, map);
                    if (go != null)
                    {
                        ray.Illuminate(go);
                        break;
                    }
                }
            }
        }

        public class Ray
        {
            public float x;
            public float y;
            public float initialX;
            public float initialY;
            public float dx;
            public float dy;
            public float intensity;

            public Ray(float x, float y, float dx, float dy, float intensity)
            {
                this.x = this.initialX = x;
                this.y = this.initialY = y;
                this.dx = dx;
                this.dy = dy;
                this.intensity = intensity;
            }

            public void Advance()
            {
                x += dx;
                y += dy;
            }

            public bool IsWithinRadius()
            {
                return Math.Sqrt(Math.Pow(x - initialX, 2) + Math.Pow(y - initialY, 2)) < lightRadius;
            }

            public GameObject Collides(GameObject player, Map map)
            {
                GameObject go = map.PeekObject((int)x, (int)y);
                if (go == player || go == null)
                    return null;
                return go;
            }

            public void Illuminate(GameObject go)
            {
                MapTile mt = (MapTile)go.GetComponent(typeof(MapTile));
                mt.lightLevel += intensity;
                if (mt.lightLevel > 1.0f)
                    mt.lightLevel = 1.0f;
            }
        }
    }
}
