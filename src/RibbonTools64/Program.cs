using System;
using System.Windows.Forms;

namespace UIRibbonTools
{
    internal static class Program
    {
        /// <summary>
        /// The Main form
        /// </summary>
        public static MainForm ApplicationForm { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (ConsoleHelper.Execute(args))
                    return;
            }

#if NET10_0_OR_GREATER
            Application.SetColorMode(SystemColorMode.System);
#endif
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(ApplicationForm = new MainForm());
        }
    }
}