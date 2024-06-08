using System;
using ARX.model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

namespace ARX.model
{
    public class Loot
    {
        public string LootType { get; set; }

        public Loot(string lootType)
        {
            LootType = lootType;
        }
    }

    public class Objets
    {
        public string Type { get; set; }
        public int Value { get; set; }

        public Objets(string type, int value)
        {
            Type = type;
            Value = value;
        }
    }

    public class Money
    {
        public int Value { get; set; }
        public Personnage Personnage { get; set; }

        public void AddMoney(int value)
        {
            Value += value;
        }

        public void Buy()
        {
            // Logique d'achat ici
        }
    }

    public class Armes
    {
        public string Type { get; set; }
        public int DegatsMin { get; set; }
        public int DegatsMax { get; set; }
        public int Probabilite { get; set; }
        public int Penetration { get; set; }
        public Effets Effets { get; set; }

        public Armes(string type, int degatsMin, int degatsMax, int probabilite, int penetration, Effets effets)
        {
            Type = type;
            DegatsMin = degatsMin;
            DegatsMax = degatsMax;
            Probabilite = probabilite;
            Penetration = penetration;
            Effets = effets;
        }
    }

}
