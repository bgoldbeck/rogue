using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class TextUI
    {
        static private int nCols;
        static private int nRows;
        static private string[] outputBuffer;


        public static void Initialize(int nColumns, int nLines)
        {
            Resize(nColumns, nLines);
            return;
        }
        
        /// <summary>
        /// Clear the contents of the buffer string table. Reset the 
        /// screen contents with default draw box.
        /// </summary>
        public static void ClearBuffer()
        {
            for (int i = 0; i < nRows; ++i)
            {
                outputBuffer[i] = "".PadRight(nCols, ' ');
            }
            return;
        }

        public static void Resize(int nColumns, int nLines)
        {
            nCols = nColumns;
            nRows = nLines;

            Console.TreatControlCAsInput = false;
            
            Console.SetWindowSize(
                Console.LargestWindowWidth / 2, 
                Console.LargestWindowHeight - 20);

            Console.SetWindowPosition(0, 0);

            if (nRows < Console.WindowHeight)
            {
                nRows = Console.WindowHeight;
                Console.BufferHeight = nRows;
            }

            if (nCols < Console.WindowWidth)
            {
                nCols = Console.WindowWidth;
                Console.BufferWidth = nCols;
            }

            outputBuffer = new string[nRows];

            ClearBuffer();

            return;
        }

        /// <summary>
        /// Draw everything from the buffer to the console.
        /// </summary>
        public static void Render()
        {
            Console.Clear();
            for (int i = 0; i < nRows; ++i)
            {
                //Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(outputBuffer[i]);
            }
            //Console.SetWindowPosition(0, 0);

            // Clear the contents in the buffer.
            ClearBuffer();

            return;
        }

        /// <summary>
        /// Write output text to the current cursor index position in the buffer.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static void Write(int x, int y, string output)
        {
            StringBuilder newWrite = new StringBuilder(outputBuffer[x]);

            newWrite.Remove(y, output.Length);
            
            outputBuffer[x] = newWrite.Insert(y, output).ToString();

            return;
        }

        public static int MaxColumns()
        {
            return nCols;
        }

        public static int MaxRows()
        {
            return nRows;
        }
    }
}
