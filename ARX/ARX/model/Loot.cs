﻿using System;
using System.Collections.Generic;

namespace ARX.model
{
    public class Loot
    {
        public List<Arme> Armes { get; set; }
        public List<Objet> Objets { get; set; }
        public int Argent { get; set; }

        public Loot()
        {
            Armes = new List<Arme>();
            Objets = new List<Objet>();
            Argent = 0;
        }

        public void CombineLoot(Loot autreloot)
        {
            if (autreloot == null) return;

            Armes.AddRange(autreloot.Armes);
            Objets.AddRange(autreloot.Objets);
            Argent += autreloot.Argent;
        }

        public void GenererLoot(int difficulte, int pourcentObjetsSurArme=80, int nbLoot = 1)
        {
            Random random = new Random();

            if (difficulte < 0) difficulte = 0;

            for (int i = 0; i < nbLoot; i++)
            {
                if (random.Next(0, 101) < pourcentObjetsSurArme)
                {
                    Objets.Add(Objet.Randobj(difficulte));
                }
                else
                {
                    Armes.Add(Arme.Randarme(difficulte));
                }
            }
        }
    }

    public class Objet
    {
        public string Type { get; set; }
        public int Value { get; set; }

        public Objet(string type, int value)
        {
            Type = type;
            Value = value;
        }

        public static Objet Randobj(int difficulte)
        {
            Random random = new Random();
            string type = "TypeAléatoire";
            int value = random.Next(1, 101);
            return new Objet(type, value);
        }
    }

    public class Arme
    {
        public string Type { get; set; }
        public string Nom { get; set; }
        public int Level { get; set; }
        public int DegatsMin { get; set; }
        public int DegatsMax { get; set; }
        public int Probabilite { get; set; }
        public int ProbaCritique { get; set; }
        public int Multicritique { get; set; }
        public string Enchant { get; set; } = "Non enchanté";

        public Arme(string type, string nom, int level, int degatsMin, int degatsMax, int probabilite, int probaCritique, int multicritique, string enchant)
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

        public static Arme Randarme(int difficulte, string type = "random")
        {
            Random random = new Random();
            if (type != "Hache" && type != "Épée" && type != "Arc")
            {
                List<string> types = new List<string> { "Hache", "Épée", "Arc" };
                type = types[random.Next(types.Count)];
            }

            int level = random.Next(0, difficulte + difficulte / 4);
            int degatsMin, degatsMax, probabilite, probaCritique, multicritique;
            string enchant = "";

            switch (type)
            {
                case "Hache":
                    degatsMin = random.Next(Convert.ToInt32(level / 2) + 1, level + 20);
                    degatsMax = random.Next(degatsMin, level * 2 + 20);
                    probabilite = random.Next(35, 65) + random.Next(0, level / 4);
                    break;
                case "Épée":
                    degatsMin = random.Next(Convert.ToInt32(level / 1.5) + 1, level * 3 + 10);
                    degatsMax = random.Next(degatsMin, level * 3 + 15);
                    probabilite = random.Next(35, 65) + random.Next(0, level / 4);
                    break;
                case "Arc":
                    degatsMin = random.Next(level, level * 3 + 5);
                    degatsMax = random.Next(degatsMin, level * 4 + 20);
                    probabilite = random.Next(20, 40) + random.Next(0, level / 6);
                    break;
                default:
                    throw new Exception("Type d'arme inconnu");
            }

            probaCritique = random.Next(0, level / 5 + 5) * level;
            if (level < 30){ multicritique = 2; }
            else {
                multicritique = random.Next(2, level / 10);
            }
            if (level >= 100 && random.Next(0, 2) == 1 || random.Next(level, 101) == 100)
            {
                enchant = listEnchant[random.Next(listEnchant.Count)];
            }

            return new Arme(type, type, level, degatsMin, degatsMax, probabilite, probaCritique, multicritique, enchant); // Léo j'ai modif "NomAléatoire" par type pour qu'on puisse voir le type de l'arme à quoi ca correspond
        }

        public static List<string> listEnchant = new List<string>
        {
        "Poison",
        "Flamme",
        "Glace",
        "Faiblesse",
        "Heal",
        "Goldtouch"
        };
    }

}
