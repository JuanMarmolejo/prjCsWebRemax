using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using prjCsWebRemax.gui;

namespace prjCsWebRemax
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Run the login form first
            frmLogin frlog = new frmLogin();
            frlog.ShowDialog();

            //If the user is valid, enter the main form.
            if (frlog.DialogResult == DialogResult.OK)
            {
                Application.Run(new frmPrincipal());
            }
        }
    }
}
