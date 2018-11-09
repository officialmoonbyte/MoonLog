using System;
using System.IO;

namespace Moonbyte.Logging
{
    public class Moonlog
    {

        #region Vars

        private static string Log;
        public static string GetLog { get { return Log; } }

        #endregion

        #region General Logging Functions

        /// <summary>
        /// Used to add a value to the log string.
        /// </summary>
        public static void AddToLog(string Header, string Value)
        {
            string value = "[" + DateTime.Now.ToString("HH:mm") + "] " + "[" + Header.ToUpper() + "] " + Value;

            //Check if Log is null, if it is not then makes a new line.
            if (Log != null) Log = Log + "\r\n" + value;

            //Cehck if log is null, if it is then set log to value
            if (Log == null) Log = value;

            //Prints value in console
            Console.WriteLine(value);
        }

        /// <summary>
        /// Adds a white space to the command list
        /// </summary>
        public static void AddWhitespace()
        {
            //Add the white space
            if (Log != null) Log += "\r\n";

            //Prints in console
            Console.WriteLine(" ");
        }

        #endregion

        #region Saving logs

        /// <summary>
        /// Used to write to the log file
        /// </summary>
        public static void WriteLog()
        {

            //Get the execution directory
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //Check if the Log is null
            if (Log != null)
            {
                //Creates the log directory
                if (!Directory.Exists(exeDirectory + @"\Logs")) Directory.CreateDirectory(exeDirectory + @"\Logs");

                //Delete the log file if it exist.
                if (File.Exists(exeDirectory + @"\Logs\Log.log")) File.Delete(exeDirectory + @"\Logs\Log.log");

                //Creates the log file, and then close the file stream.
                File.Create(exeDirectory + @"\Logs\Log.log").Close();

                //Write to the log file.
                File.WriteAllText(exeDirectory + @"\Logs\Log.log", Log);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Set the logging events for the server
        /// </summary>
        public static void SetLoggingEvents()
        {
            AppDomain.CurrentDomain.UnhandledException += ((obj, args) =>
            {
                UnhandledExceptionEventArgs e = args;

                AddToLog("Moonlog/ERROR", "Error with App Domain");

                Exception ex = (Exception)e.ExceptionObject;

                AddToLog("Moonlog/ERROR", "Message : " + ex.Message);
                AddToLog("Moonlog/ERROR", "StackTrace : " + ex.StackTrace);
                AddToLog("Moonlog/ERROR", "Source : " + ex.Source);

                WriteLog();
            });
            AppDomain.CurrentDomain.ProcessExit += ((obj, args) =>
            {
                WriteLog();
            });
        }

        #endregion
    }
}
