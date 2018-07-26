//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;

using IO;

namespace Game
{
    /// <summary>
    /// Creates a fiery logo screen for ROGUE.
    /// </summary>
    public class FireLogo
    {
        private Random rand = new Random();
        private List<String> logo = new List<String>();
        private float fadeRate = 0.95f; // The fraction of brightness the screen is reduced to every frame
        private float[,] firePoints, fireBuffer;
        private ConsoleKey press = Input.ReadKey().Key;
        private int width;
        private int height;


        /// <summary>
        /// Constructs a firelogo of width and height
        /// </summary>
        /// <param name="width">Width of screen</param>
        /// <param name="height">Height of screen</param>
        public FireLogo(int width, int height)
        {
            this.width = width;
            this.height = height;
            firePoints = new float[width, height];
            fireBuffer = new float[width, height];

            // This is the logo to display centered in the screen. Width of first row
            // determines width of the logo.
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
        }

        /// <summary>
        /// Drawing loop for the logo. Pressing [enter] will break out.
        /// </summary>
        public void Run()
        {
            do
            {
                if (WindowResized())
                    Reset(Console.WindowWidth, Console.WindowHeight);
                SeedBottomPoints();
                SmoothPoints();
                DrawPoints();
                DrawLogo();
                ConsoleUI.Render();
                ShiftPoints();
                FadePoints();
                ConsoleUI.Render();
                press = Input.ReadKey().Key;
            } while (press != ConsoleKey.Enter);
        }

        /// <summary>
        /// Reset the fire logo to a new width and height.
        /// </summary>
        /// <param name="width">Width of screen</param>
        /// <param name="height">Height of screen</param>
        private void Reset(int width, int height)
        {
            this.width = width;
            this.height = height;
            firePoints = new float[width, height];
            fireBuffer = new float[width, height];
            ConsoleUI.Initialize(width, height);
        }

        /// <summary>
        /// Checks if the window has been resized.
        /// </summary>
        /// <returns>Returns true if it has been resized.</returns>
        private bool WindowResized()
        {
            if (this.width != Console.WindowWidth || this.height != Console.WindowHeight)
                return true;
            return false;
        }

        /// <summary>
        /// Seed the bottommost row of points with random value between 0.0 and 1.0;
        /// </summary>
        private void SeedBottomPoints()
        {
            for (int x = 0; x < width; ++x)
            {
                firePoints[x, 0] = (float)rand.NextDouble();
            }
        }

        /// <summary>
        /// Draw the points to the screen using ConsoleUI.
        /// </summary>
        private void DrawPoints()
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    ConsoleUI.Write(x, y, FloatToChar(firePoints[x, y]), new Color(255, 255, 255));
                }
            }
        }

        /// <summary>
        /// Averages points with their horizontal neighbors. This gives the smoothing effect.
        /// </summary>
        private void SmoothPoints()
        {
            for (int x = 1; x < width - 1; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    if (x == 0)
                        fireBuffer[x, y] = 0.0f;
                    else if (x == width - 1)
                        fireBuffer[x, y] = 0.0f;
                    else
                        fireBuffer[x, y] = (firePoints[x - 1, y] + firePoints[x, y] + firePoints[x + 1, y]) / 3.0f;
                }
            }
            SwapBuffer();
        }

        /// <summary>
        /// Copy the temporary buffer of points into the main array.
        /// </summary>
        private void SwapBuffer()
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    firePoints[x, y] = fireBuffer[x, y];
                }
            }
        }

        /// <summary>
        /// Shifts each row of points up by one row.
        /// </summary>
        private void ShiftPoints()
        {
            for (int y = height - 1; y >= 1; --y)
            {
                for (int x = 0; x < width; ++x)
                {
                    firePoints[x, y] = firePoints[x, y - 1];
                }
            }
        }

        /// <summary>
        /// Decreases the value stored in the point array to a percent determined by fadeRate.
        /// </summary>
        private void FadePoints()
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    firePoints[x, y] = firePoints[x, y] * fadeRate;
                }
            }
        }

        /// <summary>
        /// Converts a value between 0 and 1 into a character.
        /// </summary>
        /// <param name="input">Float value to convert.</param>
        /// <returns>Returns a character to represent a float value between 0 and 1</returns>
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

        /// <summary>
        /// Draws the logo centered vertically and horizontally.
        /// </summary>
        private void DrawLogo()
        {
            int halfLogoWidth = logo[0].Length / 2;
            int halfLogoHeight = logo.Count / 2;
            ConsoleUI.Write(width / 2 - halfLogoWidth, height / 2 + halfLogoHeight, logo, Color.Salmon);
        }
    }
}
