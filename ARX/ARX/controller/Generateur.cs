using ARX.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace ARX.controller
{
    public class Loot
    {
        public string type;
        public int value;
        public int alt_value;

        public Loot()
        {
            type = "none";
            value = 0;
            alt_value = 0;
        }
    }

    public class Generateur
    {
        public static Loot[,] CopyLootMatrix(Loot[,] original, int taille)
        {
            Loot[,] copy = new Loot[taille, taille];
            for (int i = 0; i < taille; ++i)
            {
                for (int j = 0; j < taille; ++j)
                {
                    copy[i, j] = original[i, j];
                }
            }
            return copy;
        }

        public static int VoisinRandom(ref Labyrinthe laby, List<bool> visite, int cellule)
        {
            List<int> cellules = new List<int>();
            int taille = laby.Taille;
            List<List<bool>> matrice_adjacence = laby.MatriceAdjacence;
            List<Cellule> cellulesList = laby.Cellules;

            if (cellule - taille >= 0 && !visite[cellule - taille])
            {
                cellules.Add(cellule - taille);
            }

            if (cellule + taille < taille * taille && !visite[cellule + taille])
            {
                cellules.Add(cellule + taille);
            }

            if (cellule % taille != 0 && !visite[cellule - 1])
            {
                cellules.Add(cellule - 1);
            }

            if ((cellule + 1) % taille != 0 && !visite[cellule + 1])
            {
                cellules.Add(cellule + 1);
            }

            if (cellules.Count == 0)
            {
                return -1;
            }

            Random rnd = new Random();
            int numrand = rnd.Next(0, cellules.Count);
            int voisin = cellules[numrand];
            return voisin;
        }

        public static void LiaisonCellules(ref Labyrinthe laby, int cellule1, int cellule2)
        {
            int taille = laby.Taille;
            int x1 = cellule1 % taille;
            int y1 = cellule1 / taille;
            int x2 = cellule2 % taille;
            int y2 = cellule2 / taille;

            if (x1 == x2)
            {
                if (y1 < y2)
                {
                    laby.Cellules[cellule1].SouthWall = false;
                    laby.Cellules[cellule2].NorthWall = false;
                }
                else
                {
                    laby.Cellules[cellule1].NorthWall = false;
                    laby.Cellules[cellule2].SouthWall = false;
                }
            }
            else
            {
                if (x1 < x2)
                {
                    laby.Cellules[cellule1].EastWall = false;
                    laby.Cellules[cellule2].WestWall = false;
                }
                else
                {
                    laby.Cellules[cellule1].WestWall = false;
                    laby.Cellules[cellule2].EastWall = false;
                }
            }

            // Ajout de messages de débogage
            Trace.WriteLine($"Cellule1: ({x1}, {y1}), Cellule2: ({x2}, {y2})");
            Trace.WriteLine($"Murs Cellule1: N:{laby.Cellules[cellule1].NorthWall}, S:{laby.Cellules[cellule1].SouthWall}, E:{laby.Cellules[cellule1].EastWall}, W:{laby.Cellules[cellule1].WestWall}");
            Trace.WriteLine($"Murs Cellule2: N:{laby.Cellules[cellule2].NorthWall}, S:{laby.Cellules[cellule2].SouthWall}, E:{laby.Cellules[cellule2].EastWall}, W:{laby.Cellules[cellule2].WestWall}");
        }

        public static int RandomProfondeur(ref Labyrinthe laby, ref List<bool> visite, int cellule)
        {
            visite[cellule] = true;
            int voisin = VoisinRandom(ref laby, visite, cellule);
            while (voisin != -1)
            {
                laby.MatriceAdjacence[cellule][voisin] = true;
                laby.MatriceAdjacence[voisin][cellule] = true;
                LiaisonCellules(ref laby, cellule, voisin);
                RandomProfondeur(ref laby, ref visite, voisin);
                voisin = VoisinRandom(ref laby, visite, cellule);
            }
            return cellule;
        }

        public static int LabyrintheParfait(ref Labyrinthe laby, int entreX, int entreY)
        {
            int cellule = (entreX + entreY * laby.Taille);
            List<bool> visite = new List<bool>(new bool[laby.Taille * laby.Taille]);
            RandomProfondeur(ref laby, ref visite, cellule);
            return cellule;
        }

        public static int LabyrinthePlusQueParfait(ref Labyrinthe laby, int entreX, int entreY)
        {
            int cellule1 = (entreX + entreY * laby.Taille);
            List<bool> visite1 = new List<bool>(new bool[laby.Taille * laby.Taille]);
            RandomProfondeur(ref laby, ref visite1, cellule1);

            int cellule2 = (entreX + entreY * laby.Taille);
            List<bool> visite2 = new List<bool>(new bool[laby.Taille * laby.Taille]);
            RandomProfondeur(ref laby, ref visite2, cellule2);

            return cellule2;
        }

        public static int LabyrintheImparfait(ref Labyrinthe laby, int entreX, int entreY, float nbtrou)
        {
            nbtrou = (laby.Taille * laby.Taille) * (nbtrou / 100);
            int cellule = (entreX + entreY * laby.Taille);
            List<bool> visite = new List<bool>(new bool[laby.Taille * laby.Taille]);
            RandomProfondeur(ref laby, ref visite, cellule);

            List<bool> visitevide = new List<bool>(new bool[laby.Taille * laby.Taille]);
            int y = 0;
            for (int i = 0; i < nbtrou; i++)
            {
                Random rnd = new Random();
                int numrand = rnd.Next(0, laby.Taille * laby.Taille);
                int voisin = VoisinRandom(ref laby, visitevide, numrand);
                if (laby.MatriceAdjacence[numrand][voisin] == true)
                {
                    i--;
                    y++;
                    if (y >= 1000) { break; }
                }
                else
                {
                    laby.MatriceAdjacence[numrand][voisin] = true;
                    laby.MatriceAdjacence[voisin][numrand] = true;
                    y = 0;
                }
            }
            return cellule;
        }
    }
}