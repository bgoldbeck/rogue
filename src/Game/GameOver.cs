#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using IO;

namespace Game
{
    class Raindrop
    {
        private static float xVelocity = -.6f;
        private static float yVelocity = -.8f;
        private static Random rand = new Random();
        public float x;
        public float y;
        public float z;

        public Raindrop(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Step(int width, int height)
        {
            this.x += xVelocity;
            this.y += yVelocity;
            if (y <= 0)
            {
                y = height;
                x = (float) (rand.NextDouble() * width * 1.5f - (width * 0.25f) );
                z = (float) rand.NextDouble();
            }
        }

        public override String ToString()
        {
            if (z < .3)
                return "/";
            else if (z < .6)
                return ",";
            else
                return ".";
        }
    }

    class GameOver
    {
        private static float tombstoneTopPosition = .4f;    //fraction from top of screen
        private static int tombstoneRIPTextWidth = 14;
        private static float grassHeight = 20.0f;
        private static int numberOfDrops = 100;
        private static float chanceOfLightning = .005f; //Chance of lightning
        private static int lightningFrames = 2;

        private List<String> ripText = new List<string>();
        private List<String> grass = new List<String>();
        private List<Raindrop> rain = new List<Raindrop>();
        private int width;
        private int height;
        private String playerName;
        private ConsoleKey press = Input.ReadKey().Key;
        private Random rand = new Random();
        private int currentLightningFrame = 0;

        private void DrawTombstone(int topLeftX, int topLeftY, int tombstoneWidth)
        {
            // Blank area
            for (int x = 0; x < tombstoneWidth; ++x)
            {
                for (int y = 0; y < 10; ++y)
                {
                    ConsoleUI.Write(topLeftX + x, topLeftY - y, " ", Color.White);
                }
            }
            
            // Draw outline
            for (int x = 1; x < tombstoneWidth; ++x)
            {
                ConsoleUI.Write(topLeftX + x, topLeftY, "_", DuringLightning() ? Color.White : Color.Gray);
            }
            ConsoleUI.Write(topLeftX, topLeftY - 1, "/", DuringLightning() ? Color.White : Color.Gray);
            ConsoleUI.Write(topLeftX + tombstoneWidth, topLeftY - 1, @"\", DuringLightning() ? Color.White : Color.Gray);
            for (int y = 2; y < 10; ++y)
            {
                ConsoleUI.Write(topLeftX, topLeftY - y, "│", DuringLightning() ? Color.White : Color.Gray);
                ConsoleUI.Write(topLeftX + tombstoneWidth, topLeftY - y, "│", DuringLightning() ? Color.White : Color.Gray);
            }

            // Draw RIP
            int positionX = width / 2 - tombstoneRIPTextWidth / 2;
            ConsoleUI.Write(positionX, topLeftY - 2, ripText, DuringLightning() ? Color.White : Color.Gray);

            // Draw name
            ConsoleUI.Write(topLeftX + 2, topLeftY - 8, playerName, DuringLightning() ? Color.White : Color.Gray);
        }

        public void DrawGrass(int startingY)
        {
            ConsoleUI.Write(0, startingY, grass, DuringLightning() ? new Color(0, 255, 0) : new Color(15, 75, 15));
        }

        public GameOver(String playerName)
        {
            ConsoleUI.Initialize(Console.WindowWidth, Console.WindowHeight);
            this.width = Console.WindowWidth;
            this.height = Console.WindowHeight;
            this.playerName = playerName;

            // Set up RIP text
            ripText.Add(@"__   ___  __  ");
            ripText.Add(@"│ |   │   │ | ");
            ripText.Add(@"│/    │   │/  ");
            ripText.Add(@"│\    │   │   ");
            ripText.Add(@"│ \. _│_. │  .");

            // Set up grass
            Random rand = new Random();
            for (int y = 0; y < grassHeight; ++y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < width; ++x)
                {
                    float chance = y / grassHeight;
                    float r = (float)rand.NextDouble();
                    if (rand.NextDouble() < chance)
                    {
                        if (rand.Next() % 2 == 0)
                            sb.Append(")");
                        else
                            sb.Append("(");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                grass.Add(sb.ToString());
            }

            // Initialize rain
            for (int i = 0; i < numberOfDrops; ++i)
            {
                float x = (float)(rand.NextDouble() * width * 1.5 - (width * .25) );
                float y = (float)(rand.NextDouble() * height);
                float z = (float)rand.NextDouble();
                rain.Add(new Raindrop(x, y, z));
            }
        }

        private void DrawRain()
        {
            foreach (Raindrop drop in rain)
            {
                ConsoleUI.Write((int)drop.x, (int)drop.y, drop.ToString(), DuringLightning() ? Color.White : Color.Gray);
                drop.Step(width, height);
            }
        }

        private bool DuringLightning()
        {
            return currentLightningFrame > 0;
        }

        private void DrawLightning()
        {
            float chance = (float)rand.NextDouble();
            if (chance < chanceOfLightning)
            {
                currentLightningFrame = lightningFrames;
            }
            else if (currentLightningFrame > 0)
            {
                currentLightningFrame -= 1;
            }
        }

        public void Run()
        {
            do
            {
                Input.Reset();
                if (WindowResized())
                { 
                    Reset(Console.WindowWidth, Console.WindowHeight);
                }

                // Set up size and position of tombstone
                int tombstoneTopLeftY = (int)(height - tombstoneTopPosition * height);
                int tombstoneWidth = Math.Max(playerName.Length, tombstoneRIPTextWidth) + 4;
                int tombstoneTopLeftX = width / 2 - tombstoneWidth / 2;

                ConsoleUI.ClearBuffer();
                DrawGrass(tombstoneTopLeftY);
                DrawTombstone(tombstoneTopLeftX, tombstoneTopLeftY, tombstoneWidth);
                DrawRain();
                DrawLightning();

                ConsoleUI.Render();
                Input.CheckForKeyPress();
                press = Input.ReadKey().Key;
            } while (press != ConsoleKey.Enter);
        }

        private bool WindowResized()
        {
            if (this.width != Console.WindowWidth || this.height != Console.WindowHeight)
                return true;
            return false;
        }

        private void Reset(int width, int height)
        {
            this.width = width;
            this.height = height;
            ConsoleUI.Initialize(width, height);

            // Set up grass
            grass.Clear();
            Random rand = new Random();
            for (int y = 0; y < grassHeight; ++y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < width; ++x)
                {
                    float chance = y / grassHeight;
                    float r = (float)rand.NextDouble();
                    if (rand.NextDouble() < chance)
                    {
                        if (rand.Next() % 2 == 0)
                            sb.Append(")");
                        else
                            sb.Append("(");
                    }
                    else
                    {
                        sb.Append(" ");
                    }
                }
                grass.Add(sb.ToString());
            }

            // Initialize rain
            for (int i = 0; i < numberOfDrops; ++i)
            {
                float x = (float)(rand.NextDouble() * width * 1.5 - (width * .25));
                float y = (float)(rand.NextDouble() * height);
                float z = (float)rand.NextDouble();
                rain.Add(new Raindrop(x, y, z));
            }
        }
    }
}
