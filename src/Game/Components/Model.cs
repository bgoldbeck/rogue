using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;
using ConsoleUI;

namespace Game.Components
{
    class Model : Component
    {
        public List<string> model = new List<string>();

        public override void Start()
        {
            // Test model.
            model.Add("####################################");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("#                                  #");
            model.Add("####################################");
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            int x = this.gameObject.transform.position.x;
            int y = this.gameObject.transform.position.y;

            foreach (string str in model)
            {
                TextUI.Write(x++, y, str);
            }
            return;
        }
    }
}
