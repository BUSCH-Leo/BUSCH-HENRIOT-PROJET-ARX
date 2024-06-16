using ARX.view;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ARX
{
    public partial class MainWindow : Window
    {
        public int taille = 10;
        public string type = "imparfait";
        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/view/Images/ARX.ico"));
        }

        private void JouerButton_Click(object sender, RoutedEventArgs e)
        {
            GrilleJeu grilleJeu = new GrilleJeu();
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
    }
}
