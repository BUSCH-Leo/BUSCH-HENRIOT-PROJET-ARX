using ARX.view;
using System.Windows;

namespace ARX
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void JouerButton_Click(object sender, RoutedEventArgs e)
        {
            GrilleJeu grilleJeu = new GrilleJeu();
            this.Content = grilleJeu;
        }

        private void ChargerButton_Click(object sender, RoutedEventArgs e)
        {
            // Code pour charger une partie précédente
        }

        private void QuitterButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
