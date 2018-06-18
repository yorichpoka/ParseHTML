using ParseHTML.Model;
using ParseHTML.Model.Static;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ParseHTML.Views
{
    public partial class Form_Application : Form
    {
        // -- Variables -- //
        private string _adresse_site_web;
        private ObservableCollection<Site_Web> _historique_navigation;
        private const char separateur = '-';

        public Form_Application(string adresse_site_web)
        {
            // -- Initialiser les elements de la page -- //
            InitializeComponent();

            // -- Mise à jour des variables de classe -- //
            this._adresse_site_web = adresse_site_web;
            this.toolStripStatusLabel_date.Text = DateTime.Now.ToString("hh:mm:ss");

            // -- Activer la navigation vers l'adresse -- //
            webBrowser.Navigate(adresse_site_web);

            // -- Toujours autoriser l'execution des cript sur la page -- //
            webBrowser.ScriptErrorsSuppressed = true;

            // -- Initialiser la liste d'hitorique de navigation -- //
            this._historique_navigation = new ObservableCollection<Site_Web>();

            // -- Défini le comportement de _historique_navigation -- //
            this._historique_navigation.CollectionChanged += _historique_navigation_CollectionChanged;

            // -- AJouter la page d'acceuil à l'historique de navigation -- //
            this._historique_navigation.Add(new Site_Web(adresse_site_web));

            // -- Initialiser le répertoire contenant les ressources de l'application -- //
            Initialiser_App_Data();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // -- Confirmer l'action avant de fermer l'application -- //
            if (MessageBox.Show("Voulez-vous quitter l'application ?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // -- Frmer l'application -- //
                Program.form_application.Close();
            }
        }

        #region Méthodes de classe
        // -- Traitement à effectuer lorsque l'historique de navigation à changé -- //
        private void _historique_navigation_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                // -- Mise à jour du combo box d'historique de navigation -- //
                this.webBrowser_tb_adresse_site.Items.Clear();
                this.webBrowser_tb_adresse_site.Items.AddRange((sender as ObservableCollection<Site_Web>).Reverse().ToArray());

                // -- Mise à jour du combox box de chargement du coide HTML de la page -- //
                this.cb_html_code.Items.Clear();
                this.cb_html_code.Items.AddRange((sender as ObservableCollection<Site_Web>).Reverse().ToArray());
            }
            catch (Exception ex)
            {
                // -- Exception -- //
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Chargement de ressources de l'application -- //
        private void Chargement_ressource(List<Programme_TV> program_tv, List<Programme_TV> actualite_list)
        {
            // -- Répertoire actuel -- //
            string url_actuel = System.IO.Directory.GetCurrentDirectory();

            // -- vérifier que le répertoires existent -- //
            if (System.IO.Directory.Exists(url_actuel + "/App_Data/Sites/Audios") &&
                System.IO.Directory.Exists(url_actuel + "/App_Data/Sites/HTML") &&
                System.IO.Directory.Exists(url_actuel + "/App_Data/Sites/Images"))
            {
                // -- Variable ressources -- //
                List<Resource> resources = new List<Resource>();

                // -- Charger Les images -- //
                #region Charger Les images
                foreach (string url_image in System.IO.Directory.GetFiles(url_actuel + "/App_Data/Sites/Images"))
                {
                    // -- Si c'est une image du programme tv passer -- //
                    if (program_tv.Exists(l => l.fichier.ToString() == url_image))
                    {
                        continue;
                    }
                    else if (program_tv.Exists(l => l.fichier.ToString() == url_image))
                    {
                        continue;
                    }

                    System.IO.FileInfo info_fichier = new System.IO.FileInfo(url_image);
                    Image img = Image.FromFile(url_image);

                    resources.Add(new Resource
                    {
                        fichier = img,
                        intitule = info_fichier.Name.Split(separateur)[1],
                        page_web = _historique_navigation.First(l => l.id == Convert.ToInt64(info_fichier.Name.Split(separateur)[0])).url,
                        resolution = img.Height + ", " + img.Width,
                        taille = info_fichier.Length / 1024,
                        type = info_fichier.Extension
                    });

                    // -- Ajouter l'image dans le flot conteneur -- //
                    //this.flowLayoutPanel.Controls.Add(
                    //    new PictureBox() {
                    //        Width = (img.Width * 100 / img.Height),
                    //        Height = 100,
                    //        SizeMode = PictureBoxSizeMode.StretchImage,
                    //        ImageLocation = url_image
                    //    }
                    //);
                }
                #endregion

                #region Chargement des audios
                foreach (string url_audio in System.IO.Directory.GetFiles(url_actuel + "/App_Data/Sites/Audios"))
                {
                    System.IO.FileInfo info_fichier = new System.IO.FileInfo(url_audio);

                    resources.Add(new Resource
                    {
                        fichier = global::ParseHTML.Properties.Resources.Music_Library_icon,
                        intitule = info_fichier.Name.Split(separateur)[1],
                        page_web = _historique_navigation.First(l => l.id == Convert.ToInt64(info_fichier.Name.Split(separateur)[0])).url,
                        resolution = "Vide",
                        taille = info_fichier.Length / 1024,
                        type = info_fichier.Extension
                    });

                    // -- Ajouter l'image dans le flot conteneur -- //
                    //this.flowLayoutPanel.Controls.Add(
                    //    new PictureBox() {
                    //        Width = (img.Width * 100 / img.Height),
                    //        Height = 100,
                    //        SizeMode = PictureBoxSizeMode.StretchImage,
                    //        ImageLocation = url_image
                    //    }
                    //);
                }
                #endregion

                // -- Charger Les images -- //
                #region Charger Les program tv
                program_tv.ForEach(l =>
                {
                    System.IO.FileInfo info_fichier = new System.IO.FileInfo(l.fichier.ToString());
                    Image img = Image.FromFile(l.fichier.ToString());

                    l.fichier = img;
                    l.intitule = $"Chaine: {l.chaine} / Heure: {l.heure} / Description: {l.titre}";
                });
                #endregion

                #region Charger Les actualite tv
                actualite_list.ForEach(l =>
                {
                    System.IO.FileInfo info_fichier = new System.IO.FileInfo(l.fichier.ToString());
                    Image img = Image.FromFile(l.fichier.ToString());

                    l.fichier = img;
                    l.intitule = $"Description: {l.titre}";
                });
                #endregion

                // -- Mise à jour du mode d'afficharge des images -- //
                (this.dataGridView.Columns[1] as DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Zoom;

                // -- Mise à jour du contenu du data grid view -- //
                //this.dataGridView.Rows.Clear();
                this.dataGridView.DataSource = resources;

                // -- Mise à jour du mode d'afficharge des programme -- //
                (this.dataGridView_programme_tv.Columns[0] as DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Zoom;
                this.dataGridView_programme_tv.DataSource = program_tv;


                // -- Mise à jour du mode d'afficharge des programme -- //
                (this.dataGridView_actualite.Columns[0] as DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Zoom;
                this.dataGridView_actualite.DataSource = actualite_list;

                // -- Actualiser le conteneur -- //
                //this.flowLayoutPanel.Refresh();
            }
        }

        // -- Initialiser les fchiers de sauvegarde -- //
        private void Initialiser_App_Data()
        {
            // -- Répertoire actuel -- //
            string url_actuel = System.IO.Directory.GetCurrentDirectory();

            // -- Supprimer le répertoire temporaire de données -- //
            System.IO.Directory.Delete(url_actuel + "/App_Data/Sites/Audios", true);
            System.IO.Directory.Delete(url_actuel + "/App_Data/Sites/HTML", true);
            System.IO.Directory.Delete(url_actuel + "/App_Data/Sites/Images", true);

            // -- Créer les répertoires -- //
            System.IO.Directory.CreateDirectory(url_actuel + "/App_Data/Sites/Audios");
            System.IO.Directory.CreateDirectory(url_actuel + "/App_Data/Sites/HTML");
            System.IO.Directory.CreateDirectory(url_actuel + "/App_Data/Sites/Images");
        }
        #endregion

        #region Actions sur le navigateur
        private void webBrowser_btn_precedent_Click(object sender, EventArgs e)
        {
            // -- Accéder à la page précédente -- //
            this.webBrowser.GoBack();
        }

        private void webBrowser_btn_suivant_Click(object sender, EventArgs e)
        {
            // -- Accéder à la page suivante -- //
            this.webBrowser.GoForward();
        }

        private void webBrowser_btn_actualiser_Click(object sender, EventArgs e)
        {
            // -- Recharger la page -- //
            this.webBrowser.Refresh();
        }

        private void webBrowser_btn_arreter_Click(object sender, EventArgs e)
        {
            // -- Annuler le chargement de la page -- //
            this.webBrowser.Stop();
        }

        private void webBrowser_btn_acceuil_Click(object sender, EventArgs e)
        {
            // -- Activer la navigation vers l'adresse defini comme page d'acceuil -- //
            this.webBrowser.Navigate(this._adresse_site_web);
        }

        private void webBrowser_btn_rechercher_Click(object sender, EventArgs e)
        {
            // -- Vérivier que l'adresse est correcte -- //
            if (PHClass.Est_Site_Web(this.webBrowser_tb_adresse_site.Text))
            {
                // -- Accéder à une adresse définie -- //
                this.webBrowser.Navigate(this.webBrowser_tb_adresse_site.Text);

                // -- AJouter la page d'acceuil à l'historique de navigation si celui ci n'existe pas encore -- //
                //if (!this._historique_navigation.Contains(this.webBrowser_tb_adresse_site.Text))
                //{
                //    this._historique_navigation.Add(this.webBrowser_tb_adresse_site.Text);
                //}
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

        // -- Mise à jour du progress bar en fonction de l'etat de chargement de la page -- //
        private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            try
            {
                // -- Etat actuel du progress bar -- //
                this.webBrowser_progressBar.Value = (Convert.ToInt32(e.CurrentProgress / 10) > 100) ? 100
                                                                                                    : Convert.ToInt32(e.CurrentProgress / 10);
                // -- Valuer maximal du progress bar -- //
                this.webBrowser_progressBar.Maximum = (Convert.ToInt32(e.MaximumProgress / 10) > 100) ? 100
                                                                                                      : Convert.ToInt32(e.MaximumProgress / 10);
                // -- Mise à jour du label du progress bar -- //
                this.webBrowser_progressBar_label.Text = (this.webBrowser_progressBar.Value != 0) ? "Chargement de la page ..."
                                                                                                  : "Chargement de la page terminé.";
            }
            catch
            {
                // -- Message d'erreur -- //
                //MessageBox.Show(ex.Message);
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                // -- Mise à jour du titre de l'application -- //
                this.Text = "Site web : " + (sender as WebBrowser).Url.OriginalString;

                // -- Mise à jour de la page actuellement présente -- //
                this.webBrowser_tb_adresse_site.Text = (sender as WebBrowser).Url.OriginalString;

                // -- AJouter la page d'acceuil à l'historique de navigation si celui ci n'existe pas encore -- //
                if (this._historique_navigation.Count(l => l.url == (sender as WebBrowser).Url.OriginalString) == 0)
                {
                    this._historique_navigation.Add(new Site_Web((sender as WebBrowser).Url.OriginalString));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void webBrowser_tb_adresse_site_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // -- Accéder à une adresse définie -- //
                this.webBrowser.Navigate((sender as System.Windows.Forms.ToolStripComboBox).SelectedItem.ToString());
            }
            catch { }
        }

        // -- Enregistrer les ressources su site dans les différeents onglet des la page -- //
        private void webBrowser_btn_enregistrer_ressources_Click(object sender, EventArgs e)
        {
            try
            {
                // -- Réccupération de l'url du site web -- //
                string adresse_site_web = webBrowser_tb_adresse_site.Text;

                // -- Requete vers la page -- //
                Uri adresse_uri_site_web = new Uri(adresse_site_web);

                // -- Adresse serveur site web -- //
                string adresse_serveur_site_web = adresse_uri_site_web.Scheme + "://" + adresse_uri_site_web.Host + "/";

                // -- Répertoire actuel -- //
                string url_actuel = System.IO.Directory.GetCurrentDirectory();

                // -- Enregistrement du HTML du site -- //
                #region Enregistrement du HTML du site

                string contenu_html = PHClass.HTML_Site_Web(adresse_site_web);

                System.IO.File.WriteAllText(url_actuel + "/App_Data/Sites/HTML/" + this._historique_navigation.First(l => l.url == adresse_site_web).id + ".html", contenu_html);
                #endregion

                // -- Enregistrer les images du site -- //
                #region Enregistrer les images du site
                int numero_resource = 1;
                foreach (string url_image in PHClass.Images_Site_Web(contenu_html, true))
                {
                    // -- Essaies de sauvegarde -- //
                    KeyValuePair<int, Boolean> essaie = new KeyValuePair<int, bool>(1, false);

                    // -- Essayer tant que la sauvegarde n'est pas effectué -- //
                    while (essaie.Value == false && essaie.Key <= 3)
                    {
                        try
                        {
                            // -- Mise à jour de l'adresse d'une resources -- //
                            string url_image_modifier = "";

                            // -- Essaie 1 -- //
                            if (essaie.Key == 1)
                            {
                                // -- src = '//image/'
                                url_image_modifier = adresse_uri_site_web.Scheme + ":" + (url_image.Split(':').Count() > 1 ? url_image.Split(':')[1]
                                                                                                                           : url_image);
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 2)
                            {
                                url_image_modifier = adresse_serveur_site_web + url_image;
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 3)
                            {
                                url_image_modifier = url_image;
                            }
                            // -- Essaie 3 -- //
                            else
                            {

                            }

                            // -- Génération du lien de sauvegarde lde l'image -- //
                            string lien_sauvegarde = url_actuel + "\\App_Data\\Sites\\Images\\" + this._historique_navigation.First(l => l.url == adresse_site_web).id + separateur + (numero_resource++) + System.IO.Path.GetExtension(url_image_modifier);

                            // -- Telecharger la resource -- //
                            using (WebClient client = new WebClient())
                            {
                                // -- Telechargement de la resource à l'emplacement défini -- //
                                client.DownloadFile(new Uri(url_image_modifier), lien_sauvegarde);
                            }

                            // -- Mise à jour de l'etat d el'essaie -- //
                            essaie = new KeyValuePair<int, bool>(
                                        // -- Incrémenter l'essaie -- //
                                        essaie.Key + 1,
                                        // -- Teste si la sauvegarde s'est effectué -- //
                                        (System.IO.File.Exists(lien_sauvegarde))
                                    );
                        }
                        catch (Exception ex)
                        {
                            // -- Reinitiliser lessaie -- //
                            essaie = new KeyValuePair<int, bool>(essaie.Key + 1, false);
                        }
                    }
                }
                #endregion

                // -- Enregistrer les audios du site -- //
                #region Enregistrer les audios du site
                foreach (string url_audio in PHClass.Audio_Site_Web(contenu_html))
                {
                    // -- Essaies de sauvegarde -- //
                    KeyValuePair<int, Boolean> essaie = new KeyValuePair<int, bool>(1, false);

                    // -- Essayer tant que la sauvegarde n'est pas effectué -- //
                    while (essaie.Value == false && essaie.Key <= 2)
                    {
                        try
                        {
                            // -- Mise à jour de l'adresse d'une resources -- //
                            string url_audios_modifier = "";

                            // -- Essaie 1 -- //
                            if (essaie.Key == 1)
                            {
                                url_audios_modifier = adresse_uri_site_web.Scheme + ":" + (url_audio.Split(':').Count() > 1 ? url_audio.Split(':')[1]
                                                                                                                            : url_audio);
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 2)
                            {
                                url_audios_modifier = adresse_serveur_site_web + url_audio;
                            }
                            // -- Essaie 3 -- //
                            else
                            {

                            }

                            // -- Génération du lien de sauvegarde lde l'image -- //
                            string lien_sauvegarde = url_actuel + "\\App_Data\\Sites\\Audios\\" + this._historique_navigation.First(l => l.url == adresse_site_web).id + separateur + (numero_resource++) + System.IO.Path.GetExtension(url_audios_modifier);

                            // -- Telecharger la resource -- //
                            using (WebClient client = new WebClient())
                            {
                                // -- Telechargement de la resource à l'emplacement défini -- //
                                client.DownloadFile(new Uri(url_audios_modifier), lien_sauvegarde);
                            }

                            // -- Mise à jour de l'etat d el'essaie -- //
                            essaie = new KeyValuePair<int, bool>(
                                        // -- Incrémenter l'essaie -- //
                                        essaie.Key + 1,
                                        // -- Teste si la sauvegarde s'est effectué -- //
                                        (System.IO.File.Exists(lien_sauvegarde))
                                    );
                        }
                        catch (Exception ex)
                        {
                            // -- Reinitiliser lessaie -- //
                            essaie = new KeyValuePair<int, bool>(essaie.Key + 1, false);
                        }
                    }
                }
                #endregion

                // -- Enregistrement des programme TV du site -- //
                #region Enregistrement des programme TV du site
                List<Programme_TV> programmes_tv_list = PHClass.Programes_TV(contenu_html, true);
                foreach (Programme_TV programme_tv in programmes_tv_list)
                {
                    // -- Essaies de sauvegarde -- //
                    KeyValuePair<int, Boolean> essaie = new KeyValuePair<int, bool>(1, false);

                    // -- Essayer tant que la sauvegarde n'est pas effectué -- //
                    while (essaie.Value == false && essaie.Key <= 2)
                    {
                        try
                        {
                            // -- Mise à jour de l'adresse d'une resources -- //
                            string url_image_modifier = "";

                            // -- Essaie 1 -- //
                            if (essaie.Key == 1)
                            {
                                // -- src = '//image/'
                                url_image_modifier = adresse_uri_site_web.Scheme + ":" + (programme_tv.fichier.ToString().Split(':').Count() > 1 ? programme_tv.fichier.ToString().Split(':')[1]
                                                                                                                                                 : programme_tv.fichier.ToString());
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 2)
                            {
                                url_image_modifier = adresse_serveur_site_web + programme_tv.fichier.ToString();
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 3)
                            {
                                url_image_modifier = programme_tv.fichier.ToString();
                            }
                            // -- Essaie 3 -- //
                            else
                            {

                            }

                            // -- Génération du lien de sauvegarde lde l'image -- //
                            string lien_sauvegarde = url_actuel + "\\App_Data\\Sites\\Images\\" + this._historique_navigation.First(l => l.url == adresse_site_web).id + separateur + (numero_resource++) + System.IO.Path.GetExtension(url_image_modifier);

                            // -- Telecharger la resource -- //
                            using (WebClient client = new WebClient())
                            {
                                // -- Telechargement de la resource à l'emplacement défini -- //
                                client.DownloadFile(new Uri(url_image_modifier), lien_sauvegarde);
                            }

                            // -- Mise à jour de l'etat d el'essaie -- //
                            essaie = new KeyValuePair<int, bool>(
                                        // -- Incrémenter l'essaie -- //
                                        essaie.Key + 1,
                                        // -- Teste si la sauvegarde s'est effectué -- //
                                        (System.IO.File.Exists(lien_sauvegarde))
                                    );

                            // -- Mise à jour du lien de destination du fichier -- //
                            if (System.IO.File.Exists(lien_sauvegarde))
                            {
                                programme_tv.fichier = lien_sauvegarde;
                            }
                        }
                        catch (Exception ex)
                        {
                            // -- Reinitiliser lessaie -- //
                            essaie = new KeyValuePair<int, bool>(essaie.Key + 1, false);
                        }
                    }
                }
                #endregion

                // -- Enregistrement des programme TV du site -- //
                #region Enregistrement des actualité du site
                List<Programme_TV> actualite_list = PHClass.Actualite(contenu_html, true);
                foreach (Programme_TV actualite in actualite_list)
                {
                    // -- Essaies de sauvegarde -- //
                    KeyValuePair<int, Boolean> essaie = new KeyValuePair<int, bool>(1, false);

                    // -- Essayer tant que la sauvegarde n'est pas effectué -- //
                    while (essaie.Value == false && essaie.Key <= 2)
                    {
                        try
                        {
                            // -- Mise à jour de l'adresse d'une resources -- //
                            string url_image_modifier = "";

                            // -- Essaie 1 -- //
                            if (essaie.Key == 1)
                            {
                                // -- src = '//image/'
                                url_image_modifier = adresse_uri_site_web.Scheme + ":" + (actualite.fichier.ToString().Split(':').Count() > 1 ? actualite.fichier.ToString().Split(':')[1]
                                                                                                                                              : actualite.fichier.ToString());
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 2)
                            {
                                url_image_modifier = adresse_serveur_site_web + actualite.fichier.ToString();
                            }
                            // -- Essaie 2 -- //
                            else if (essaie.Key == 3)
                            {
                                url_image_modifier = actualite.fichier.ToString();
                            }
                            // -- Essaie 3 -- //
                            else
                            {

                            }

                            // -- Génération du lien de sauvegarde lde l'image -- //
                            string lien_sauvegarde = url_actuel + "\\App_Data\\Sites\\Images\\" + this._historique_navigation.First(l => l.url == adresse_site_web).id + separateur + (numero_resource++) + System.IO.Path.GetExtension(url_image_modifier);

                            // -- Telecharger la resource -- //
                            using (WebClient client = new WebClient())
                            {
                                // -- Telechargement de la resource à l'emplacement défini -- //
                                client.DownloadFile(new Uri(url_image_modifier), lien_sauvegarde);
                            }

                            // -- Mise à jour de l'etat d el'essaie -- //
                            essaie = new KeyValuePair<int, bool>(
                                        // -- Incrémenter l'essaie -- //
                                        essaie.Key + 1,
                                        // -- Teste si la sauvegarde s'est effectué -- //
                                        (System.IO.File.Exists(lien_sauvegarde))
                                    );

                            // -- Mise à jour du lien de destination du fichier -- //
                            if (System.IO.File.Exists(lien_sauvegarde))
                            {
                                actualite.fichier = lien_sauvegarde;
                            }
                        }
                        catch (Exception ex)
                        {
                            // -- Reinitiliser lessaie -- //
                            essaie = new KeyValuePair<int, bool>(essaie.Key + 1, false);
                        }
                    }
                }
                #endregion

                // -- Charger le contenu des images -- //
                Chargement_ressource(programmes_tv_list, actualite_list);

                // -- Message -- //
                MessageBox.Show("Telechargement des ressources terminé!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // -- Message -- //
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Teste()
        {
            for (int i = 0; i <= 500; i++)
            {
                System.Threading.Thread.Sleep(10);
            }
        }
        #endregion

        #region Action sur les resources HTML
        private void btn_charger_html_code_Click(object sender, EventArgs e)
        {
            try
            {
                // -- Détermine si l'utilisateur a selectionné un element dans la liste -- //
                if (!string.IsNullOrEmpty(cb_html_code.SelectedItem.ToString()))
                {
                    // -- Générer le lien du site - //
                    string url_site = System.IO.Directory.GetCurrentDirectory() + "/App_Data/Sites/HTML/" + (cb_html_code.SelectedItem as Site_Web).id + ".html";

                    // -- Vérifie que le resources du site ont été chargé -- //
                    if (System.IO.File.Exists(url_site))
                    {
                        // -- Lire le contenu du fchier html du site -- //
                        this.richTextBox_html_code.Text = System.IO.File.ReadAllText(url_site);
                    }
                    else
                    {
                        // -- Message -- //
                        MessageBox.Show("Le ressources du site web n'ont pas été chargés.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // -- Message -- //
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Actions sur les datagrid view

        #endregion

        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // -- Initialiser la fenêtre -- //
            Program.form_apropos = new Form_Apropos();

            // -- Ouvrir la fenêtre apropos -- //
            Program.form_apropos.ShowDialog(this);
        }

        private void Form_Application_FormClosed(object sender, FormClosedEventArgs e)
        {
            // -- QUItter l'application -- //
            Application.Exit();
        }
    }
}
