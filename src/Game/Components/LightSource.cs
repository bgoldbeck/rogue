//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using Game.Interfaces;
using IO;

namespace Game.Components
{
    class LightSource : Component
    {
        private static float lightRadius = 10.0f;   // Maximum distance to illuminate
        private static int lightRays = 1000;    // Number of rays to emit
        private static float lightSpeed = .5f;  // Distance to move each point of light per step
        private float totalLightIntensity = 10.0f;  // Amount of light to emit at object location

        /// <summary>
        /// Creates a light source of the given intensity
        /// </summary>
        /// <param name="intensity">Brightness of the light to emit. 10 is a good start.</param>
        public LightSource(float intensity = 10.0f)
        {
            totalLightIntensity = intensity;
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            SpreadLight(transform.position.x, transform.position.y);
            return;
        }

        public override void Render()
        {
            return;
        }

        private void SpreadLight(int x, int y)
        {
            Map map = MapManager.CurrentMap();
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
                if (map == null || player == null)
                {
                    return null;
                }
                GameObject go = map.PeekObject((int)x, (int)y);
                
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
