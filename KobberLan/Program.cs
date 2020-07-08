using KobberLan.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KobberLan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Catch all exceptions
            Application.ThreadException += new ThreadExceptionEventHandler(Log.Get().ThreadException); // Add the event handler for handling UI thread exceptions to the event.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException); // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Log.Get().UnhandledException); // Add the event handler for handling non-UI thread exceptions to the event. 

            //Start program
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KobberLan());
        }
    }
}
