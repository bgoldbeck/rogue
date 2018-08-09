#region copyright
// Copyright (C) 2018 "Daniel Bramblett" <bram4@pdx.edu>, "Daniel Dupriest" <kououken@gmail.com>, "Brandon Goldbeck" <bpg@pdx.edu>
// This software is licensed under the MIT License. See LICENSE file for the full text.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace IO
{
    public class ConsoleUI
    {
        private const String defaultColor = "\u001b[38;2;128;128;128m";
        static private int width;
        static private int height;
        static private List<List<char>> buffer;
        private static List<List<String>> colorBuffer;
        private static bool isANSISupported = true;
        static private bool colorEnabled = true;

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static void Initialize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            Console.CursorVisible = false;

            buffer = new List<List<char>>();
            colorBuffer = new List<List<String>>();

            for (int x = 0; x < width; ++x)
            {
                List<char> bufferRow = new List<char>();
                List<String> colorBufferRow = new List<String>();

                for (int y = 0; y < height; ++y)
                {
                    bufferRow.Add(' ');
                    // Add some default color
                    colorBufferRow.Add(defaultColor); 
                }
                buffer.Add(bufferRow);
                colorBuffer.Add(colorBufferRow);
            }

            bool isWindows = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows);
            if (isWindows)
            {
                // This is code to enable ANSI Character support on windows taken from
                // https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                {
                    Console.WriteLine("WARNING: Failed to get output console mode");
                    isANSISupported = false;
                    Console.ReadKey();
                    return;
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(iStdOut, outConsoleMode))
                {
                    Console.WriteLine($"WARNING: Failed to set output console mode, error code: {GetLastError()}");
                    isANSISupported = false;
                    Console.ReadKey();
                    return;
                }
            }

            return;
        }
        
        /// <summary>
        /// Clear the contents of the buffer string table. Reset the 
        /// screen contents with default draw box.
        /// </summary>
        public static void ClearBuffer()
        {
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    buffer[x][y] = ' ';
                    colorBuffer[x][y] = defaultColor;
                }
            }

            return;
        }
        
        /// <summary>
        /// Draw everything from the buffer to the console.
        /// </summary>
        public static void Render()
        {
            Console.CursorVisible = false;

            // If color is turned on and supported, draw with color codes
            if (colorEnabled && isANSISupported)
            {
                for (int y = height - 1; y >= 0; --y)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int x = 0; x < width; ++x)
                    {
                        if (!buffer[x][y].Equals(' '))
                        {
                            sb.Append(colorBuffer[x][y]);
                        }
                        sb.Append(buffer[x][y]);
                    }
                    Console.Write(sb.ToString());
                }
                Console.SetCursorPosition(0, 0);
            }
            else  //Otherwise draw plain
            {
                for (int y = height - 1; y >= 0; --y)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int x = 0; x < width; ++x)
                    {
                        sb.Append(buffer[x][y]);
                    }
                    Console.Write(sb.ToString());
                }
                Console.SetCursorPosition(0, 0);
            }
        }

        public static void Resize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;

            buffer = new List<List<char>>();
            colorBuffer = new List<List<String>>();

            for (int x = 0; x < width; ++x)
            {
                List<char> bufferRow = new List<char>();
                List<String> colorBufferRow = new List<String>();

                for (int y = 0; y < height; ++y)
                {
                    bufferRow.Add(' ');
                    // Add some default color
                    colorBufferRow.Add(defaultColor);
                }
                buffer.Add(bufferRow);
                colorBuffer.Add(colorBufferRow);
            }
        }

        public static void ToggleColor()
        {
            if (colorEnabled)
            {
                colorEnabled = false;
                Console.Write(defaultColor);
            }
            else
                colorEnabled = true;
        }

        public static void Write(int x, int y, char output, Color color)
        {
            //don't do anything if we're off the screen
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }
    
            buffer[x][y] = output;
            colorBuffer[x][y] = color.ToCode();
            return;
        }

        public static void Write(int x, int y, String output, Color color)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                Write(x + i, y, output[i], color);
            }
        }
        
        public static void Write(int x, int y, List<String> lines, Color color)
        {
            for (int i = 0; i < lines.Count; ++i)
            {
                    Write(x, y - i, lines[i], color);
            }
        }
        

        public static int MaxWidth()
        {
            return width;
        }

        public static int MaxHeight()
        {
            return height;
        }
    }
}
