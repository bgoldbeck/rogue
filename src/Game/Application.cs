﻿#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;

using Ecs;
using Game.Components;
using IO;

namespace Game
{
    public class Application
    {
        private ConsoleKey press = Input.ReadKey().Key;
        private int width;
        private int height;
        private bool isRunning = true;

        public bool Initialize()
        {

            width = Console.WindowWidth;
            height = Console.WindowHeight;
            ConsoleUI.Initialize(width, height);


            GameObject stateManager = GameObject.Instantiate("StateManager");
            stateManager.Name = "StateManager";

            stateManager.AddComponent(new StateManager(width, height));
            
            

            Update();
            Render();
            return true;
        }

        public int Loop()
        {
            double dt = 0.0;
            while (isRunning)
            {
                dt = Time.deltaMs;

                Input.CheckForKeyPress();

                press = Input.ReadKey().Key;

                if (press == ConsoleKey.Escape)
                {
                    isRunning = false;
                }

                CheckForResize();
                Update();

                // Milliseconds per frame in the bottom left corner of screen. 
                // (This is better than FPS), FPS is for noobs.
#if DEBUG
                ConsoleUI.Write(0, 0, "Hit the 'D' key to swap from debug view to game view", Color.Gold);
                ConsoleUI.Write(0, 1, dt.ToString() + " ms/frame", Color.Gold);
                ConsoleUI.Write(0, 2, (1.0 / dt * 1000.0).ToString() + " fps", Color.Gold);
#endif

                Render();


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

