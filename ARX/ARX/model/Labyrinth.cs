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

        //labyActuel.Initialize(ARX.profondeur, ARX.difficulte, fogofwar);

        public void Initialize(int profondeur, int difficulte, bool visibilite, int multiargent = 1, int multiloot = 1)
        {
            Random random = new Random();
            if (profondeur < 15) { Taille = random.Next(5, profondeur + 10); }
            else if (profondeur <= 40) { Taille = random.Next(profondeur - 10, profondeur + 10); }
            else { Taille = random.Next(profondeur - 10, 50); }
            int randtype = random.Next(1, 100);
            if (randtype < 66) { Type = "Imparfait"; }
            else { Type = "Plusqueparfait"; }
            Profondeur = profondeur;
            PourcentEnnemi = random.Next(5, 20);
            PourcentCoffre = random.Next(15, 60);
            Difficulte = difficulte;
            Visibilite = visibilite;
            Cellules = new List<Cellule>();
            Multiargent = multiargent;
            Multiloot = multiloot;
            int entre = 0;

            MatriceAdjacence = new List<List<bool>>();
            for (int i = 0; i < Taille * Taille; i++)
            {
                List<bool> row = new List<bool>(new bool[Taille * Taille]);
                MatriceAdjacence.Add(row);
            }

            int nbfond = 5;
            int nbmurs = 2;

            for (int x = 0; x < Taille; x++)
            {
                for (int y = 0; y < Taille; y++)
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
            int entreX = random.Next(1, Taille + 1);
            int entreY = random.Next(1, Taille + 1);
            entre = entreY * Taille + entreX;
            var me = this;
            if (Type == "Parfait")
            {
                Generateur.LabyrintheParfait(ref me, entreX, entreY);
            }
            else if (Type == "Imparfait")
            {
                Generateur.LabyrintheImparfait(ref me, entreX, entreY,random.Next(5,20));

            }
            else if (Type == "Plusqueparfait")
            {
                Generateur.LabyrinthePlusQueParfait(ref me, entreX, entreY);
            }
            int tailleDivisee = (int)Math.Round((double)Taille / 4);
            SetRandomExitDifficulties(1+random.Next(0,tailleDivisee),entre);
        }

        private void SetRandomExitDifficulties(int numberexit, int entre)
        {
            Random random = new Random();
            int cellCount = Cellules.Count;

            for (int i = 0; i < numberexit; i++)
            {
                int randomIndex = random.Next(0, cellCount);
                if (randomIndex == entre)
                {
                    i--;
                }
                else
                {
                    Cellules[randomIndex].DifficulteSortie = random.Next(1, 6);
                }
            }
        }
    }
}