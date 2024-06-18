using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ARX.model;

namespace ARX.view
{
    public partial class InventoryWindow : Window
    {
        public ObservableCollection<Item> InventoryItems { get; set; }
        public string Pseudo { get; set; }
        public int Vie { get; set; }
        public int VieMax { get; set; }
        public int Dexterite { get; set; }
        public int Force { get; set; }

        public InventoryWindow()
        {
            InitializeComponent();

            var settings = Settings.Load();
            Pseudo = settings.Pseudo;

            Personnage personnage = new Personnage("NomDuPersonnage", "ClasseDuPersonnage", null, 100, 100, 1, 1, "spriteTop", "spriteFront", 0);

            Vie = personnage.Vie;
            VieMax = personnage.VieMax;
            Dexterite = personnage.Dexterite;
            Force = personnage.Force;

            InitializeInventory();

            this.DataContext = new
            {
                Settings = settings,
                InventoryItems = InventoryItems,
                Vie = Vie,
                VieMax = VieMax,
                Dexterite = Dexterite,
                Force = Force,
                Personnage = personnage
            };
        }


        private void InitializeInventory()
        {
            InventoryItems = new ObservableCollection<Item>
            {
                new Item { Name = "Objet 1", Price = 10, IsSelected = false },
                new Item { Name = "Objet 2", Price = 5, IsSelected = false },
                new Item { Name = "Objet 3", Price = 15, IsSelected = false },
                new Item { Name = "Un suce", Price = 69, IsSelected = false }
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                this.Close();
            }
        }
    }

    // Définissez votre classe Item ici (elle reste inchangée)
    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }

        public int EffectValue { get; set; }
        public bool IsSelected { get; set; }
    }
}
