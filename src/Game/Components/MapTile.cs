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
    class MapTile : Component
    {
        public char character = '?';
        public Color color = new Color(255, 255, 255);
        public float lightLevel = 0.0f;
        private static float lightLevelAfterDiscovery = .3f;
        private static float amountToDimEachStep = .1f;

        public MapTile()
        {
        }

        public MapTile(char character, Color color, float lightLevel = 0.0f)
        {
            this.character = character;
            this.color = color;
            this.lightLevel = lightLevel;
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            if (lightLevel > lightLevelAfterDiscovery)
            {
                lightLevel -= amountToDimEachStep;
            }
            return;
        }

        public override void Render()
        {
            return;
        }
    }
}
