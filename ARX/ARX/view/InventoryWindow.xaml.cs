using System;
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

        public InventoryWindow(Personnage joueur)
        {
            InitializeComponent();

            InventoryItems = new ObservableCollection<Item>();

            var settings = Settings.Load();
            Pseudo = settings.Pseudo;

            Vie = joueur.Vie;
            VieMax = joueur.VieMax;
            Dexterite = joueur.Dexterite;
            Force = joueur.Force;

            this.DataContext = new
            {
                Settings = settings,
                InventoryItems = InventoryItems,
                Vie = Vie,
                VieMax = VieMax,
                Dexterite = Dexterite,
                Force = Force,
                Personnage = joueur
            };
        }

        public void InitializeInventory()
        {
            InventoryItems.Add(new Item { Name = "Un suce", Price = 69, IsSelected = false });
            InventoryItems.Add(new Item { Name = "Potion de vie (+10)", Type = "Potion", Price = 69, EffectValue = 10, IsSelected = false });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                this.Close();
            }
        }

        public void AddItem(Item newItem)
        {
            InventoryItems.Add(newItem);
        }

        public void RemoveItem(Item itemToRemove)
        {
            InventoryItems.Remove(itemToRemove);
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public int EffectValue { get; set; }
        public bool IsSelected { get; set; }
    }
}