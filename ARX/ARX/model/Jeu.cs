﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARX.model;

namespace ARX.model
{
    public class Arx
    {
        public int Difficulte { get; set; }
        public string Event { get; set; }
        public int Etage { get; set; }
        public int Seed { get; set; }

        public void NewGame(int difficulte, string eventDetail, int etage, int seed)
        {
            Difficulte = difficulte;
            Event = eventDetail;
            Etage = etage;
            Seed = seed;
        }
    }

}