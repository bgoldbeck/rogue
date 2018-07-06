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
    class Model : Component
    {
        public List<String> model = new List<String>();
        public List<List<String>> colorModel = new List<List<String>>();

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
            int x = this.gameObject.transform.position.x;
            int y = this.gameObject.transform.position.y;

            /*
            for (int i = 0; i < model.Count; ++i)
            { 
                for (int j = 0; j < model[i].Length; ++j)
                { 
                    ConsoleUI.Write(x++, y, model[i][j], colorModel[i][j]);
                }
                --y;
            }
            */
            ConsoleUI.Write(x, y, model, colorModel);
            return;
        }
    }
}
