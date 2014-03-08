/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Program Entry Point
 * By Josh Keegan 08/03/2013
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QuantitativeEvaluation
{
    class Program
    {
        //Constants
        private const string LOGS_DIR_PATH = "logs";
        private const LogLevel LOG_LEVEL = LogLevel.All;

        static void Main(string[] args)
        {
            //Initialise logging
            bool dirCreated = false;
            if (!Directory.Exists(LOGS_DIR_PATH))
            {
                dirCreated = true;
                System.IO.Directory.CreateDirectory(LOGS_DIR_PATH);
            }

            int logAttempt = 0;
            while(true)
            {
                string logName = String.Format("{0}/Quantitative_Evaluation_Log.{1}.{2:000}.log", LOGS_DIR_PATH, DateTime.Now.ToString("yyyy-MM-dd"), logAttempt);
                if (!System.IO.File.Exists(logName))
                {
                    Log.Initialise(logName, LOG_LEVEL);
                    Log.Info("Log Initialised");
                    if (dirCreated)
                    {
                        Log.Warn("Logs Directory was not found, creating . . .");
                    }
                    break;
                }
                logAttempt++;
            }


        }
    }
}
