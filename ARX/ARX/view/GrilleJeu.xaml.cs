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
        private int playerHealth;
        public Labyrinthe labyActuel;
        private InventoryWindow inventory;
        private Personnage joueur;
        private CombatWindow combatWindow;
        private Vendeur vendeur;
        private Arme arme;
        private bool fogofwar = false;
        private int Difficulte;
        private int Profondeur;


        public GrilleJeu(Personnage joueur, InventoryWindow inventory, int difficulte, int profondeur)
        {
            InitializeComponent();
            labyActuel = new Labyrinthe();
            labyActuel.Initialize(profondeur, difficulte, fogofwar);
            GenerateGrid(labyActuel);
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.Focusable = true;
            this.Focus();

            this.joueur = joueur;

            arme = joueur.Armes;

            this.inventory = inventory;

            vendeur = new Vendeur(inventory);

            this.DataContext = new
            {
                joueur = joueur,
                labyrinthe = labyActuel
            };
            Difficulte = difficulte;
            Profondeur = profondeur;
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
                case Key.E:
                    inventory.UpdatePlayerData(joueur);
                    inventory.ShowInventory();
                    break;
            }
        }

        private void MovePlayer(int dx, int dy)
        {
            var currentCell = labyActuel.Cellules.Find(c => c.Joueur);
            if (currentCell == null)
                return;

            int newX = currentCell.X + dx;
            int newY = currentCell.Y + dy;

            if (newX >= 0 && newX < labyActuel.Taille && newY >= 0 && newY < labyActuel.Taille)
            {
                var newCell = labyActuel.Cellules.Find(c => c.X == newX && c.Y == newY);
                if (newCell != null && CanMoveTo(currentCell, newCell, dx, dy))
                {
                    if (dx == 1) currentCell.JoueurOrientation = 180; // Sud
                    else if (dx == -1) currentCell.JoueurOrientation = 0; // Nord
                    else if (dy == 1) currentCell.JoueurOrientation = 90; // Est
                    else if (dy == -1) currentCell.JoueurOrientation = -90; // Ouest

                    currentCell.Joueur = false;
                    newCell.Joueur = true;
                    newCell.JoueurOrientation = currentCell.JoueurOrientation;
                    RefreshGrid();

                    if (newCell.EnemyInCell != null)
                    {
                        
                    }

                    if (newCell.EnemyInCell != null)
                    {
                        CombatWindow combatWindow = new CombatWindow(joueur, newCell.EnemyInCell, inventory, arme);
                        combatWindow.PlayerDied += CombatWindow_PlayerDied;
                        combatWindow.EnemyDefeated += CombatWindow_EnemyDefeated;
                        combatWindow.ShowDialog();

                        playerHealth = joueur.Vie;
                    }
                    if (newCell.DifficulteSortie != 0)
                    {
                        labyActuel = new Labyrinthe();
                        labyActuel.Initialize(Profondeur, Difficulte, fogofwar);
                        GenerateGrid(labyActuel);
                        vendeur = new Vendeur(inventory);
                        vendeur.Show();
                        vendeur.Focus();

                    }
                }
            }
        }

        private bool CanMoveTo(Cellule currentCell, Cellule newCell, int dx, int dy)
        {
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

            int totalCells = size * size;

            for (int i = 0; i < size; i++)
            {
                CellGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                CellGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < totalCells; i++)
            {
                int row = i / size;
                int column = i % size;

                var cellule = labyrinthe.Cellules[i];

                Grid cellGrid = new Grid();

                Image cellImage = new Image();
                string imagePath = cellule.Fond;
                Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                BitmapImage bitmap = new BitmapImage(imageUri);
                cellImage.Source = bitmap;
                cellImage.Stretch = Stretch.Fill;
                cellGrid.Children.Add(cellImage);

                if (cellule.Joueur)
                {
                    Image cellImageJoueur = new Image();
                    string imagePathJoueur = cellule.Joueur_top;
                    Uri imageUriJoueur = new Uri(imagePathJoueur, UriKind.Absolute);
                    BitmapImage bitmapJoueur = new BitmapImage(imageUriJoueur);
                    cellImageJoueur.Source = bitmapJoueur;
                    cellImageJoueur.Stretch = Stretch.Fill;

                    cellImageJoueur.RenderTransformOrigin = new Point(0.5, 0.5);

                    RotateTransform rotateTransform = new RotateTransform(cellule.JoueurOrientation);
                    cellImageJoueur.RenderTransform = rotateTransform;

                    cellGrid.Children.Add(cellImageJoueur);
                }
                if (cellule.EnemyInCell != null)
                {
                    Enemy enemy = cellule.EnemyInCell;
                    Image enemyImage = new Image();

                    string enemyImagePath = $"pack://application:,,,/ARX;component/view/Images/enemy/{enemy.Type}{enemy.IndexImage}top.png";

                    Uri enemyImageUri = new Uri(enemyImagePath, UriKind.Absolute);
                    BitmapImage enemybitmap = new BitmapImage(enemyImageUri);
                    enemyImage.Source = enemybitmap;

                    double cellSize = CellGrid.ActualWidth / labyActuel.Taille;

                    enemyImage.Width = cellSize * 0.5;
                    enemyImage.Height = cellSize * 0.5;
                    enemyImage.Stretch = Stretch.Uniform;

                    cellGrid.Children.Add(enemyImage);
                }
                if (cellule.DifficulteSortie > 0 && cellule.DifficulteSortie < 6)
                {
                    int valeur = cellule.DifficulteSortie;
                    Image enemyImage = new Image();

                    string stairsImagePath = $"pack://application:,,,/ARX;component/view/Images/stairs{valeur}.png";

                    Uri stairsImageUri = new Uri(stairsImagePath, UriKind.Absolute);
                    BitmapImage stairsbitmap = new BitmapImage(stairsImageUri);
                    enemyImage.Source = stairsbitmap;

                    double cellSize = CellGrid.ActualWidth / labyActuel.Taille;
                    enemyImage.Stretch = Stretch.Uniform;

                    cellGrid.Children.Add(enemyImage);
                }

                AddWallsToCell(cellule, cellGrid);

                CellGrid.Children.Add(cellGrid);
                Grid.SetRow(cellGrid, row);
                Grid.SetColumn(cellGrid, column);
            }
        }

        private void AddWallsToCell(Cellule cellule, Grid cellGrid)
        {
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

            wallImage.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform rotateTransform = new RotateTransform(angle);
            wallImage.RenderTransform = rotateTransform;

            cellGrid.Children.Add(wallImage);
        }

        private void QuitterButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.CloseGridJeu();
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

        private void CombatWindow_PlayerDied(object sender, EventArgs e)
        {
            CloseGridJeu();
        }

        private void CloseGridJeu()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
        private void CombatWindow_EnemyDefeated(object sender, EventArgs e)
        {
            CombatWindow combatWindow = sender as CombatWindow;
            if (combatWindow != null)
            {
                var currentCell = labyActuel.Cellules.Find(c => c.Joueur);
                if (currentCell != null && currentCell.EnemyInCell != null)
                {
                    if (currentCell.EnemyInCell.Stuff != null)
                    {
                        currentCell.loot.CombineLoot(currentCell.EnemyInCell.Stuff);
                    }
                    currentCell.EnemyInCell = null;

                    RefreshGrid();
                }
            }
        }
    }
}