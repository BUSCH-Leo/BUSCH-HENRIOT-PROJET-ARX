using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ARX.model;

namespace ARX.model
{
    public class Arx
    {
        public Labyrinthe Labyrinthe { get ; set ; }
        public string Event { get; set; }
        public int Profondeur { get; set; }
        public int Difficulte { get; set; }
        public int Seed { get; set; }

        public Arx( string eventDetail="", int difficulte=1, int etage=1, int seed = 1, int Difficulte = 1)
        {
            
            Event = eventDetail;
            Profondeur = etage;
            Seed = seed;
            Difficulte = difficulte;

        }
    }

}
