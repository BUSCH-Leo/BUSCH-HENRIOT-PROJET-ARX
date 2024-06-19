using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ARX.model;

namespace ARX.view
{
    public partial class CombatWindow : Window
    {
        private Random random = new Random();
        private int playerHealth;
        private int maxHealth;
        private int EnnemiVie;
        private string EnnemiNom;
        private int EnnemiDegMin;
        private int EnnemiDegMax;
        private InventoryWindow inventory;
        private Personnage joueur;
        private Enemy enemy;

        public event EventHandler PlayerDied;
        public event EventHandler EnemyDefeated;

        private int degamin;
        private int degamax;

        public CombatWindow(Personnage joueur, Enemy ennemi, InventoryWindow inventory, Arme arme)
        {
            InitializeComponent();
            LoadImages();
            this.joueur = joueur;
            this.enemy = ennemi;
            this.inventory = inventory;
            playerHealth = joueur.Vie;
            maxHealth = joueur.VieMax;
            EnnemiNom = ennemi.Nom;
            EnnemiVie = ennemi.Vie;
            EnnemiDegMin = ennemi.degaMin;
            EnnemiDegMax = ennemi.degaMax;

            degamin = arme.DegatsMin;
            degamax = arme.DegatsMax;

            InitializeCombat();
        }

        private void LoadImages()
        {
            BackgroundImage.Source = new BitmapImage(new Uri("Images/maze.png", UriKind.Relative));
            EnemyImage.Source = new BitmapImage(new Uri("Images/creature.png", UriKind.Relative));
        }

        private void InitializeCombat()
        {
            UpdateHealthDisplays();
            EnemyNameText.Text = $"Ennemi : {EnnemiNom}";
        }

        private void UpdateHealthDisplays()
        {
            PlayerHealthText.Text = $"Player Health: {playerHealth}";
            PlayerHealthText.Foreground = Brushes.White;
            EnemyHealthText.Text = $"Enemy Health: {EnnemiVie}";
            EnemyHealthText.Foreground = Brushes.White;
        }

        private void PlayerAttack()
        {
            int damage = random.Next(degamin, degamax + 1);
            EnnemiVie -= damage;
            if (EnnemiVie < 0) EnnemiVie = 0;
            MessageBox.Show($"You dealt {damage} damage to the enemy!");
            UpdateHealthDisplays();

            if (EnnemiVie > 0)
            {
                EnemyAttack();
            }
            else
            {
                MessageBox.Show("You defeated the enemy!");
                EnemyDefeated?.Invoke(this, EventArgs.Empty);
                joueur.Vie = playerHealth;
                Close();
            }
        }

        private void EnemyAttack()
        {
            int damage = random.Next(EnnemiDegMin, EnnemiDegMax + 1);
            playerHealth -= damage;
            if (playerHealth < 0) playerHealth = 0;
            MessageBox.Show($"The enemy dealt {damage} damage to you!");
            UpdateHealthDisplays();

            if (playerHealth <= 0)
            {
                MessageBox.Show("You have been defeated!");
                PlayerDied?.Invoke(this, EventArgs.Empty);
                ReturnToMainWindow();
            }
        }

        private void ReturnToMainWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerAttack();
        }

        private void FleeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You fled from the battle!");
            joueur.Vie = playerHealth;
            Close();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            InventoryWindowCombat inventoryWindowCombat = new InventoryWindowCombat(playerHealth, maxHealth, inventory);
            inventoryWindowCombat.Owner = this;
            inventoryWindowCombat.ShowDialog();
            playerHealth = inventoryWindowCombat.PlayerHealth;
            UpdateHealthDisplays();
        }
    }
}
