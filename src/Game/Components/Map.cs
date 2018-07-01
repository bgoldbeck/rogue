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
        public override void Start()
        {
            Model mapModel = (Model)this.gameObject.AddComponent(new Model());

            DungeonMaker dm = new DungeonMaker(80, 30, (int)DateTime.Now.Ticks);
            dm.Generate();
            mapModel.model = dm.Stringify();
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
    }
}
