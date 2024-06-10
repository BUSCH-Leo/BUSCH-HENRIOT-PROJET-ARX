using System;
using System.Collections.Generic;
using ARX.model;

namespace ARX.model
{
    public class Cellule
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Fond { get; set; }
        public string Mur { get; set; }
        public bool NorthWall { get; set; }
        public bool SouthWall { get; set; }
        public bool EastWall { get; set; }
        public bool WestWall { get; set; }
        public List<Loot> LootItems { get; set; }
        public Enemy EnemyInCell { get; set; }

        public Cellule(int x, int y, string fond, string mur, bool northWall, bool southWall, bool eastWall, bool westWall, List<Loot> lootItems, Enemy enemyInCell)
        {
            X = x;
            Y = y;
            Fond = fond;
            Mur = mur;
            NorthWall = northWall;
            SouthWall = southWall;
            EastWall = eastWall;
            WestWall = westWall;
            LootItems = lootItems;
            EnemyInCell = enemyInCell;
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
        public List<List<bool>> MatriceAdjacence { get; set; } // Correction de la syntaxe ici
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

                    var cellule = new Cellule(
                        x, y,
                        fond,
                        mur,
                        true, true, true, true,
                        new List<Loot>(),
                        null
                    );
                    Cellules.Add(cellule);
                }
            }
        }
    }
}