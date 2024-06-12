using System;
using System.Diagnostics;
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

            // Calculate the total number of cells in the grid
            int totalCells = size * size;

            for (int i = 0; i < size; i++)
            {
                CellGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Iterate over the total number of cells
            for (int i = 0; i < totalCells; i++)
            {
                int row = i / size; // Calculate the row index
                int column = i % size; // Calculate the column index

                var cellule = labyrinthe.Cellules[i]; // Get the cell corresponding to the sequential order

                Grid cellGrid = new Grid();

                // Affichage du nombre en arrière-plan
                TextBlock nombreText = new TextBlock()
                {
                    Text = cellule.Nombre.ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 0.5 // Opacité réduite pour que le texte soit visible mais ne masque pas les autres éléments
                };
                cellGrid.Children.Add(nombreText);

                AddWallsToCell(cellule, cellGrid);

                // Ensure the cell is placed at the correct X and Y coordinates
                CellGrid.Children.Add(cellGrid);
                Grid.SetRow(cellGrid, row);
                Grid.SetColumn(cellGrid, column);
            }
        }



        private void AddWallsToCell(Cellule cellule, Grid cellGrid)
        {
            // Affichage des murs en utilisant les images spécifiées dans la propriété Mur de chaque cellule
            if (cellule.NorthWall)
            {
                AddWallImage(cellule.Mur, 0, cellGrid);
            }

            if (cellule.SouthWall)
            {
                AddWallImage(cellule.Mur, 180, cellGrid);
            }

            if (cellule.EastWall)
            {
                AddWallImage(cellule.Mur, 90, cellGrid);
            }

            if (cellule.WestWall)
            {
                AddWallImage(cellule.Mur, -90, cellGrid);
            }
        }


        private void AddWallImage(string imagePath, double angle, Grid cellGrid)
        {
            Image wallImage = new Image();
            Uri imageUri = new Uri(imagePath, UriKind.Absolute);
            BitmapImage bitmap = new BitmapImage(imageUri);
            wallImage.Source = bitmap;
            wallImage.Stretch = Stretch.Fill;

            // Définir le point d'origine de la rotation au centre de l'image
            wallImage.RenderTransformOrigin = new Point(0.5, 0.5);

            // Appliquer la rotation
            RotateTransform rotateTransform = new RotateTransform(angle);
            wallImage.RenderTransform = rotateTransform;

            cellGrid.Children.Add(wallImage); // Ajout de l'image directement au conteneur de la cellule (Grid)
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
