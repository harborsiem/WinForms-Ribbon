//#define german
//comment or uncomment line before to change language
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace _19_Localization
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if german
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
#else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
#endif
#if NET462_OR_GREATER
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#else
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
#endif
#if NET10_0_OR_GREATER
            Application.SetColorMode(SystemColorMode.System);
#endif
            Application.Run(new Form1());
        }
    }
}
