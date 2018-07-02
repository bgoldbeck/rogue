using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

namespace Game.Components
{
    class Map : Component
    {
        private int width;
        private int height;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Regen(int width, int height)
        {
            this.width = width;
            this.height = height;
            SetupMap();
        }

        public override void Start()
        {
            Model mapModel = (Model)this.gameObject.AddComponent(new Model());
            SetupMap();
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            return;
        }

        private void SetupMap()
        {
            Model mapModel = (Model)gameObject.GetComponent<Model>();

            DungeonMaker dm = new DungeonMaker(this.width, this.height, (int)DateTime.Now.Ticks);
            dm.Generate();
            mapModel.model = dm.Stringify();

        }
    }
}
