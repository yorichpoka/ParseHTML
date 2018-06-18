using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseHTML.Model
{
    public class Programme_TV : Resource
    {
        public string chaine { get; set; }
        public string heure { get; set; }
        public string titre { get; set; }

        public Programme_TV() { }
    }
}
