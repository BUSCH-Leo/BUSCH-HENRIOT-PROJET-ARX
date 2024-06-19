using System;
using System.Collections.Generic;
using ARX.model;
using ARX.controller;
using System.Formats.Asn1;
using System.IO;

namespace ARX.model
{
    public class Sortie
    {
        public int Difficulte { get; set; }

        public Sortie(int difficulte)
        {
            Difficulte = difficulte;
        }
    }
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
        public Loot loot { get; set; }
        public Enemy EnemyInCell { get; set; }
        public bool Joueur { get; set; }
        public double JoueurOrientation { get; set; }
        public int Multiargent { get; set; }
        public int Multiloot { get; set; }
        public bool IsVisible { get; set; }
        public bool IsExplored { get; set; }
        public int DifficulteSortie { get; set; }



        public Cellule(int x, int y, string fond, string mur, string joueur_top, bool northWall, bool southWall, bool eastWall, bool westWall, Loot stuff, Enemy enemyInCell, bool joueur, int multiargent, int multiloot)
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
            loot = stuff;
            EnemyInCell = enemyInCell;
            Joueur = joueur;
            JoueurOrientation = 0;
            Multiargent = multiargent;
            Multiloot = multiloot;
            IsVisible = false;
            IsExplored = false;
            DifficulteSortie = 0;
        }
        public Loot GenererLoot()
        {
            Loot loot = new Loot();

            return loot;
        }
    }

    public class Labyrinthe
    {
        public int Taille { get; set; }
        public string Type { get; set; }
        public int Profondeur { get; set; }
        public int PourcentEnnemi { get; set; }
        public int PourcentCoffre { get; set; }
        public int Difficulte { get; set; }
        public List<List<bool>> MatriceAdjacence { get; set; }
        public bool Visibilite { get; set; }
        public List<Cellule> Cellules { get; set; }
        public int Multiargent { get; set; }
        public int Multiloot { get; set; }

        public void Initialize(int taille, string type, int profondeur, int pourcentEnnemi, int pourcentCoffre, int difficulte, bool visibilite, int multiargent, int multiloot)
        {
            Taille = taille;
            Type = type;
            Profondeur = profondeur;
            PourcentEnnemi = pourcentEnnemi;
            PourcentCoffre = pourcentCoffre;
            Difficulte = difficulte;
            Visibilite = visibilite;
            Cellules = new List<Cellule>();
            Multiargent = multiargent;
            Multiloot = multiloot;

            MatriceAdjacence = new List<List<bool>>();
            for (int i = 0; i < taille * taille; i++)
            {
                List<bool> row = new List<bool>(new bool[taille * taille]);
                MatriceAdjacence.Add(row);
            }

            Random random = new Random();
            int nbfond = 5;
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
                        new Loot(),
                        null,
                        false,
                        multiargent,
                        multiloot
                    )
                    { };

                    if (random.Next(0, 100) < PourcentCoffre)
                    {
                        var loot = new Loot();
                        loot.GenererLoot(difficulte, 75, random.Next(0, 11));
                        cellule.loot = loot;
                    }

                    if (random.Next(0, 100) < PourcentEnnemi)
                    {
                        cellule.EnemyInCell = Enemy.GenererEnemy(Difficulte);
                    }


                    if (x == 0 && y == 0)
                    {
                        cellule.Joueur = true;
                    }
                    Cellules.Add(cellule);
                }
            }
            var me = this;
            if (type == "Parfait")
            {
                Generateur.LabyrintheParfait(ref me, 0, 0);
            }
            else if (type == "Imparfait")
            {
                Generateur.LabyrintheImparfait(ref me, 0, 0, 10);

            }
            else if (type == "Plusqueparfait")
            {
                Generateur.LabyrinthePlusQueParfait(ref me, 0, 0);
            }
            int tailleDivisee = (int)Math.Round((double)taille / 4);
            SetRandomExitDifficulties(1+random.Next(0,tailleDivisee));
        }

        private void SetRandomExitDifficulties(int numberexit)
        {
            Random random = new Random();
            int cellCount = Cellules.Count;

            for (int i = 0; i < numberexit; i++)
            {
                int randomIndex = random.Next(0, cellCount);
                Cellules[randomIndex].DifficulteSortie = random.Next(1, 6);
            }
        }
    }
}