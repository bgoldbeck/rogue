//Copyright(c) 2018 Daniel Bramblett, Daniel Dupriest, Brandon Goldbeck

using System;
using System.IO;

namespace Ecs
{
    /// <summary>
    /// Writes debug information to a log file.
    /// </summary>
    class Debug
    {
        private static StreamWriter writer = null;

        /// <summary>
        /// Write to log using normal debug flag.
        /// </summary>
        /// <param name="log">The log message to write.</param>
        public static void Log(string log)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToString(), "DEBUG:", log));
            return;
        }

        /// <summary>
        /// Write to log using the warning flag.
        /// </summary>
        /// <param name="log">The log message to write.</param>
        public static void LogWarning(string warning)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToString(), "Warning:", warning));
            return;
        }

        /// <summary>
        /// Write to log using the error flag.
        /// </summary>
        /// <param name="log">The log message to write.</param>
        public static void LogError(string error)
        {
            WriteToLog(string.Format("{0} {1} {2}", System.DateTime.Now.ToString(), "ERROR:", error));
            return;
        }

        /// <summary>
        /// Write the message to the log file.
        /// </summary>
        /// <param name="message"></param>
        [System.Diagnostics.Conditional("DEBUG")]
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

        /// <summary>
        /// Create the log file.
        /// </summary>
        private static void CreateLogFile()
        {
            File.WriteAllText("Log.txt", String.Empty);
            writer = new StreamWriter(File.Open("Log.txt", System.IO.FileMode.Create));
            if (writer != null)
            {
                writer.AutoFlush = true;
            }
            return;
        }
    }
}
