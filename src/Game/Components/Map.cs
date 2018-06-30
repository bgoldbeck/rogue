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

            // Test model.
            mapModel.model.Add("####################################################################################");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("#                                                                                  #");
            mapModel.model.Add("####################################################################################");
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
