//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Ecs;

using Game.Components;
using IO;

namespace Game
{
    public class Application
    {
        private bool isRunning = true;
        private ConsoleKey press = Input.ReadKey().Key;
        private int width;
        private int height;
        
        public void Initialize()
        {

            width = Console.WindowWidth;
            height = Console.WindowHeight;
            ConsoleUI.Initialize(width, height);

            LoadScreen();

            GameObject gameManager = GameObject.Instantiate("GameManager");
            gameManager.AddComponent(new GameManager(width, height));

            Update();
            Render();
            return;
        }

        private void LoadScreen()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            FireLogo fl = new FireLogo(width, height);
            fl.Run();
            return;
        }


        public int Loop()
        {
            double dt = 0.0;
            while (isRunning)
            {

                dt = Time.deltaMs;

                // Milliseconds per frame in the bottom left corner of screen. 
                // (This is better than FPS), FPS is for noobs.
                ConsoleUI.Write(0, 1, dt.ToString() + " ms/frame", Color.Gold);
                ConsoleUI.Write(0, 0, (1.0/dt * 1000.0).ToString() + " fps", Color.Gold);


                press = Input.ReadKey().Key;

                if (press == ConsoleKey.Escape)
                { 
                    isRunning = false;
                }

                CheckForResize();
                Update();
                Render();
                GameObject.ForceFlush();


                Input.Reset();
                Time.Update();
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckForResize()
        {
            int newWidth = Console.WindowWidth;
            int newHeight = Console.WindowHeight;

            if (newWidth != width || newHeight != height)
            {
                ConsoleUI.Resize(newWidth, newHeight);
                width = newWidth;
                height = newHeight;

                GameObject.OnResize();
                Render();
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            GameObject.EarlyUpdate();
            GameObject.Update();
            GameObject.LateUpdate();
            return;
        }

        public void Render()
        {
            GameObject.Render();

            ConsoleUI.Render();
            ConsoleUI.ClearBuffer();

            return;
        }
    }
}

