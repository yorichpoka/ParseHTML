using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseHTML.Model
{
    public class Site_Web
    {
        // -- Privé -- //
        private long _id { get; set; }

        // -- Public -- //
        public long id { get { return this._id; } }
        public string url { get; set; }
        public object donnee { get; set; }

        public Site_Web(string url)
        {
            // -- Défnition de l'identifiant de l'objet -- //
            this._id = DateTime.Now.Ticks;

            this.url = url;
            this.donnee = new object();
        }

        // -- Rédéfnition de la méthode ToString -- //
        public override string ToString()
        {
            return this.url;
        }
    }
}
