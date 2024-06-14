using System;
using System.Collections.Generic;
using ARX.model;
using ARX.controller;
using System.Formats.Asn1;
using System.IO;

namespace ARX.model
{
    public class Cellule
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Fond { get; set; }
        public string Mur { get; set; }
        public string Joueur_top { get; set; }
        public bool NorthWall { get; set; }
        public bool SouthWall { get; set; }
        public bool EastWall { get; set; }
        public bool WestWall { get; set; }
        public List<Loot> ListeLoot { get; set; }
        public Enemy EnemyInCell { get; set; }
        public bool Joueur { get; set; }
        public double JoueurOrientation { get; set; }

        public Cellule(int x, int y, string fond, string mur, string joueur_top, bool northWall, bool southWall, bool eastWall, bool westWall, List<Loot> listeLoot, Enemy enemyInCell, bool joueur)
        {
            X = x;
            Y = y;
            Fond = fond;
            Mur = mur;
            Joueur_top = joueur_top;
            NorthWall = northWall;
            SouthWall = southWall;
            EastWall = eastWall;
            WestWall = westWall;
            ListeLoot = listeLoot;
            EnemyInCell = enemyInCell;
            Joueur = joueur;
            JoueurOrientation = 0;
        }
    }


    public class Labyrinthe
    {
        public int Taille { get; set; }
        public string Type { get; set; }
        public int Profondeur { get; set; }
        public int QuotaSpawn { get; set; }
        public int PourcentEnnemi { get; set; }
        public int PourcentCoffre { get; set; }
        public int Difficulte { get; set; }
        public List<List<bool>> MatriceAdjacence { get; set; }
        public bool Visibilite { get; set; }
        public List<Cellule> Cellules { get; set; }

        public void Initialize(int taille, string type, int profondeur, int quotaSpawn, int pourcentEnnemi, int pourcentCoffre, int difficulte, bool visibilite)
        {
            Taille = taille;
            Type = type;
            Profondeur = profondeur;
            QuotaSpawn = quotaSpawn;
            PourcentEnnemi = pourcentEnnemi;
            PourcentCoffre = pourcentCoffre;
            Difficulte = difficulte;
            Visibilite = visibilite;
            Cellules = new List<Cellule>();

            MatriceAdjacence = new List<List<bool>>();
            for (int i = 0; i < taille * taille; i++)
            {
                List<bool> row = new List<bool>(new bool[taille * taille]);
                MatriceAdjacence.Add(row);
            }

            Random random = new Random();
            int nbfond= 5;
            int nbmurs = 2;

            for (int x = 0; x < taille; x++)
            {
                for (int y = 0; y < taille; y++)
                {
                    string fond = $"pack://application:,,,/ARX;component/view/Images/fond{random.Next(1, nbfond + 1)}.png";
                    string mur = $"pack://application:,,,/ARX;component/view/Images/mur{random.Next(1, nbmurs + 1)}.png";
                    string joueur_top = $"pack://application:,,,/ARX;component/view/Images/joueur_top.png";


                    var cellule = new Cellule(
                        x, y,
                        fond,
                        mur,
                        joueur_top,
                        true, true, true, true,
                        new List<Loot>(),
                        null,
                        false
                    )
                    {
                    };
                    if (random.Next(0, 100) < PourcentCoffre * Difficulte)
                    {
                        cellule.ListeLoot.Add(GenererLoot());
                    }

                    if (random.Next(0, 100) < PourcentEnnemi * Difficulte)
                    {
                        cellule.EnemyInCell = GenererEnemy();
                    }

                    if (x == 0 && y == 0)
                    {
                        cellule.Joueur = true;
                    }
                    Cellules.Add(cellule);
                }
            }
            var me = this;
            if (type == "parfait")
            {
                Generateur.LabyrintheParfait(ref me, 0, 0);
            }
            else if (type == "imparfait")
            {
                Generateur.LabyrintheImparfait(ref me, 0, 0, 10);
            }
            else if (type == "plusqueparfait")
            {
                Generateur.LabyrinthePlusQueParfait(ref me, 0, 0);
            }
        }

        private Loot GenererLoot()
        {
            Loot loot = new Loot();
            return loot;
        }

        private Enemy GenererEnemy()
        {
            // Liste de types d'ennemis possibles (à adapter selon votre jeu)
            List<string> typesEnnemis = new List<string> { "Guerrier", "Mage", "Archer", "Boss" };

            // Choix aléatoire d'un type d'ennemi
            Random random = new Random();
            string typeEnnemi = typesEnnemis[random.Next(typesEnnemis.Count)];

            // Lecture du fichier CSV pour les noms aléatoires
            Dictionary<string, NomsAleatoires> nomsAleatoiresParType = LireNomsAleatoiresDepuisCSV();

            // Vérification si le type d'ennemi est présent dans les données lues
            if (nomsAleatoiresParType.ContainsKey(typeEnnemi))
            {
                NomsAleatoires nomsAleatoires = nomsAleatoiresParType[typeEnnemi];
                string nomAleatoire = GenererNomAleatoire(nomsAleatoires, random);

                // Création de l'ennemi avec les attributs générés aléatoirement
                return new Enemy
                {
                    Nom = nomAleatoire,
                    Type = typeEnnemi,
                    VieMax = random.Next(50, 101), // Exemple de génération de la vie maximale
                    Vie = 100, // Exemple de valeur initiale de la vie
                    // Autres attributs à initialiser selon votre jeu
                };
            }
            else
            {
                // Si le type d'ennemi n'est pas trouvé dans les données du CSV, retourner un ennemi par défaut
                return new Enemy
                {
                    Nom = "Ennemi Inconnu",
                    Type = typeEnnemi,
                    VieMax = 100,
                    Vie = 100,
                    // Autres attributs par défaut
                };
            }
        }

        private Dictionary<string, NomsAleatoires> LireNomsAleatoiresDepuisCSV()
        {
            Dictionary<string, NomsAleatoires> data = new Dictionary<string, NomsAleatoires>();

            // Chemin vers votre fichier CSV contenant les noms aléatoires par type d'ennemi
            string cheminFichierCSV = "chemin_vers_votre_fichier.csv";

            // Lecture du fichier CSV et parsing des données
            using (var reader = new StreamReader(cheminFichierCSV))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                // Lecture des données et stockage dans la structure appropriée
                var records = csv.GetRecords<NomsAleatoires>();

                foreach (var record in records)
                {
                    data.Add(record.TypeEnnemi, record);
                }
            }

            return data;
        }

        private string GenererNomAleatoire(NomsAleatoires nomsAleatoires, Random random)
        {
            // Logique pour générer un nom aléatoire en utilisant les préfixes, infixes, suffixes et titre
            string nomAleatoire = $"{nomsAleatoires.Prefixes[random.Next(nomsAleatoires.Prefixes.Count)]}" +
                                  $"{nomsAleatoires.Infixes[random.Next(nomsAleatoires.Infixes.Count)]}" +
                                  $"{nomsAleatoires.Suffixes[random.Next(nomsAleatoires.Suffixes.Count)]}" +
                                  $" {nomsAleatoires.Titres[random.Next(nomsAleatoires.Titres.Count)]}";

            return nomAleatoire;
        }
    }
}