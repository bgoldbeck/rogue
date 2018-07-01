using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class ConsoleUI
    {
        static private int nCols;
        static private int nRows;
        static private string[] outputBuffer;


        public static void Initialize(int nColumns, int nLines)
        {
            Console.CursorVisible = false;
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
            
            //Commented these out to possibly solve line-skipping problem.
            //Console.SetWindowSize(1, 1);
            //Console.SetBufferSize(100, 100);
            //Console.SetWindowSize(80, 40);

            if (nRows < Console.WindowHeight)
            {
                nRows = Console.BufferHeight;
            }

            if (nCols < Console.WindowWidth)
            {
                nCols = Console.BufferWidth;
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
            //Console.Clear();
            //Console.SetCursorPosition(0, 0);
            //Console.SetWindowPosition(0, 0);
            for (int i = 0; i < nRows; ++i)
            {
                //Console.ForegroundColor = ConsoleColor.Green;

                //Changed WriteLine to Write to possibly solve line-skipping problem.
                Console.Write(outputBuffer[i]);
            }

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            //Console.MoveBufferArea(0, 0, nCols, nRows, 0, 0);
            //Console.SetCursorPosition(0, 0);
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
            //don't do anything if we're off the screen
            if (x < 0 || x >= nCols || y < 0 || y >= nRows)
            {
                return;
            }

            StringBuilder newWrite = new StringBuilder(outputBuffer[y]);

            newWrite.Remove(x, output.Length);
            
            outputBuffer[y] = newWrite.Insert(x, output).ToString();

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
