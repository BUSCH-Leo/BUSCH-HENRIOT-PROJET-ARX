using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ARX.model;

namespace ARX.view
{
    public partial class GrilleJeu : UserControl
    {
        public Labyrinthe labyActuel;

        public GrilleJeu()
        {
            InitializeComponent();
            labyActuel = new Labyrinthe();
            labyActuel.Initialize(10, "imparfait", 1, 10, 50, 50, 1, false);
            GenerateGrid(labyActuel);
        }

        private void GenerateGrid(Labyrinthe labyrinthe)
        {
            int size = labyrinthe.Taille;
            CellGrid.RowDefinitions.Clear();
            CellGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < size; i++)
            {
                CellGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            foreach (var cellule in labyrinthe.Cellules)
            {
                Image cellImage = new Image();

                string imagePath = cellule.Fond;
                Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                BitmapImage bitmap = new BitmapImage(imageUri);
                cellImage.Source = bitmap;
                cellImage.Stretch = Stretch.Fill;

                Border cellBorder = new Border
                {
                    BorderThickness = new Thickness(0),
                    Child = cellImage
                };

                CellGrid.Children.Add(cellBorder);
                Grid.SetRow(cellBorder, cellule.Y);
                Grid.SetColumn(cellBorder, cellule.X);

                AddWallsToCell(cellule, cellBorder);
            }
        }

        private void AddWallsToCell(Cellule cellule, Border cellBorder)
        {
            if (cellule.NorthWall)
            {
                Border northWall = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 1, 0, 0)
                };
                CellGrid.Children.Add(northWall);
                Grid.SetRow(northWall, cellule.Y);
                Grid.SetColumn(northWall, cellule.X);
            }

            if (cellule.SouthWall)
            {
                Border southWall = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 0, 0, 1)
                };
                CellGrid.Children.Add(southWall);
                Grid.SetRow(southWall, cellule.Y + 1);
                Grid.SetColumn(southWall, cellule.X);
            }

            if (cellule.EastWall)
            {
                Border eastWall = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 0, 1, 0)
                };
                CellGrid.Children.Add(eastWall);
                Grid.SetRow(eastWall, cellule.Y);
                Grid.SetColumn(eastWall, cellule.X + 1);
            }

            if (cellule.WestWall)
            {
                Border westWall = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1, 0, 0, 0)
                };
                CellGrid.Children.Add(westWall);
                Grid.SetRow(westWall, cellule.Y);
                Grid.SetColumn(westWall, cellule.X);
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
            labyActuel.Initialize(10, "imparfait", 1, 10, 50, 50, 1, false);
            // Nettoyer la grille actuelle
            CellGrid.Children.Clear();
            // Regénérer la grille avec les nouveaux murs
            GenerateGrid(labyActuel);
        }
    }
}
