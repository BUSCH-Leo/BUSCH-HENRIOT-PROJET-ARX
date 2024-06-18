using ARX.model;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ARX
{
    public partial class SettingsWindow : Window
    {
        private Settings settings;

        public SettingsWindow(Settings settings)
        {
            InitializeComponent();
            this.settings = settings;
            this.DataContext = settings;
            ProfileImage.Source = new BitmapImage(Settings.ToAbsoluteUri(settings.ProfileImagePath));
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Sélectionnez une image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                settings.ProfileImagePath = openFileDialog.FileName;
                ProfileImage.Source = new BitmapImage(Settings.ToAbsoluteUri(settings.ProfileImagePath));
            }
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            settings.Save();
            this.Close();
        }
    }
}
