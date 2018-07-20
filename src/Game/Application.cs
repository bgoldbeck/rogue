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
        private Random rand = new Random();
        private List<String> logo = new List<String>();
        
        public void Initialize()
        {

            width = Console.WindowWidth;
            height = Console.WindowHeight;
            ConsoleUI.Initialize(width, height);

            Time.Initialize();

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
            float[,] firePoints = new float[width, height];
            logo.Add(@" ___________________________________________ ");
            logo.Add(@"/    ___      __      __             ___    \");
            logo.Add(@"│    │  │    │  │    │  │    │  │    │      │");
            logo.Add(@"│    │ /     │  │    │  │    │  │    │      │");
            logo.Add(@"│    │/      │  │    │ __    │  │    │__    │");
            logo.Add(@"│    │\      │  │    │  │    │  │    │      │");
            logo.Add(@"│    │ \     │  │    │  │    │  │    │      │");
            logo.Add(@"│    │  \ .  │__│.   │__│.   │__│.   │__.   │");
            logo.Add(@"|                                           |");
            logo.Add(@"| Real-time Open-source Game Using Entities |");
            logo.Add(@"`──────────────────────────────────────────' ");
            logo.Add(@"Press [enter] to begin. ───────────── v 0.1  ");


            do
            {
                SeedBottomPoints(firePoints);
                ConsoleUI.Render();
                float [,] next = SmoothPoints(firePoints);
                DrawPoints(next);
                DrawLogo();
                ShiftPoints(next);
                FadePoints(next);
                firePoints = next;
                ConsoleUI.Render();
                press = Input.ReadKey().Key;
            } while (press != ConsoleKey.Enter);

            return;
        }

        private void SeedBottomPoints(float[,] points)
        {
            for (int x = 0; x < width; ++x)
            {
                points[x, 0] = (float)rand.NextDouble();
            }
        }

        private void DrawPoints(float[,] firePoints)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ConsoleUI.Write(x, y, FloatToChar(firePoints[x, y]), new Color(255,255,255));
                }
            }
        }

        private float[,] SmoothPoints(float [,] firePoints)
        {
            float[,] nextPoints = new float[width, height];
            for (int x = 1; x < width-1; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (x == 0)
                        nextPoints[x, y] = 0.0f;
                    else if (x == width-1)
                        nextPoints[x, y] = 0.0f;
                    else
                        nextPoints[x, y] = ( firePoints[x - 1,y] + firePoints[x, y] + firePoints[x + 1, y] ) / 3.0f;
                }
            }
            return nextPoints;
        }

        private void ShiftPoints(float [,] firePoints)
        {
            for (int y = height-1; y >= 1; --y)
            {
                for (int x = 0; x < width; ++x)
                {
                    firePoints[x, y] = firePoints[x, y - 1];
                }
            }
        }

        private void FadePoints(float[,] points)
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    points[x, y] = points[x, y] * 0.95f;
                }
            }
        }

        private char FloatToChar(float input)
        {
            if (input > .8)
                return '#';
            else if (input > .6)
                return '+';
            else if (input > .4)
                return ':';
            else if (input > .2)
                return '.';
            else return ' ';
        }

        private void DrawLogo()
        {
            int halfLogoWidth = logo[0].Length / 2;
            int halfLogoHeight = logo.Count() / 2;
            ConsoleUI.Write(width / 2 - halfLogoWidth, height / 2 + halfLogoHeight, logo, Color.Salmon);
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

