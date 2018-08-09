#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

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
