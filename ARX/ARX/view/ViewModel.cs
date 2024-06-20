using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ARX.model;

namespace ARX.view
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Item> _inventoryItems;
        private string _pseudo;
        private Personnage _joueur;
        private Labyrinthe _labyrinthe;

        public ObservableCollection<Item> InventoryItems
        {
            get => _inventoryItems;
            set
            {
                _inventoryItems = value;
                OnPropertyChanged();
            }
        }

        public string Pseudo
        {
            get => _pseudo;
            set
            {
                _pseudo = value;
                OnPropertyChanged();
            }
        }

        public Personnage Joueur
        {
            get => _joueur;
            set
            {
                _joueur = value;
                OnPropertyChanged();
            }
        }

        public Labyrinthe Labyrinthe
        {
            get => _labyrinthe;
            set
            {
                _labyrinthe = value;
                OnPropertyChanged();
            }
        }

        public InventoryViewModel(Personnage joueur, Labyrinthe labyrinthe)
        {
            InventoryItems = new ObservableCollection<Item>();
            var settings = Settings.Load();
            Pseudo = settings.Pseudo;
            Joueur = joueur;
            Labyrinthe = labyrinthe;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
