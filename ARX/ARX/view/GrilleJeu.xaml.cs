using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ARX.view
{
    public partial class GrilleJeu : UserControl
    {
        private int taille = 5;

        public GrilleJeu()
        {
            InitializeComponent();
            GenerateGrid(taille);
        }

        private void GenerateGrid(int size)
        {
            CellGrid.RowDefinitions.Clear();
            CellGrid.ColumnDefinitions.Clear();

            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                CellGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            bool[][] adjacencyMatrix = GenerateAdjacencyMatrix();
            AddWallsToGrid(adjacencyMatrix);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Ajouter une cellule d'image
                    Image cellImage = new Image();

                    int randomImageIndex = rand.Next(1, 6);
                    string imagePath = @"C:\Users\tyyyty\Desktop\travail\iut année 1\saé\sae ARX\ARX\ARX\view\Images\test" + randomImageIndex + ".png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                    cellImage.Source = bitmap;

                    cellImage.Stretch = Stretch.Fill;

                    Border cellBorder = new Border
                    {
                        BorderThickness = new Thickness(0),
                        Child = cellImage
                    };

                    CellGrid.Children.Add(cellBorder);
                    Grid.SetRow(cellBorder, i);
                    Grid.SetColumn(cellBorder, j);
                }
            }
        }

        private bool[][] GenerateAdjacencyMatrix()
        {
            bool[][] adjacencyMatrix = new bool[taille][];
            for (int i = 0; i < taille; i++)
            {
                adjacencyMatrix[i] = new bool[taille];
                for (int j = 0; j < taille; j++)
                {
                    adjacencyMatrix[i][j] = true;
                }
            }
            return adjacencyMatrix;
        }

        private void AddWallsToGrid(bool[][] adjacencyMatrix)
        {
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    if (i < taille - 1 && !adjacencyMatrix[i][j])
                    {
                        Border wallBorder = new Border
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 1, 0, 0)
                        };
                        CellGrid.Children.Add(wallBorder);
                        Grid.SetRow(wallBorder, i + 1);
                        Grid.SetColumn(wallBorder, j);
                    }

                    if (j < taille - 1 && !adjacencyMatrix[i][j + 1])
                    {
                        Border wallBorder = new Border
                        {
                            BorderBrush = Brushes.Black,
                            BorderThickness = new Thickness(0, 0, 1, 0)
                        };
                        CellGrid.Children.Add(wallBorder);
                        Grid.SetRow(wallBorder, i);
                        Grid.SetColumn(wallBorder, j + 1);
                    }
                }
            }
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Options :\n1. Sauvegarder\n2. Quitter", "Options", MessageBoxButton.OK);
        }

        // Méthode appelée lors du clic sur le menu "Génération"
        private void GenerationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Régénérer la grille avec une nouvelle matrice d'adjacence
            bool[][] newAdjacencyMatrix = GenerateAdjacencyMatrix();
            // Nettoyer la grille actuelle
            CellGrid.Children.Clear();
            // Regénérer la grille avec les nouveaux murs
            GenerateGrid(taille);
        }
    }
}
