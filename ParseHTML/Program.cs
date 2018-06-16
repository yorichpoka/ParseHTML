using ParseHTML.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParseHTML
{
    static class Program
    {
        // -- Variables -- //
        public static Form_URL form_url                 { get; set; }
        public static Form_Application form_application { get; set; }
        public static Form_Apropos form_apropos         { get; set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // -- Instancier le formulaire de réccupération de l'URL -- //
            form_url = new Form_URL();

            // -- Executer l'application -- //
            Application.Run(form_url);
        }
    }
}
