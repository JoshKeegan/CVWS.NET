/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * Quantitative Evaluation
 * Log class - Logs information with different log levels
 * By Josh Keegan 08/03/2014
 * 
 * Note on implementation: LogLevel is checked using bitwise logic, so to log 
 *  events on multiple specific log levels, bitwise OR them together to create
 *  the precise logging criteria desired.
 */

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace QuantitativeEvaluation
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8,
        All = 15
    }

    public static class Log
    {
        //Private variables
        private static string filePath;
        private static LogLevel logLevel;
        private static StreamWriter logWriter;

        //Public Methods
        public static void Initialise(string fileName, LogLevel logLevel)
        {
            Log.filePath = fileName;
            Log.logLevel = logLevel;

            Log.logWriter = new StreamWriter(fileName);
        }

        //Log level writes
        public static void Error(string message)
        {
            if (mayWriteLevel(LogLevel.Error))
            {
                write(message, LogLevel.Error);
            }
        }

        public static void Warn(string message)
        {
            if (mayWriteLevel(LogLevel.Warning))
            {
                write(message, LogLevel.Warning);
            }
        }

        public static void Info(string message)
        {
            if (mayWriteLevel(LogLevel.Info))
            {
                write(message, LogLevel.Info);
            }
        }

        public static void Debug(string message)
        {
            if (mayWriteLevel(LogLevel.Debug))
            {
                write(message, LogLevel.Debug);
            }
        }

        //Private Methods
        private static bool mayWriteLevel(LogLevel logLevel)
        {
            //Use bitwise AND for checking specific rights
            return (Log.logLevel & logLevel) == logLevel;
        }

        private static void write(string message, LogLevel logLevel)
        {
            //Work out where the call to Log.*LogLevel*(str msg) came from and log that too
            StackTrace trace = new StackTrace();
            StackFrame frame = null;
            frame = trace.GetFrame(2);
            string caller = "";

            if (frame != null && frame.GetMethod().DeclaringType != null)
            {
                caller = String.Format("{0}: ", frame.GetMethod().DeclaringType.Name);
            }

            message = String.Format("{0}: {1}", logLevel.ToString(), message);

            String text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " - " + caller + message;

            //Also write the log contents to the console as a live view
            Console.WriteLine(text);

            Log.logWriter.WriteLine(text);
            Log.logWriter.Flush(); //Flush after writes to guard log content against program crash
        }
    }
}
