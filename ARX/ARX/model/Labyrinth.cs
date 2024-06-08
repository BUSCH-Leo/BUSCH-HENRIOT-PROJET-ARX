using System;
using System.Collections.Generic;
using ARX.model;

namespace ARX.model
{
    public class Cellule
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string BackgroundImage { get; set; }
        public string WallImage { get; set; }
        public bool NorthWall { get; set; }
        public bool SouthWall { get; set; }
        public bool EastWall { get; set; }
        public bool WestWall { get; set; }
        public List<Loot> LootItems { get; set; }
        public Enemy EnemyInCell { get; set; }

        public Cellule(string backgroundImage, string wallImage, bool northWall, bool southWall, bool eastWall, bool westWall, List<Loot> lootItems, Enemy enemyInCell)
        {
            BackgroundImage = backgroundImage;
            WallImage = wallImage;
            NorthWall = northWall;
            SouthWall = southWall;
            EastWall = eastWall;
            WestWall = westWall;
            LootItems = lootItems;
            EnemyInCell = enemyInCell;
            X = x;
            Y = y;
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
        public int MatriceAdjacence { get; set; }
        public int Visibilite { get; set; }
        public List<Cellule> Cellules { get; set; }

        public void Initialize(int taille, string type, int profondeur, int quotaSpawn, int pourcentEnnemi, int pourcentCoffre, int difficulte, int matriceAdjacence, int visibilite, List<Cellule> Cellules)
        {
            Taille = taille;
            Type = type;
            Profondeur = profondeur;
            QuotaSpawn = quotaSpawn;
            PourcentEnnemi = pourcentEnnemi;
            PourcentCoffre = pourcentCoffre;
            Difficulte = difficulte;
            MatriceAdjacence = matriceAdjacence;
            Visibilite = visibilite;
            Cellules = Cellules;
        }
    }

}
