using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectModel
{
    public class Article
    {
        public int Id { get; set; }
        public string DataPubblicazione { get; set; }
        public string Titolo { get; set; }
        public string Testo { get; set; }
        public string UrlFoto { get; set; }
    }
}
