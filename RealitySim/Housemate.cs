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

        public int? PlayerNum { get; private set; }

        public Housemate(string name, LOCATION currentLocation, int? playerNum)
        {
            this.Name = name;
            this.currentLocation = currentLocation;
            this.PlayerNum = playerNum;
        }

        public void IncrementOpinion(Housemate housemate, int value)
        {
            if (!Opinions.ContainsKey(housemate))
            {
                Opinions.Add(housemate, 0);
            }
            Opinions[housemate] += value;
        }

        public bool HasPositiveOpinionOf(Housemate housemate)
        {
            IncrementOpinion(housemate, 0);
            return Opinions[housemate] >= 0;
        }

        public void ShowInfo(List<(Housemate, RELATIONSHIP)> rels, List<Housemate> nearbyHousemates)
        {
            List<string> housemateInfo = new List<string>();
            housemateInfo.Add($"Name: {Name}");
            housemateInfo.Add($"Cash: ${Cash.ToString()}.00");
            housemateInfo.Add($"Energy: {Energy.ToString()} / {HousemateMaxEnergy.ToString()}");
            housemateInfo.Add($"Current Location: {currentLocation.ToString()}");
            housemateInfo.Add($"Nearby Housemates: {string.Join(", ", nearbyHousemates.Select(h => h.Name).ToList())}");

            string noone = "No One";

            string like = (Karma > 0) ? "like" : "dislike";
            housemateInfo.Add($"Viewers {like} {Name}. (Karma = {Karma.ToString()})");

            string Friends = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.FRIEND).Select(r => r.Item1.Name).ToList());
            //if (Friends == string.Empty) { Friends = noone; }
            
            string Enemies = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.ENEMY).Select(r => r.Item1.Name).ToList());
            //if (Enemies == string.Empty) { Enemies = noone; }
            
            string Likes = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.LIKE_AND_DISLIKED_BY).Select(r => r.Item1.Name).ToList());
            //if (Likes == string.Empty) { Likes = noone; }

            string Dislike = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.DISLIKE_AND_LIKED_BY).Select(r => r.Item1.Name).ToList());
            //if (Dislike == string.Empty) { Dislike = noone; }

            Housemate? SignficantOher = rels.Where(r => r.Item2 == RELATIONSHIP.DATING).FirstOrDefault().Item1;
            string SOName = SignficantOher == null ? noone : SignficantOher.Name;

            housemateInfo.Add($"Significant Other: {SOName}");
            if (Friends.Length > 0) { housemateInfo.Add($"Friends: {Friends}"); }
            if (Enemies.Length > 0) { housemateInfo.Add($"Enemies: {Enemies}"); }
            if (Likes.Length > 0) { housemateInfo.Add($"Likes: {Likes}"); }
            if (Dislike.Length > 0) { housemateInfo.Add($"Dislikes: {Dislike}"); }

            foreach (string s in housemateInfo)
            {
                Console.WriteLine(s);
            }
        }
        
        public Action SelectAction(List<Action> availableActions)
        {
            throw new NotImplementedException();
        }

        public Housemate SelectTarget(Action action)
        {
            throw new NotImplementedException();
        }

        private void BuildRelationshipMatrix()
        {

        }
    }
}
