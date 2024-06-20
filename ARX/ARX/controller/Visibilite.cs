using System;
using System.Collections.Generic;
using ARX.model;

namespace ARX.controller
{
    public class Visibilite
    {
        public List<bool> CalculerVisibilite(Labyrinthe labyrinthe, int entreX, int entreY)
        {
            int taille = labyrinthe.Taille;
            List<List<bool>> matriceAdjacence = labyrinthe.MatriceAdjacence;

            int cellule = (entreX + entreY * taille);
            List<bool> vue = new List<bool>(new bool[taille * taille]);
            vue[cellule] = true;

            CalculerDirectionSud(matriceAdjacence, taille, cellule, vue);
            CalculerDirectionNord(matriceAdjacence, taille, cellule, vue);
            CalculerDirectionOuest(matriceAdjacence, taille, cellule, vue);
            CalculerDirectionEst(matriceAdjacence, taille, cellule, vue);

            NordEst(entreX, entreY, taille, vue, matriceAdjacence);
            SudEst(entreX, entreY, taille, vue, matriceAdjacence);
            SudOuest(entreX, entreY, taille, vue, matriceAdjacence);
            NordOuest(entreX, entreY, taille, vue, matriceAdjacence);

            return vue;
        }

        private void CalculerDirectionSud(List<List<bool>> matriceAdjacence, int taille, int cellule, List<bool> vue)
        {
            while (cellule - taille >= 0 && matriceAdjacence[cellule][cellule - taille])
            {
                cellule = cellule - taille;
                vue[cellule] = true;
            }
        }

        private void CalculerDirectionNord(List<List<bool>> matriceAdjacence, int taille, int cellule, List<bool> vue)
        {
            while (cellule + taille < taille * taille && matriceAdjacence[cellule][cellule + taille])
            {
                cellule = cellule + taille;
                vue[cellule] = true;
            }
        }

        private void CalculerDirectionOuest(List<List<bool>> matriceAdjacence, int taille, int cellule, List<bool> vue)
        {
            while (cellule % taille != 0 && matriceAdjacence[cellule][cellule - 1])
            {
                cellule = cellule - 1;
                vue[cellule] = true;
            }
        }

        private void CalculerDirectionEst(List<List<bool>> matriceAdjacence, int taille, int cellule, List<bool> vue)
        {
            while ((cellule + 1) % taille != 0 && matriceAdjacence[cellule][cellule + 1])
            {
                cellule = cellule + 1;
                vue[cellule] = true;
            }
        }

        private void NordEst(int entreX, int entreY, int taille, List<bool> vue, List<List<bool>> matriceAdjacence)
        {
            int cellule = (entreX + entreY * taille);
            if (cellule - taille >= 0 && (cellule + 1) % taille != 0 &&
                matriceAdjacence[cellule][cellule - taille] &&
                matriceAdjacence[cellule][cellule + 1] &&
                matriceAdjacence[cellule - taille][cellule - taille + 1] &&
                matriceAdjacence[cellule + 1][cellule - taille + 1] &&
                !vue[cellule - taille + 1])
            {
                cellule = (cellule - taille) + 1;
                vue[cellule] = true;
                NordEst(entreX + 1, entreY, taille, vue, matriceAdjacence);
                NordEst(entreX, entreY - 1, taille, vue, matriceAdjacence);
            }
        }

        private void SudEst(int entreX, int entreY, int taille, List<bool> vue, List<List<bool>> matriceAdjacence)
        {
            int cellule = (entreX + entreY * taille);
            if (cellule + taille < taille * taille && (cellule + 1) % taille != 0 &&
                matriceAdjacence[cellule][cellule + taille] &&
                matriceAdjacence[cellule][cellule + 1] &&
                matriceAdjacence[cellule + taille][cellule + taille + 1] &&
                matriceAdjacence[cellule + 1][cellule + taille + 1] &&
                !vue[cellule + taille + 1])
            {
                cellule = (cellule + taille) + 1;
                vue[cellule] = true;
                SudEst(entreX + 1, entreY, taille, vue, matriceAdjacence);
                SudEst(entreX, entreY + 1, taille, vue, matriceAdjacence);
            }
        }

        private void SudOuest(int entreX, int entreY, int taille, List<bool> vue, List<List<bool>> matriceAdjacence)
        {
            int cellule = (entreX + entreY * taille);
            if (cellule + taille < taille * taille && cellule % taille != 0 &&
                matriceAdjacence[cellule][cellule + taille] &&
                matriceAdjacence[cellule][cellule - 1] &&
                matriceAdjacence[cellule + taille][cellule + taille - 1] &&
                matriceAdjacence[cellule - 1][cellule + taille - 1] &&
                !vue[(cellule + taille) - 1])
            {
                cellule = (cellule + taille) - 1;
                vue[cellule] = true;
                SudOuest(entreX - 1, entreY, taille, vue, matriceAdjacence);
                SudOuest(entreX, entreY + 1, taille, vue, matriceAdjacence);
            }
        }

        private void NordOuest(int entreX, int entreY, int taille, List<bool> vue, List<List<bool>> matriceAdjacence)
        {
            int cellule = (entreX + entreY * taille);
            if (cellule - taille >= 0 && cellule % taille != 0 &&
                matriceAdjacence[cellule][cellule - taille] &&
                matriceAdjacence[cellule][cellule - 1] &&
                matriceAdjacence[cellule - taille][cellule - taille - 1] &&
                matriceAdjacence[cellule - 1][cellule - taille - 1] &&
                !vue[(cellule - taille) - 1])
            {
                cellule = (cellule - taille) - 1;
                vue[cellule] = true;
                NordOuest(entreX - 1, entreY, taille, vue, matriceAdjacence);
                NordOuest(entreX, entreY - 1, taille, vue, matriceAdjacence);
            }
        }
    }
}
