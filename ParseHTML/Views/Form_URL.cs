using ParseHTML.Model.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParseHTML.Views
{
    public partial class Form_URL : Form
    {
        public Form_URL()
        {
            // -- Initialisation des composants -- //
            InitializeComponent();

            // -- Définir un site web par défaut dans le champ -- //
            this.tb_adresse_site_web.Text = "https://www.moustique.be/programme-tele";
        }

        private void btn_fermer_Click(object sender, EventArgs e)
        {
            // -- Fermer l'application -- //
            Application.Exit();
        }

        private void btn_scanner_Click(object sender, EventArgs e)
        {
            // -- Vérivier que l'adresse est correcte -- //
            if (PHClass.Est_Site_Web(this.tb_adresse_site_web.Text))
            {
                // -- Me fermer -- //
                this.Hide();

                // -- Ouvrir la fenêtre application -- //
                Program.form_application = new Form_Application(this.tb_adresse_site_web.Text);
                Program.form_application.Show();
            }
            else
            {
                // -- Message d'erreur -- //
                MessageBox.Show(
                    "L'adresse du site web est incorrect, celui ci est peut être inexistant!", "information", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }
        }
    }
}
