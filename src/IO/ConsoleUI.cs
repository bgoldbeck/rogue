//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IO
{
    public class ConsoleUI
    {
        private const string defaultColor = "\u001b[37m";
        static private int width;
        static private int height;
        static private List<List<char>> buffer;
        private static List<List<String>> colorBuffer;

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
   

            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
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
            
            for (int y = height - 1; y >= 0; --y)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < width; ++x)
                {
                    sb.Append(string.Format("{0}{1}{2}", colorBuffer[x][y], buffer[x][y].ToString(), "\u001b[0m"));   
                }

                Console.Write(sb.ToString());
            }
            Console.SetCursorPosition(0, 0);

            return;
        }

        public static void Write(int x, int y, char output, String color = defaultColor)
        {
            //don't do anything if we're off the screen
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }
    
            buffer[x][y] = output;
            colorBuffer[x][y] = color;
            return;
        }

        public static void Write(int x, int y, String output, List<String> colors = null)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                String color = colors != null && colors.Count == output.Length ? colors[i] : defaultColor;
                Write(x + i, y, output[i], color);
            }
        }
        
        public static void Write(int x, int y, List<String> lines, List<List<String>> colors = null)
        {
            for (int i = 0; i < lines.Count; ++i)
            {
                if (colors == null || colors.Count != lines.Count)
                { 
                    Write(x, y - i, lines[i]);
                }
                else
                {
                    Write(x, y - i, lines[i], colors[i]);
                }

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
