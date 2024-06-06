using ARX.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARX.Model
{

    public class Personnage
    {
        public string Nom { get; set; }
        public string Classe { get; set; }
        public Armes Armes { get; set; }
        public int VieMax { get; set; }
        public int Vie { get; set; }
        public int Force { get; set; }
        public int Dexterite { get; set; }
        public Effets Effets { get; set; }
        public string TopSprite { get; set; }
        public string FrontSprite { get; set; }
        public int Money { get; set; }

        public Personnage(string nom, string classe, Armes armes, int vieMax, int vie, int force, int dexterite, Effets effets, string topSprite, string frontSprite, int money)
        {
            Nom = nom;
            Classe = classe;
            Armes = armes;
            VieMax = vieMax;
            Vie = vie;
            Force = force;
            Dexterite = dexterite;
            Effets = effets;
            TopSprite = topSprite;
            FrontSprite = frontSprite;
            Money = money;
        }
    }

    public class Ennemis
    {
        public string Nom { get; set; }
        public string Classe { get; set; }
        public Armes Armes { get; set; }
        public int VieMax { get; set; }
        public int Vie { get; set; }
        public int Force { get; set; }
        public int Dexterite { get; set; }
        public Effets Effets { get; set; }
        public int Destination { get; set; }
        public string Action { get; set; }
        public int Money { get; set; }

        public Ennemis(string nom, string classe, Armes armes, int vieMax, int vie, int force, int dexterite, Effets effets, int destination, string action, int money)
        {
            Nom = nom;
            Classe = classe;
            Armes = armes;
            VieMax = vieMax;
            Vie = vie;
            Force = force;
            Dexterite = dexterite;
            Effets = effets;
            Destination = destination;
            Action = action;
            Money = money;
        }
    }

    public enum Effets
    {
        Poison,
        Flamme,
        Glace
    }


}