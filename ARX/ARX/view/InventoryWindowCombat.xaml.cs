using System;
using System.Windows;
using System.Windows.Controls;
using ARX.model;

namespace ARX.view
{
    public partial class InventoryWindowCombat : Window
    {
        public int PlayerHealth { get; private set; }
        int MaxHealth { get; set; }
        private InventoryWindow inventory;

        public InventoryWindowCombat(int currentHealth, int maxHealth, InventoryWindow inventory)
        {
            InitializeComponent();
            PlayerHealth = currentHealth;
            MaxHealth = maxHealth;
            this.inventory = inventory;
            InitializeInventoryCombat(inventory);
        }

        private void InitializeInventoryCombat(InventoryWindow inventory)
        {
            foreach (var item in inventory.InventoryItems)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = item.Name,
                    Tag = item
                };
                listBoxItem.Selected += ListBoxItem_Selected;
                InventoryListBox.Items.Add(listBoxItem);
            }
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ListBoxItem)sender;
            var item = (Item)selectedItem.Tag;

            if (item.Type == "Potion")
            {
                PlayerHealth += item.EffectValue;
                if (PlayerHealth > MaxHealth) PlayerHealth = MaxHealth;
                MessageBox.Show($"Tu as utilisé un/une {item.Name} !");

                inventory.RemoveItem(item);
                InventoryListBox.Items.Remove(selectedItem);
                Close();
            }
        }
    }
}
