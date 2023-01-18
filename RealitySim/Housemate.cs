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
        public LOCATION currentLocation;
        public Dictionary<Housemate, int> Opinions { get; private set; } = new Dictionary<Housemate, int>();
        public string Name { get; private set; }
        public int Karma { get; private set; } = 0;
        public int Cash { get; set; } = 500;
        public bool Awake { get; set; } = true;
        public List<ACTION> ActionHistory { get; private set; }

        public int Energy { get; set; } = HousemateMaxEnergy;

        public Housemate(string name, LOCATION currentLocation)
        {
            this.Name = name;
            this.currentLocation = currentLocation;
        }

        public Action SelectAction(List<Action> availableActions)
        {
            throw new NotImplementedException();
        }

        public Housemate SelectTarget(Action action)
        {
            throw new NotImplementedException();
        }
    }
}
