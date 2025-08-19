using System;
using System.Windows.Forms;

namespace EmCRUD
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            if (!DbBootstrap.TryInitWithMessage())
                return; 

            Application.Run(new Form1());
        }
    }
}
