//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ecs;

namespace Game.Components
{
    class HUD : Component
    {
        private int width;
        private int height;

        public HUD(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            Model model = (Model)gameObject.GetComponent(typeof(Model));
            model.model.Clear();

            AddTop(model);
            AddText(model, "XP: Over 9000!");
            AddText(model, "HUD Width: " + width);
            AddText(model, "HUD Height: " + height);
            AddBottom(model);

            return;
        }

        private void AddTop(Model model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("╔");
            for (int i = 0; i < width - 2; ++i)
                sb.Append("═");
            sb.Append("╗");
            model.model.Add(sb.ToString());
        }

        private void AddText(Model model, String line)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("║ ");
            for (int i = 0; i < line.Length; ++i)
            {
                if (i + 5 > width)
                    break;
                sb.Append(line[i]);
            }
            for (int i = 0; i < width - line.Length - 4; ++i)
                sb.Append(" ");
            sb.Append(" ║");
            model.model.Add(sb.ToString());
        }

        private void AddBottom(Model model)
        {
            int linesToPad = height - model.model.Count - 1;
            for (int i = 0; i < linesToPad; ++i)
                AddText(model, "-- automatic padding --");
            StringBuilder sb = new StringBuilder();
            sb.Append("╚");
            for (int i = 0; i < width - 2; ++i)
                sb.Append("═");
            sb.Append("╝");
            model.model.Add(sb.ToString());
        }

        public override void Render()
        {
            return;
        }
    }
}
