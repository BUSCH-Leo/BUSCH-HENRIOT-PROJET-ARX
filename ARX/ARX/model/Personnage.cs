using ARX.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ARX.model
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

    public class Enemy
    {
        public string Nom { get; set; }
        public string Type { get; set; }
        public int VieMax { get; set; }
        public int Vie { get; set; }
        public int degaMin { get; set; }
        public int degaMax { get; set; }
        public int probaTouch { get; set; }
        public Effets Effets { get; set; }
        public int Destination { get; set; }
        public string Action { get; set; }
        public List<Loot> stuff { get; set; }

        public Enemy(, int difficulte)
        {
            Random random = new Random();
            Type = type;
            string prefix = "";
            string infix = "";
            string suffix = "";
            string titre = "";

            if (type == "Goblin")
            {
                Random rand = new Random();
                prefix = randomgoblin.prefix[rand.Next(randomgoblin.prefix.Count)];
                infix = randomgoblin.infixes[rand.Next(randomgoblin.infixes.Count)];
                suffix = randomgoblin.suffixes[rand.Next(randomgoblin.suffixes.Count)];
                titre = randomgoblin.titre[rand.Next(randomgoblin.titre.Count)];
                string Nom = prefix + infix + suffix + titre;
                VieMax = random.Next(15, 25) + difficulte / 4;
                Vie = VieMax;
                degaMin = random.Next(5, 15) + difficulte / 4;
                stuff = Loot.GenererLoot(difficulte - 20,85,rand.Next(0,4));
                stuff.argent=rand.Next(0,difficulte*2+12)
            }
        }

        public static List<string> listeEnemy = new List<string>
        {"Goblin"};
    }

    public enum Effets
    {
        Poison,
        Flamme,
        Glace,
        Faiblesse,

    }

    

    static class randomgoblin
    {
        public static List<string> prefix = new List<string> { "Grub", "Snag", "Gri", "Mol", "War", "Sni", "Sti", "Rot", "Gob", "Fil", "Bil", "Toa", "Gru", "Sno", "Glo", "Gna", "Wre", "Slu", "Fan", "Dir", "Mag", "Muc", "Net", "Bog" };

        public static List<string> infixes = new List<string> { "na", "kle", "toe", "ter", "ba", "der", "knu", "gle", "bli", "do", "mna", "kel", "to", "ku", "ho", "ne", "ma", "ge", "ru", "lu", "ki", "la", "mo", "wa", "pa", "tu", "fe", "da", "no", "fa", "go", "pu", "fi", "ja" };

        public static List<string> suffixes = new List<string> { "sh", "ck", "tt", "ne", "zz", "nt", "nd", "ze", "le", "rt", "ge", "de", "mp", "ke", "pp", "ff", "me", "ng", "ft", "mp", "rd", "ze", "ss", "nk", "re" };

        public static List<string> titre = new List<string> { " le gobleur", " la malice", "le malicieu", "goblin depuis des générations", "le goblin", "le gabbagooblin", "le globin", "le gooblin", "le ginblo" };
    }


}