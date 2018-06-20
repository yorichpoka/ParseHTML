using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParseHTML.Model.Static
{
    public static class PHClass
    {
        /// <summary>
        /// Vérifie qu'une adresse site web est existante
        /// </summary>
        public static bool Est_Site_Web(string adresse_site_web)
        {
            try
            {
                using (HttpClient Client = new HttpClient())
                {
                    HttpResponseMessage result = Client.GetAsync(new Uri(adresse_site_web)).Result;

                    return 
                        (result.StatusCode == HttpStatusCode.Accepted || result.StatusCode == HttpStatusCode.OK) ? true 
                                                                                                                 : false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtenir le code source html d'une page web
        /// </summary>
        public static string HTML_Site_Web(string adresse_site_web)
        {
            try
            {
                // -- Teste si l'adresse est valide -- //
                if (Est_Site_Web(adresse_site_web))
                {
                    // -- Creation de la requete -- //
                    HttpWebRequest request = WebRequest.Create(adresse_site_web) as HttpWebRequest;

                    // -- Réccupération de la réponse -- //
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    // -- Lecture du contenu de la réponse -- //
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        return
                            streamReader.ReadToEnd();
                    }
                }
            }
            catch { }

            return string.Empty;
        }

        /// <summary>
        /// Obtenir les images contenues dans une page web
        /// </summary>
        public static List<string> Images_Site_Web(string contenu, Boolean est_contenu_html)
        {
            List<string> images = new List<string>();

            try
            {
                // -- Creation de l'objet page site web -- // 
                HtmlDocument page_html = new HtmlDocument();

                // -- Charger le contenu du document -- //
                page_html.LoadHtml(
                    est_contenu_html ? contenu
                                     : HTML_Site_Web(contenu)
                );

                // -- Réccupérer les liens de toutes les images dans le site -- //
                images.AddRange(
                    page_html
                        // -- Balise root -- //
                        .DocumentNode
                        // -- Parcourir toutes la balises 'img' contenu dans le root -- //                                    
                        .Descendants("img")
                        // -- Selectionner les attribut 'src' des balises 'img' trouvées -- //                                 
                        .Select(l => l.GetAttributeValue("src", null))   
                        // -- Ne selectionner que cexu qui on une valeur -- //   
                        .Where(l => !string.IsNullOrEmpty(l) && !string.IsNullOrWhiteSpace(l))
                        // -- Lister les resultats obtenus -- //
                        .ToList()
                );

                // -- Réccupérer les liens de toutes les images dans le site -- //
                images.AddRange(
                    page_html
                        // -- Balise root -- //
                        .DocumentNode
                        // -- Parcourir toutes la balises 'img' contenu dans le root -- //                                    
                        .Descendants("img")
                        // -- Selectionner les attribut 'src' des balises 'img' trouvées -- //                                 
                        .Select(l => l.GetAttributeValue("dsrc", null))
                        // -- Ne selectionner que cexu qui on une valeur -- //   
                        .Where(l => !string.IsNullOrEmpty(l) && !string.IsNullOrWhiteSpace(l))
                        // -- Lister les resultats obtenus -- //
                        .ToList()
                );
            }
            catch { }

            return 
                images
                    .Distinct()
                    .ToList();
        }

        /// <summary>
        /// Obtenir les images contenues dans une page web
        /// </summary>
        public static List<Programme_TV> Programes_TV(string contenu, Boolean est_contenu_html)
        {
            List<Programme_TV> liste_des_programmes = new List<Programme_TV>();

            try
            {
                // -- Creation de l'objet page site web -- // 
                HtmlDocument page_html = new HtmlDocument();

                #region Cas 1
                // -- Charger le contenu du document -- //
                page_html.LoadHtml(
                    est_contenu_html ? contenu
                                     : HTML_Site_Web(contenu)
                );

                // -- Traitement dans le document HTML -- //
                page_html
                    // -- Balise root -- //
                    .DocumentNode
                    // -- Parcourir toutes la balises 'div' contenu dans le root -- //                                    
                    .Descendants("div")
                    // -- QUi ont pour class grid_2 -- //
                    .Where(l => l.HasClass("grid_2"))
                    // -- Lister le résultat -- //
                    .ToList()
                    // -- Parcourir le résultat -- //
                    .ForEach(div_chaine => {
                        // -- Déclaration du nom de la chaine -- //
                        string chaine = "Programme";
                        try
                        {
                            // -- Mise à jour du nom de la chaine -- //
                            chaine = div_chaine.Descendants("div")
                                                .FirstOrDefault(l => l.HasClass("chanel-header"))
                                                    .Descendants("div")
                                                        .FirstOrDefault(l => l.HasClass("chanel-logo"))
                                                            .Descendants("img")
                                                                .FirstOrDefault().GetAttributeValue("alt", "Programme");
                        }
                        catch(Exception ex) { }

                        try
                        {
                            // -- Lister les programmes de la chaine -- //
                            var div_programme = div_chaine.Descendants("div").FirstOrDefault(l => l.HasClass("chanel-content"));

                            div_programme
                                .Descendants("a")
                                .ToList()
                                .ForEach(program =>
                                {
                                    try
                                    {
                                        // -- Ajout du programme -- //
                                        Programme_TV prog = new Programme_TV();

                                        // -- Réccupération de la chaine -- //
                                        prog.chaine = chaine;
                                        // -- Réccupération de l'heure -- //
                                        prog.heure = program.Descendants("h5").FirstOrDefault().InnerHtml;
                                        // -- Réccupération du titre -- //
                                        prog.titre = program.Descendants("h4").FirstOrDefault().InnerHtml;
                                        // -- réccupération de l'image -- //
                                        prog.fichier = program.Descendants("img").FirstOrDefault().GetAttributeValue("src", string.Empty);

                                        // -- AJout dans la liste -- //
                                        liste_des_programmes.Add(prog);
                                    }
                                    catch(Exception ex) { }
                                });
                        }
                        catch { }
                    });
                #endregion

                #region Cas 2
                try
                {
                    // -- Charger le contenu du document -- //
                    page_html.LoadHtml(
                        est_contenu_html ? contenu
                                         : HTML_Site_Web(contenu)
                    );

                    // -- Traitement dans le document HTML -- //
                    page_html
                        // -- Balise root -- //
                        .DocumentNode
                        // -- Parcourir toutes la balises 'div' contenu dans le root -- //                                    
                        .Descendants("div")
                        // -- QUi ont pour class programs -- //
                        .FirstOrDefault(l => l.HasClass("programs"))
                        // -- Parcourir les div qui sont dans programs -- //
                        .Descendants("div")
                        // -- Qui ont pour classe row -- //
                        .Where(l => l.HasClass("row"))
                        // -- Lister le résultat -- //
                        .ToList()
                        // -- Parcourir le résultat -- //
                        .ForEach(div_programme =>
                        {
                            // -- Déclaration du nom de la chaine -- //
                            string chaine = "Programme";
                            //try
                            //{
                            //// -- Mise à jour du nom de la chaine -- //
                            //chaine = div_chaine.Descendants("div")
                            //                        .FirstOrDefault(l => l.HasClass("chanel-header"))
                            //                            .Descendants("div")
                            //                                .FirstOrDefault(l => l.HasClass("chanel-logo"))
                            //                                    .Descendants("img")
                            //                                        .FirstOrDefault().GetAttributeValue("alt", "Programme");
                            //}
                            //catch (Exception ex) { }

                            try
                            {
                                // -- Lister les programmes de la chaine -- //
                                div_programme.Descendants("div")
                                    .Where(l => l.HasClass("item"))
                                    .ToList()
                                    .ForEach(div_chaine =>
                                    {
                                        try
                                        {
                                            // -- Ajout du programme -- //
                                            Programme_TV prog = new Programme_TV();

                                            // -- Réccupération de la chaine -- //
                                            prog.chaine = chaine;
                                            // -- Réccupération de l'heure -- //
                                            prog.heure = div_chaine.Descendants("div").FirstOrDefault(l => l.HasClass("hour-type")).InnerHtml.Replace("\t", string.Empty);
                                            // -- Réccupération du titre -- //
                                            prog.titre = div_chaine.Descendants("a").FirstOrDefault(l => l.HasClass("title")).InnerHtml.Replace("\t", string.Empty);
                                            // -- réccupération de l'image -- //
                                            prog.fichier = div_chaine.Descendants("img").FirstOrDefault().GetAttributeValue("dsrc", string.Empty).Replace("\t", string.Empty);

                                            // -- AJout dans la liste -- //
                                            liste_des_programmes.Add(prog);
                                        }
                                        catch (Exception ex) { }
                                    });
                            }
                            catch { }
                        });
                }
                catch { }
                #endregion
            }
            catch { }

            return
                liste_des_programmes;
        }

        /// <summary>
        /// Obtenir les images contenues dans une page web
        /// </summary>
        public static List<Programme_TV> Actualite(string contenu, Boolean est_contenu_html)
        {
            List<Programme_TV> liste_des_actialite = new List<Programme_TV>();

            try
            {
                // -- Creation de l'objet page site web -- // 
                HtmlDocument page_html = new HtmlDocument();

                // -- Charger le contenu du document -- //
                page_html.LoadHtml(
                    est_contenu_html ? contenu
                                     : HTML_Site_Web(contenu)
                );

                // -- Réccupération du premier iframe -- //
                string url_actualite = page_html
                                        .DocumentNode
                                        .Descendants("iframe")
                                        .FirstOrDefault()
                                        .GetAttributeValue("src", "");

                // -- Mise à jour de l'url -- //
                url_actualite = "https:" + url_actualite;

                // -- Mise à jour du html page -- //
                page_html.LoadHtml(HTML_Site_Web(url_actualite));

                // -- Traitement dans le document HTML -- //
                page_html
                    // -- Balise root -- //
                    .DocumentNode
                    // -- Parcourir toutes la balises 'div' contenu dans le root -- //                                    
                    .Descendants("ul")
                    // -- QUi ont pour class grid_2 -- //
                    .FirstOrDefault(l => l.HasClass("nobullets"))
                    .Descendants("li")
                    // -- Lister le résultat -- //
                    .ToList()
                    // -- Parcourir le résultat -- //
                    .ForEach(li_actu => {
                        try
                        {
                            // -- Ajout du programme -- //
                            Programme_TV actu = new Programme_TV();

                            // -- Réccupération du titre -- //
                            actu.titre = li_actu.Descendants("h1").FirstOrDefault().InnerHtml;
                            // -- réccupération de l'image -- //
                            actu.fichier = li_actu.Descendants("img").FirstOrDefault().GetAttributeValue("src", null);

                            // -- AJout dans la liste -- //
                            liste_des_actialite.Add(actu);
                        }
                        catch { }
                    });
            }
            catch { }

            return
                liste_des_actialite;
        }

        /// <summary>
        /// Obtenir les audios contenues dans une page web
        /// </summary>
        public static List<string> Audio_Site_Web(string contenu_html)
        {
            List<string> audios = new List<string>();

            try
            {
                // -- Creation de l'objet page site web -- // 
                HtmlDocument page_html = new HtmlDocument();

                // -- Charger le contenu du document -- //
                page_html.LoadHtml(contenu_html);

                // -- Réccupérer les liens de tous les audios dans le site -- //
                audios.AddRange(
                    page_html
                        // -- Balise root -- //
                        .DocumentNode
                        // -- Parcourir toutes la balises 'a' contenu dans le root -- //                                    
                        .Descendants("a")
                        // -- Selectionner les attribut 'src' des balises 'img' trouvées -- //                                 
                        .Select(l => l.GetAttributeValue("href", null))
                        // -- Ne selectionner que cexu qui on une valeur -- //   
                        .Where(l => !string.IsNullOrEmpty(l) && !string.IsNullOrWhiteSpace(l) && (l.Contains(".wav") || l.Contains(".mp3")))
                        // -- Lister les resultats obtenus -- //
                        .ToList()
                );
            }
            catch { }

            return
                audios
                    .Distinct()
                    .ToList();
        }
    }
}
