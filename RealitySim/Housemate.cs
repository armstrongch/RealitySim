using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal class Housemate
    {
        public string Name { get; private set; }
        public int Karma { get; private set; }
        public int Cash { get; private set; }
        public LOCATION currentLocation;
        public Dictionary<Housemate, int> Opinions { get; private set; }
        public bool Awake { get; private set; };

        public Housemate(string name, LOCATION currentLocation)
        {
            this.Name = name;
            this.currentLocation = currentLocation;
            this.Karma = 0;
            this.Cash = 500;
            this.Awake = true;
        }
    }
}
