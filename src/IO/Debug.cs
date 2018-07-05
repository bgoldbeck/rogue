using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    class Debug
    {
        private static StreamWriter writer = null;

        public static void Log(string log)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToString(), "DEBUG:", log));
            return;
        }
        public static void LogWarning(string warning)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToFileTime(), "Warning:", warning));
            return;
        }

        public static void LogError(string error)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToFileTime(), "ERROR:", error));
            return;
        }

        private static void WriteToLog(string message)
        {
            if (writer == null)
            {
                CreateLogFile();
            }

            if (writer != null)
            {
                writer.WriteLine(message);
            }
            return;
        }

        private static void CreateLogFile()
        {
            writer = new StreamWriter(File.Open("Log.txt", System.IO.FileMode.Create));
            if (writer != null)
            {
                writer.AutoFlush = true;
            }
            return;
        }
    }
}
