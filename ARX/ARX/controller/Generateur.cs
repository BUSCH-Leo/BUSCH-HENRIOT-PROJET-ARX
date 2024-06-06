using System;
using System.Collections.Generic;

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

public class Program
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

    public static int VoisinRandom(List<List<bool>> matrice_adjacence, List<bool> visite, int cellule, int taille)
    {
        List<int> cellules = new List<int>();

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

    public static int RandomProfondeur(List<List<bool>> matrice_adjacence, List<bool> visite, int cellule, int taille)
    {
        visite[cellule] = true;
        int voisin = VoisinRandom(matrice_adjacence, visite, cellule, taille);
        while (voisin != -1)
        {
            matrice_adjacence[cellule][voisin] = true;
            matrice_adjacence[voisin][cellule] = true;
            RandomProfondeur(matrice_adjacence, visite, voisin, taille);
            voisin = VoisinRandom(matrice_adjacence, visite, cellule, taille);
        }
        return cellule;
    }

    public static int LabyrintheParfait(List<List<bool>> matrice_adjacence, int taille, int entreX, int entreY)
    {
        int cellule = (entreX + entreY * taille);
        List<bool> visite = new List<bool>(new bool[taille * taille]);
        RandomProfondeur(matrice_adjacence, visite, cellule, taille);
        return cellule;
    }

    public static int LabyrinthePlusQueParfait(List<List<bool>> matrice_adjacence, int taille, int entreX, int entreY)
    {
        int cellule1 = (entreX + entreY * taille);
        List<bool> visite1 = new List<bool>(new bool[taille * taille]);
        RandomProfondeur(matrice_adjacence, visite1, cellule1, taille);
        int cellule2 = (entreX + entreY * taille);
        List<bool> visite2 = new List<bool>(new bool[taille * taille]);
        RandomProfondeur(matrice_adjacence, visite2, cellule2, taille);
        return cellule2;
    }

    public static int LabyrintheImparfait(List<List<bool>> matrice_adjacence, int taille, int entreX, int entreY, float nbtrou)
    {
        nbtrou = (taille * taille) * (nbtrou / 100);
        int cellule = (entreX + entreY * taille);
        List<bool> visite = new List<bool>(new bool[taille * taille]);
        RandomProfondeur(matrice_adjacence, visite, cellule, taille);
        int voisin;
        int numrand;
        List<bool> visitevide = new List<bool>(new bool[taille * taille]);
        int y = 0;
        for (int i = 0; i < nbtrou; i++)
        {
            Random rnd = new Random();
            numrand = rnd.Next(0, taille * taille);
            voisin = VoisinRandom(matrice_adjacence, visitevide, numrand, taille);
            if (matrice_adjacence[numrand][voisin] == true)
            {
                i--;
                y++;
                if (y >= 1000) { break; }
            }
            else
            {
                matrice_adjacence[numrand][voisin] = true;
                matrice_adjacence[voisin][numrand] = true;
                y = 0;
            }
        }
        return cellule;
    }

    public static Loot[,] PlaceLoot(List<List<bool>> matrice_adjacence, int taille, int proba)
    {
        Loot[,] LootsMatrix = new Loot[taille, taille];

        for (int i = 0; i < taille; ++i)
        {
            for (int j = 0; j < taille; ++j)
            {
                Random rnd = new Random();
                int randType = rnd.Next(0, 100);
                int randValue = rnd.Next(1, 9);    // Valeur à changer au besoin
                if (randType < proba * 50 / 100) // 50%
                {
                    LootsMatrix[i, j] = new Loot();
                    LootsMatrix[i, j].type = "Ennemi";
                    LootsMatrix[i, j].value = randValue * 2; // Degats entre 2 et 20
                }
                else if (randType > proba * 50 / 100 && randType <= (proba * 50 / 100 + proba * 25 / 100)) // 25 %
                {
                    LootsMatrix[i, j] = new Loot();
                    LootsMatrix[i, j].type = "Soin";
                    LootsMatrix[i, j].value = randValue;
                }
                else if (randType > (proba * 75 / 100) && randType <= proba) // 25 %
                {
                    LootsMatrix[i, j] = new Loot();
                    LootsMatrix[i, j].type = "Tresor";
                    LootsMatrix[i, j].value = randValue;
                }
            }
        }
        return LootsMatrix;
    }
}
