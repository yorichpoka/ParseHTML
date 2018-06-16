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
            }
            catch { }

            return 
                images
                    .Distinct()
                    .ToList();
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
