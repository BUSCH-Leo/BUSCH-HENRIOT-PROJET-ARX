using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ARX.model;
using System.IO;


namespace ARX.view
{
    public partial class GrilleJeu : UserControl
    {
        public Labyrinthe labyActuel;

        public GrilleJeu()
        {
            InitializeComponent();
            labyActuel = new Labyrinthe();
            labyActuel.Initialize(10, "imparfait", 1, 10, 50, 1, false, 1, 1);
            GenerateGrid(labyActuel);
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.Focusable = true;
            this.Focus();
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Z:
                case Key.Up:
                    MovePlayer(-1, 0); // Move up
                    break;
                case Key.Q:
                case Key.Left:
                    MovePlayer(0, -1); // Move left
                    break;
                case Key.S:
                case Key.Down:
                    MovePlayer(1, 0); // Move down
                    break;
                case Key.D:
                case Key.Right:
                    MovePlayer(0, 1); // Move right
                    break;
            }
            if (e.Key == Key.E)
            {
                InventoryWindow inventoryWindow = new InventoryWindow();
                inventoryWindow.Show();
            }
        }

        private void MovePlayer(int dx, int dy)
        {
            // Trouver la position actuelle du joueur
            var currentCell = labyActuel.Cellules.Find(c => c.Joueur);
            if (currentCell == null)
                return;

            int newX = currentCell.X + dx;
            int newY = currentCell.Y + dy;

            // Vérifier si la nouvelle position est valide
            if (newX >= 0 && newX < labyActuel.Taille && newY >= 0 && newY < labyActuel.Taille)
            {
                var newCell = labyActuel.Cellules.Find(c => c.X == newX && c.Y == newY);
                if (newCell != null && CanMoveTo(currentCell, newCell, dx, dy))
                {
                    // Mettre à jour l'orientation
                    if (dx == 1) currentCell.JoueurOrientation = 180; // Sud
                    else if (dx == -1) currentCell.JoueurOrientation = 0; // Nord
                    else if (dy == 1) currentCell.JoueurOrientation = 90; // Est
                    else if (dy == -1) currentCell.JoueurOrientation = -90; // Ouest

                    currentCell.Joueur = false;
                    newCell.Joueur = true;
                    newCell.JoueurOrientation = currentCell.JoueurOrientation;
                    RefreshGrid();
                }
            }
        }


        private bool CanMoveTo(Cellule currentCell, Cellule newCell, int dx, int dy)
        {
            // Vérifier les murs
            if (dy == 1 && currentCell.EastWall) return false;
            if (dy == -1 && currentCell.WestWall) return false;
            if (dx == 1 && currentCell.SouthWall) return false;
            if (dx == -1 && currentCell.NorthWall) return false;

            return true;
        }

        private void RefreshGrid()
        {
            CellGrid.Children.Clear();
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

                // Add base image of the cell
                Image cellImage = new Image();
                string imagePath = cellule.Fond;
                Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                BitmapImage bitmap = new BitmapImage(imageUri);
                cellImage.Source = bitmap;
                cellImage.Stretch = Stretch.Fill;
                cellGrid.Children.Add(cellImage);

                // Add player image if player is present in the cell
                if (cellule.Joueur)
                {
                    Image cellImageJoueur = new Image();
                    string imagePathJoueur = cellule.Joueur_top;
                    Uri imageUriJoueur = new Uri(imagePathJoueur, UriKind.Absolute);
                    BitmapImage bitmapJoueur = new BitmapImage(imageUriJoueur);
                    cellImageJoueur.Source = bitmapJoueur;
                    cellImageJoueur.Stretch = Stretch.Fill;

                    // Définir le point d'origine de la rotation au centre de l'image
                    cellImageJoueur.RenderTransformOrigin = new Point(0.5, 0.5);

                    // Appliquer la rotation
                    RotateTransform rotateTransform = new RotateTransform(cellule.JoueurOrientation);
                    cellImageJoueur.RenderTransform = rotateTransform;

                    cellGrid.Children.Add(cellImageJoueur);
                }
                if (cellule.EnemyInCell != null)
                {
                    Enemy enemy = cellule.EnemyInCell;
                    Image enemyImage = new Image();

                    // Générer le chemin d'accès aux images en fonction de l'index de l'ennemi
                    string enemyImagePath = $"pack://application:,,,/ARX;component/view/Images/enemy/{enemy.Type}{enemy.IndexImage}top.png";

                    // Utiliser l'image top pour l'ennemi
                    Uri enemyImageUri = new Uri(enemyImagePath, UriKind.Absolute);
                    BitmapImage enemybitmap = new BitmapImage(enemyImageUri);
                    enemyImage.Source = enemybitmap;

                    // Taille de la cellule (supposant que chaque cellule a la même taille)
                    double cellSize = CellGrid.ActualWidth / labyActuel.Taille; // Supposant une grille carrée

                    // Définir la taille de l'image de l'ennemi
                    enemyImage.Width = cellSize * 0.5; // Ajuster la taille selon vos besoins
                    enemyImage.Height = cellSize * 0.5; // Ajuster la taille selon vos besoins
                    enemyImage.Stretch = Stretch.Uniform;

                    // Ajouter l'image de l'ennemi à la grille
                    cellGrid.Children.Add(enemyImage);
                }
                if (cellule.DifficulteSortie > 0 && cellule.DifficulteSortie < 6)
                {
                    int valeur = cellule.DifficulteSortie;
                    Image enemyImage = new Image();

                    // Générer le chemin d'accès aux images en fonction de l'index de l'ennemi
                    string stairsImagePath = $"pack://application:,,,/ARX;component/view/Images/stairs{valeur}.png";

                    // Utiliser l'image top pour l'ennemi
                    Uri stairsImageUri = new Uri(stairsImagePath, UriKind.Absolute);
                    BitmapImage stairsbitmap = new BitmapImage(stairsImageUri);
                    enemyImage.Source = stairsbitmap;

                    // Taille de la cellule (supposant que chaque cellule a la même taille)
                    double cellSize = CellGrid.ActualWidth / labyActuel.Taille; // Supposant une grille carrée
                    enemyImage.Stretch = Stretch.Uniform;

                    // Ajouter l'image de l'ennemi à la grille
                    cellGrid.Children.Add(enemyImage);
                }

                // Add walls to the cell
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

        private void QuitterButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
                FileName = "GameSave",
                DefaultExt = ".arxsave",
                Filter = "Fichiers ARXSAVE (*.arxsave)|*.arxsave"
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                await Task.Run(() => File.WriteAllText(filePath, "Game saved here"));

                MessageBox.Show($"Fichier enregistré : {filePath}");
            }
        }
    }
}