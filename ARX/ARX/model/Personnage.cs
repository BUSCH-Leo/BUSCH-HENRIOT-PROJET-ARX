using ARX.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Effects;


namespace ARX.model
{

    public class Personnage
    {
        public string Nom { get; set; }
        public string Classe { get; set; }
        public Arme Armes { get; set; }
        public int VieMax { get; set; }
        public int Vie { get; set; }
        public int Force { get; set; }
        public int Dexterite { get; set; }
        public List<Effet> Effets { get; set; }
        public string TopSprite { get; set; }
        public string FrontSprite { get; set; }
        public int Money { get; set; }

        public Personnage(string nom, string classe, Arme armes, int vieMax, int vie, int force, int dexterite, string topSprite, string frontSprite, int money, List<Effet> effets = null)
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

    public class Enemy
    {
        public string Nom { get; set; }
        public string Type { get; set; }
        public int VieMax { get; set; }
        public int Vie { get; set; }
        public int degaMin { get; set; }
        public int degaMax { get; set; }
        public int probaTouch { get; set; }
        public List<Effet> Effets { get; set; }
        public int Destination { get; set; }
        public string Action { get; set; }
        public Loot Stuff { get; set; }
        public int IndexImage { get; set;}

        public static int nbimagegobl { get; } = 1;


        public Enemy(string nom, string type, int viemax, int degamin, int degamax, int probatouch, Loot stuff,int indeximage, List<Effet> effets = null)
        {
            Nom = nom;
            Type = type;
            VieMax = viemax;
            Vie = viemax;
            degaMin = degamin;
            degaMax = degamax;
            probaTouch = probatouch;
            Effets = effets;
            Stuff = stuff;
            IndexImage = indeximage;
        }
        public static Enemy GenererEnemy(int difficulte, string type = "random")
        {
            Random rand = new Random();
            if (!listeEnemy.Contains(type))
            {
                type = listeEnemy[rand.Next(listeEnemy.Count)];
            }
            if (type == "Goblin")
            {
                string prefix = randomgoblin.prefix[rand.Next(randomgoblin.prefix.Count)];
                string infix = randomgoblin.infixes[rand.Next(randomgoblin.infixes.Count)];
                string suffix = randomgoblin.suffixes[rand.Next(randomgoblin.suffixes.Count)];
                string titre = randomgoblin.titre[rand.Next(randomgoblin.titre.Count)];
                string Nom = prefix + infix + suffix + titre;
                int VieMax = rand.Next(15, 25) + difficulte / 4;
                int Vie = VieMax;
                int degaMin = rand.Next(5, 7) + difficulte / 4;
                int degaMax = rand.Next(degaMin, 20) + difficulte / 4;
                int probaTouch = (int)Math.Round(25 + 25 * (1 - Math.Exp(-0.02 * difficulte)));
                Loot stuff = new Loot();
                stuff.GenererLoot(difficulte - 20, 85, rand.Next(0, 4));
                stuff.Argent = rand.Next(0, difficulte * 2 + 12);
                int indeximage = rand.Next(1,nbimagegobl+1);

                return new Enemy(Nom, type, VieMax, degaMin, degaMax, probaTouch, stuff, indeximage);
            }
            else
            {
                throw new Exception($"Type d'ennemi inconnu : {type}");
            }
        }


        public static List<string> listeEnemy = new List<string>
        {"Goblin"};
    }

    public enum ListEffets
    {
        Poison,
        Flamme,
        Glace,
        Faiblesse,

    }
    public class Effet
    {
        public string Type { get; set; }
        public int Impacte { get; set; }
        public int Temps { get; set; }

        public Effet(string type, int impacte, int temps)
        {
            Type = type;
            Impacte = impacte;
            Temps = temps;
        }
    }


    static class randomgoblin
    {
        public static List<string> prefix = new List<string> { "Grub", "Snag", "Gri", "Mol", "War", "Sni", "Sti", "Rot", "Gob", "Fil", "Bil", "Toa", "Gru", "Sno", "Glo", "Gna", "Wre", "Slu", "Fan", "Dir", "Mag", "Muc", "Net", "Bog" };

        public static List<string> infixes = new List<string> { "na", "kle", "toe", "ter", "ba", "der", "knu", "gle", "bli", "do", "mna", "kel", "to", "ku", "ho", "ne", "ma", "ge", "ru", "lu", "ki", "la", "mo", "wa", "pa", "tu", "fe", "da", "no", "fa", "go", "pu", "fi", "ja" };

        public static List<string> suffixes = new List<string> { "sh", "ck", "tt", "ne", "zz", "nt", "nd", "ze", "le", "rt", "ge", "de", "mp", "ke", "pp", "ff", "me", "ng", "ft", "mp", "rd", "ze", "ss", "nk", "re" };

        public static List<string> titre = new List<string> { " le gobleur", " la malice", "le malicieu", " goblin depuis des générations", " le goblin", " le gabbagooblin", " le globin", " le gooblin", " le ginblo" };
    }


}