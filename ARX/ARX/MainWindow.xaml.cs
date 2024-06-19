﻿using System;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ARX.model;
using ARX.view;

namespace ARX
{
    public partial class MainWindow : Window
    {
        private Settings settings;
        private Personnage joueur; // ajouter
        private InventoryWindow inventory; // ajouter

        public Arx ARX { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/view/Images/ARX.ico"));
            settings = Settings.Load();
        }

        private void JouerButton_Click(object sender, RoutedEventArgs e) // modifier
        {
            joueur = new Personnage(
                "", // type
                Arme.Randarme(1), // arme
                100, // vie max
                100, // vie
                1, // force
                1, // dexterite
                10 // pognon
            );

            inventory = new InventoryWindow(joueur);
            inventory.InitializeInventory();
            GrilleJeu grilleJeu = new GrilleJeu(joueur, inventory,1,1);
            this.Content = grilleJeu;
        }

        private void ChargerButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
                Filter = "Fichiers ARXSAVE (*.arxsave)|*.arxsave",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MessageBox.Show($"Fichier sélectionné : {filePath}");
            }
        }

        private void QuitterButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(settings);
            settingsWindow.ShowDialog();
        }
    }
}