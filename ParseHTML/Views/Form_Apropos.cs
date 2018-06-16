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
    public partial class Form_Apropos : Form
    {
        public Form_Apropos()
        {
            InitializeComponent();

            // -- Mise à jour du contenu du panel -- //
            this.label_description.Text = "Application de réccupération des ressources d'une page web.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // -- Fermer la fenêtre -- //
            Program.form_apropos.Close();
        }
    }
}
