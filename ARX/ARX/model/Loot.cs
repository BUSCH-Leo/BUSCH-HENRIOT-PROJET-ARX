using System;
using ARX.model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using System.Runtime.ConstrainedExecution;
using System.Web;

namespace ARX.model
{
    public class Loot
    {
        public string LootType { get; set; }

        public Loot(string lootType, int difficulte, int valeur = 0)
        {
            LootType = lootType;
        }

        public static List<Loot> GenererLoot(int difficulte, int pourcentObjets, int pourcentArme, int nbLoot, int multiplicateurArgent = 1)
        {
            List<Loot> loots = new List<Loot>();
            Random random = new Random();
            if (difficulte < 0) { difficulte = 0; }

            if (random.Next(0, pourcentObjets+pourcentArme) < pourcentObjets)
            {
                loots.Add(new Loot("Objet", difficulte));
            }

            if (random.Next(0, pourcentObjets + pourcentArme) >= pourcentObjets)
            {
                loots.Add(new Loot("Arme", difficulte));
            }

            int argent = random.Next(0,25+difficulte) * multiplicateurArgent;
            for (int i = 0; i < argent; i++)
            {
                loots.Add(new Loot("Argent", difficulte, argent));
            }

            return loots;
        }
    }

    public class Objets
    {
        public string Type { get; set; }
        public int Value { get; set; }

        public Objets(string type, int value)
        {
            Type = type;
            Value = value;
        }
    }

    public class Money
    {
        public int Value { get; set; }
        public Personnage Personnage { get; set; }
        public Money(int valeur) {
            Value = valeur;
        }

        public void AddMoney(int value)
        {
            Value += value;
        }

        public void Buy()
        {
            // Logique d'achat ici
        }
    }


    public class Armes
    {
        public string Type { get; set; }
        public string Nom { get; set; }
        public int Level { get; set; }
        public int DegatsMin { get; set; }
        public int DegatsMax { get; set; }
        public int Probabilite { get; set; }
        public int ProbaCritique { get; set; }
        public int Multicritique { get; set; }
        public string Enchant { get; set; }

        public Armes(string type, string nom, int level, int degatsMin, int degatsMax, int probabilite, int probaCritique, int multicritique, string enchant)
        {
            Type = type;
            Nom = nom;
            Level = level;
            DegatsMin = degatsMin;
            DegatsMax = degatsMax;
            Probabilite = probabilite;
            ProbaCritique = probaCritique;
            Multicritique = multicritique;
            Enchant = enchant;
        }

        public static Armes Randarme(int difficulte, string type = "random" )
        {
            string nom = "";
            Random random = new Random();
            if (type != "Hache" || type != "Épée" || type != "Arc")
            {
                List<string> types = new List<string> { "Hache", "Épée", "Arc" };
                type = types[random.Next(types.Count)];
            }
            

            int level = random.Next(0, difficulte + difficulte / 4);
            int degatsMin, degatsMax, probabilite, penetration, probaCritique, multicritique;
            string enchant = "";

            switch (type)
            {
                case "Hache":
                    degatsMin = random.Next(Convert.ToInt32(level / 2)+1, level + 20);
                    degatsMax = random.Next(degatsMin, level * 2 + 20);
                    probabilite = random.Next(35, 65) + random.Next(0, level / 4);

                    nom =nom + nomArme.hache[random.Next(nomArme.hache.Count)];
                    break;
                case "Épée":
                    degatsMin = random.Next(Convert.ToInt32(level / 1.5) + 1, level * 3 + 10);
                    degatsMax = random.Next(degatsMin, level * 3 + 15);
                    probabilite = random.Next(35, 65) + random.Next(0, level / 4);
                    nom = nom + nomArme.épée[random.Next(nomArme.épée.Count)];
                    break;
                case "Arc":
                    degatsMin = random.Next(level, level * 3 + 5);
                    degatsMax = random.Next(degatsMin, level * 4 + 20);
                    probabilite = random.Next(20, 40) + random.Next(0, level / 6);
                    nom = nom + nomArme.arc[random.Next(nomArme.arc.Count)];
                    break;
                default:
                    throw new Exception("Type d'arme inconnu");
            }

            nom = nom + nomArme.titre[random.Next(nomArme.titre.Count)];

            probaCritique = random.Next(0, level/5+5) * level;
            multicritique = random.Next(2, level/10);

            if (level>=100 && random.Next(0,2)==1 || random.Next(level,101)==100 )
            {
                enchant.Add(Armes.listEnchant[random.Next(Armes.listEnchant.Count)]);
                nom += nomArme.enchant[random.Next(nomArme.enchant.Count)];
            }
            if (level >= 100 && random.Next(0, 2) == 1 || random.Next(level, 101) == 100)
            {
                nom = nom + nomArme.createur[random.Next(nomArme.createur.Count)];
            }

            return new Armes(type, nom, level, degatsMin, degatsMax, probabilite, probaCritique, multicritique, enchant);
        }

        public static List<string> listEnchant = new List<string>
        {
            "Poison",
            "Flamme",
            "Glace",
            "Faiblesse",
            "heal",
            "goldtouch",
        };

        static class nomArme
        {
            public static List<string> hache = new List<string> { "hache", "hache de guerre", "pioche", "étoile du matin" };

            public static List<string> épée = new List<string> { "glaive", "épée", "dague" };

            public static List<string> arc = new List<string> { "arc", "arbalette", "arc long", "arc incurvé", "arc a pulley", "arbalette longue" };

            public static List<string> titre = new List<string> { " de destruction", " neuve", " en mauvaise états", " moisie", "sur mesure", " plaqué or", " desctructeur", " de l'ombre", " de l'inquisition" };

            public static List<string> enchant = new List<string> { " magique", " enchanté", " élémentaire", " éblouissant" };

            public static List<string> createur = new List<string> { $" de" + new Enemy("Goblin", 1).Nom };

        }
    
    }
}