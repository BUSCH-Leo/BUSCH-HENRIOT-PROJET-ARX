using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ARX.controller;
using ARX.model;

namespace ARX.view
{
    public partial class Vendeur : Window
    {
        public ObservableCollection<Item> InventoryItems { get; set; }
        public ObservableCollection<Item> ShopItems { get; set; }

        public InventoryWindow inventory;

        public Vendeur(InventoryWindow inventory)
        {
            InitializeComponent();

            InventoryItems = inventory.InventoryItems;

            ShopItems = Item.GenerateRandomItems(5);

            InventoryList.ItemsSource = InventoryItems;
            ShopList.ItemsSource = ShopItems;

            this.DataContext = new
            {
                InventoryItems = InventoryItems,
                ShopItems = ShopItems
            };
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            UpdateTotalPrices();
        }

        private void UpdateTotalPrices()
        {
            double totalAchat = ShopItems.Where(item => item.IsSelected).Sum(item => item.Price);
            double totalVente = InventoryItems.Where(item => item.IsSelected).Sum(item => item.Price);

            TotalBuyPrice.Text = totalAchat.ToString("F2");
            TotalSellPrice.Text = totalVente.ToString("F2");
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour la vente d'objets sélectionnés dans l'inventaire
            var itemsToSell = InventoryItems.Where(item => item.IsSelected).ToList();
            foreach (var item in itemsToSell)
            {
                InventoryItems.Remove(item);
            }
            UpdateTotalPrices();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            // Logique pour l'achat d'objets sélectionnés dans le magasin
            var itemsToBuy = ShopItems.Where(item => item.IsSelected).ToList();
            foreach (var item in itemsToBuy)
            {
                ShopItems.Remove(item);
            }
            UpdateTotalPrices();
        }
    }
}
