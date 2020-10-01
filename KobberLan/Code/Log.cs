using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KobberLan.Code
{
    //-------------------------------------------------------------
    // Singleton pattern
    public class Log
    //-------------------------------------------------------------
    {
        private static Log log;

        private KobberLan kobberLan;
        private int errors;
        private int warnings;

        //-------------------------------------------------------------
        public enum LogType
        //-------------------------------------------------------------
        {
            Info,
            Warning,
            Error
        }

        //-------------------------------------------------------------
        private Log()
        //-------------------------------------------------------------
        {
            errors = 0;
            warnings = 0;
        }

        //-------------------------------------------------------------
        public static Log Get()
        //-------------------------------------------------------------
        {
            if (log == null)
            {
                log = new Log();
            }
            return log;
        }

        //-------------------------------------------------------------
        public int Warnings()
        //-------------------------------------------------------------
        {
            return warnings;
        }

        //-------------------------------------------------------------
        public int Errors()
        //-------------------------------------------------------------
        {
            return errors;
        }

        //-------------------------------------------------------------
        public void SetGuiReference(KobberLan guiRef)
        //-------------------------------------------------------------
        {
            kobberLan = guiRef;
        }

        //-------------------------------------------------------------
        public void UnhandledException(object o, UnhandledExceptionEventArgs eventArgs)
        //-------------------------------------------------------------
        {
            Exception e = (Exception)eventArgs.ExceptionObject;
            Write(e.Message, LogType.Error);
        }

        //-------------------------------------------------------------
        public void ThreadException(object o, ThreadExceptionEventArgs eventArgs)
        //-------------------------------------------------------------
        {
            Write(eventArgs.Exception.Message, LogType.Error);
        }

        //-------------------------------------------------------------
        public void Write(string msg, LogType logType = LogType.Info)
        //-------------------------------------------------------------
        {
            // frame 1, true for source info
            StackFrame frame = new StackFrame(1, true);
            var method = frame.GetMethod();
            var fileName = System.IO.Path.GetFileNameWithoutExtension(frame.GetFileName());
            var lineNumber = frame.GetFileLineNumber();

            //-------------------------------------------------------------
            //Log to Desharp
            //-------------------------------------------------------------
            Desharp.Level desharpLevel = Desharp.Level.DEBUG;
            if (logType == LogType.Info) desharpLevel = Desharp.Level.INFO;
            if (logType == LogType.Warning) desharpLevel = Desharp.Level.WARNING;
            if (logType == LogType.Error) desharpLevel = Desharp.Level.ERROR;
            Desharp.Debug.Log(msg, desharpLevel);

            //-------------------------------------------------------------
            //Log to Console
            //-------------------------------------------------------------
            if (logType == LogType.Error) //Always log errors
            //-------------------------------------------------------------
            {
                Debug.WriteLine(msg);
                kobberLan.Invoke(new Action(() =>
                {
                    if(kobberLan != null) kobberLan.SetErrors(++errors);
                }));
            }
            //-------------------------------------------------------------
            else if (logType == LogType.Warning) //Always log warnings
            //-------------------------------------------------------------
            {
                Debug.WriteLine(msg);
                kobberLan.Invoke(new Action(() =>
                {
                    if (kobberLan != null) kobberLan.SetWarnings(++warnings);
                }));
            }
            //-------------------------------------------------------------
            else //Information
            //-------------------------------------------------------------
            {

            }

        }

    }
}
