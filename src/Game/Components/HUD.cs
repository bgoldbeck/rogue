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
        private List<String> log;

        public HUD(int width, int height)
        {
            this.width = width;
            this.height = height;
            log = new List<String>();
        }

        public void Log(String line)
        {
            log.Add(line);
        }

        public override void Start()
        {
            return;
        }

        public override void Update()
        {
            Model model = (Model)gameObject.GetComponent(typeof(Model));
            model.model.Clear();

            Actor player = (Actor)GameObject.FindWithTag("Player").GetComponent(typeof(Actor));

            AddTop(model);
            AddText(model, player.Name);
            AddText(model, "Level: " + player.Level);
            AddText(model, "Attack: " + player.Attack);
            AddText(model, "Armor: " + player.Armor);
            AddDivider(model);
            AddText(model, "Inventory maybe ?");
            AddDivider(model);
            AddLog(model);
            AddBottom(model);

            return;
        }

        private void AddLog(Model model)
        {
            log.Reverse();
            int linesToFill = height - model.model.Count - 1;
            List<List<String>> logLines = new List<List<String>>();
            for (int i = 0; i < linesToFill; ++i)
            {
                if (i > log.Count - 1)
                    break;
                logLines.Add(WordWrap(log[i]));
            }

            int count = 0;
            for (int i = 0; i < logLines.Count; ++i)
            {
                for (int j = 0; j < logLines[i].Count; ++j)
                {
                    if (count < linesToFill)
                    {
                        AddText(model, logLines[i][j]);
                        ++count;
                    }
                }
            }
            log.Reverse();
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

        private void AddDivider(Model model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("╠");            
            for (int i = 0; i < width - 2; ++i)
                sb.Append("═");
            sb.Append("╣");
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

        /// <summary>
        /// Code found in this thread: https://social.msdn.microsoft.com/Forums/en-US/e549e7a7-bcd9-4f18-b797-4590180855c2/wrap-the-text-with-fixed-size-length-of-30-using-c?forum=csharpgeneral
        /// </summary>
        List<String> WordWrap(String line)
        {
            List<String> sentence = new List<String>();
            int index = 0;
            String result = "";
            foreach (char c in line)
            {
                //if smaller then 30 add to result
                if (index <= width - 4)
                {
                    //increase char index
                    index++;
                    result += c;
                }
                if (index == width - 4)
                {
                    //if index hits the first 30 chars add to list and clear result and index
                    sentence.Add(result);
                    result = "";
                    index = 0;
                }
            }
            //add the last remaing characters 
            sentence.Add(result);
            return sentence;
        }

        public override void Render()
        {
            return;
        }
    }
}
