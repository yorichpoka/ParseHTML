using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseHTML.Model
{
    public class Resource
    {
        [Description("Fichier")]
        public object fichier { get; set; }
        [Description("Page web")]
        public string page_web { get; set; }
        [Description("Resolution")]
        public string resolution { get; set; }
        [Description("Intitule")]
        public string intitule { get; set; }
        [Description("Taile")]
        public double taille { get; set; }
        [Description("Type")]
        public string type { get; set; }

        public Resource() { }
    }
}
