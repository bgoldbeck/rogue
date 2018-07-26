//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using Ecs;
using IO;

namespace Game.Components
{
    class MapTile : Component
    {
        public char character = '?';
        public Color color = new Color(255, 255, 255);
        public float lightLevel = 0.0f;
        private float lightLevelAfterDiscovery = .4f;
        private static float amountToDimEachStep = .1f;
        public bool disableLighting = false;

        public MapTile()
        {
        }

        public MapTile(char character, Color color, bool disableLighting = false)
        {
            this.character = character;
            this.color = color;
            this.disableLighting = disableLighting;
        }

        public void SetLightLevelAfterDiscovery(float value)
        {
            if (value < 0f)
                lightLevelAfterDiscovery = 0f;
            else if (value > 1.0f)
                lightLevelAfterDiscovery = 1.0f;
            else
                lightLevelAfterDiscovery = value;
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
