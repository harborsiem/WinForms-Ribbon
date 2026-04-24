using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _08_Images
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
