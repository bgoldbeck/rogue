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
    class HUD : Component
    {
        private int width;
        private int height;
        private List<String> log;
        private static HUD hud = null;
        private Enemy target = null;

        public HUD(int width, int height)
        {
            this.width = width;
            this.height = height;
            log = new List<String>();
        }

        public static HUD CacheInstance()
        {
            return hud;
        }

        public static HUD Append(String line)
        {
            hud = CacheInstance();
            if (hud != null)
            {
                hud.Log(line);
            }
            return hud;
        }

        public static HUD Clear()
        {
            hud = CacheInstance();
            if (hud != null)
            {
                // TODO: Clear the contents of the HUD.
                // Might be good for when we do map changes.
                throw new NotImplementedException();
            }
            return hud;
        }

        public void Log(String line)
        {
            List<String> wrapped = WordWrap(line);
            foreach (String str in wrapped)
                log.Add(str);
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
            Update();
            return;
        }

        public void Target(Enemy enemy)
        {
            hud.target = enemy;
        }

        public override void Start()
        {
            if (hud != null && hud != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                hud = this;
            }
            return;
        }

        public override void Update()
        {
            Model model = (Model)gameObject.GetComponent(typeof(Model));
            model.model.Clear();

            Actor player = Player.MainPlayer();

            AddTop(model);
            AddText(model, player.Name);
            AddText(model, "  Level: " + player.Level + " (" + player.Xp + " xp)");
            AddText(model, "  Health: " + player.HitPoints);
            AddText(model, "  Attack: " + player.Attack);
            AddText(model, "  Armor: " + player.Armor);
            AddDivider(model);

            // Display inventory
            AddText(model, "Inventory:");
            Inventory inv = (Inventory)player.GetComponent<Inventory>();
            foreach (Item it in inv.Bag)
            {
                AddText(model, "  " + it.name);
            }

            // Show the last attacked enemy info
            AddDivider(model);
            AddTarget(model);

            AddDivider(model);

            // Display log. Must be the last item in the HUD for length to calculate correctly.
            AddLog(model); 

            AddBottom(model);

            return;
        }

        /// <summary>
        /// Function to grab enough recent log lines to fill the space
        /// </summary>
        /// <param name="model">Model to add to</param>
        private void AddLog(Model model)
        {
            int linesToFill = height - model.model.Count - 1;
            for (int i = log.Count - linesToFill; i < log.Count; ++i)
            {
                if (i < 0)
                    continue;
                AddText(model, log[i]);
            }
        }

        private void AddTarget(Model model)
        {
            if (target == null)
            {
                AddText(model, "Target:");
                return;
            }

            AddText(model, "Target: " + target.Name);
      
            int displayMaxWidth = width - 6;
            int currentHp = target.HitPoints;
            int maxHp = target.MaxHitPoints;
            int displayValue = currentHp;
            int displayWidth = maxHp;

            // Scale the hp to HUD width if necessary
            if (maxHp > displayMaxWidth)
            {
                displayValue = currentHp / maxHp * displayMaxWidth;
                displayWidth = displayMaxWidth;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for(int i = 0; i < displayWidth; ++i)
            {
                if (i < displayValue)
                    sb.Append("*");
                else
                    sb.Append(" ");
            }
            sb.Append("]");
            AddText(model, sb.ToString());
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
                AddText(model, "");
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
