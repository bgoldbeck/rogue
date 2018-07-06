//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class ConsoleUI
    {
        static private int width;
        static private int height;
        static private List<List<char>> buffer;
        
        public static void Initialize(int newWidth, int newHeight)
        {
            width = newWidth;
            height = newHeight;
            Console.CursorVisible = false;

            buffer = new List<List<char>>();
            for (int x = 0; x < width; ++x)
            {
                List<char> bufferRow = new List<char>();
                for (int y = 0; y < height; ++y)
                {
                    bufferRow.Add(' ');
                }
                buffer.Add(bufferRow);
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
                    sb.Append(buffer[x][y]);
                }
                Console.Write(sb.ToString());
            }
            Console.SetCursorPosition(0, 0);

            return;
        }

        public static void Write(int x, int y, char output)
        {
            //don't do anything if we're off the screen
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }
    
            buffer[x][y] = output;
            return;
        }

        public static void Write(int x, int y, String output)
        {
            for (int i = 0; i < output.Length; ++i)
            {
                Write(x + i, y, output[i]);
            }
        }

        public static void Write(int x, int y, List<String> lines)
        {
            for (int i = 0; i < lines.Count; ++i)
            {
                Write(x, y - i, lines[i]);
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
