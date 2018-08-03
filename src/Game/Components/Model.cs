//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;

using Ecs;
using IO;

namespace Game.Components
{
    class Model : Component
    {
        public List<String> model = new List<String>();
        public Color color = new Color(255, 255, 255);

        public override void Start()
        {
            model = new List<string>();
            return;
        }

        public override void Update()
        {
            return;
        }

        public override void Render()
        {
            int x = this.gameObject.Transform.position.x;
            int y = this.gameObject.Transform.position.y;

            ConsoleUI.Write(x, y, model, color);
            return;
        }
    }
}
