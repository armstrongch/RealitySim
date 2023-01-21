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
        public int Karma { get; set; } = 0;
        public int Cash { get; set; } = 500;
        public bool Awake { get; set; } = true;
        public List<(int,ACTION)> ActionHistory { get; private set; } = new List<(int, ACTION)>();

        public int Energy { get; set; } = HousemateMaxEnergy;

        public Housemate(string name, LOCATION currentLocation)
        {
            this.Name = name;
            this.currentLocation = currentLocation;
        }

        public void IncrementOpinion(Housemate housemate, int value)
        {
            if (!Opinions.ContainsKey(housemate))
            {
                Opinions.Add(housemate, 0);
            }
            Opinions[housemate] += value;
        }

        public int GetOpinion(Housemate housemate)
        {
            IncrementOpinion(housemate, 0);
            return Opinions[housemate];
        }

        public void ShowInfo()
        {
            List<string> housemateInfo = new List<string>();
            housemateInfo.Add($"Name: {Name}");
            housemateInfo.Add($"Cash: ${Cash.ToString()}.00");
            housemateInfo.Add($"Energy: {Energy.ToString()} / {HousemateMaxEnergy.ToString()}");
            housemateInfo.Add($"Current Location: {currentLocation.ToString()}");
            
            string like = (Karma > 0) ? "like" : "dislike";
            housemateInfo.Add($"Viewers {like} {Name}. (Karma = {Karma.ToString()})");

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
