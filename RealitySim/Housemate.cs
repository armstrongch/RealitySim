using System;
using System.Collections.Generic;
using System.IO;
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
        public int Cash { get; set; } = 150;
        public bool Awake { get; set; } = true;
        public List<(int,ACTION)> ActionHistory { get; private set; } = new List<(int, ACTION)>();
        public List<WitnessedInfidelity> WitnessedInfidelities { get; private set; } = new List<WitnessedInfidelity>();

        public int Energy { get; set; } = HousemateMaxEnergy;

        public int? PlayerNum { get; private set; }

        Random rand = new Random();

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
            return GetOpinionOf(housemate) >= 0;
        }

        public int GetOpinionOf(Housemate housemate)
        {
            IncrementOpinion(housemate, 0);
            return Opinions[housemate];
        }

        public void ShowInfo(List<(Housemate, RELATIONSHIP)> rels, List<Housemate> nearbyHousemates)
        {
            List<string> housemateInfo = new List<string>();
            housemateInfo.Add($"Name: {Name}");
            housemateInfo.Add($"Cash: {Cash.ToString()}.00");
            housemateInfo.Add($"Energy: {Energy.ToString()} / {HousemateMaxEnergy.ToString()}");
            housemateInfo.Add($"Current Location: {currentLocation.ToString()}");
            housemateInfo.Add($"Nearby Housemates: {string.Join(", ", nearbyHousemates.Select(h => h.Name).ToList())}");

            string noone = "No One";

            string like = (Karma > 0) ? "like" : "dislike";
            housemateInfo.Add($"Viewers {like} {Name} (Karma = {Karma.ToString()})");

            string Friends = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.FRIEND).Select(r => r.Item1.Name).ToList());
            
            string Enemies = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.ENEMY).Select(r => r.Item1.Name).ToList());
            
            string Likes = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.LIKE_AND_DISLIKED_BY).Select(r => r.Item1.Name).ToList());

            string Dislike = string.Join(", ", rels.Where(r => r.Item2 == RELATIONSHIP.DISLIKE_AND_LIKED_BY).Select(r => r.Item1.Name).ToList());

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
            Action selectedAction = availableActions.OrderBy(a => rand.Next()).First();
            return selectedAction;
        }

        public Housemate SelectTarget(CPU_TARGET_TYPE targetType, List<Housemate> nearbyHousemates, Housemate? SO)
        {
            if (nearbyHousemates.Count == 0)
            {
                throw new Exception("Cannot select a target with no nearby housemates.");
            }
            
            Housemate? target;
            List<Housemate> nearbyFriendlies = nearbyHousemates.Where(h => HasPositiveOpinionOf(h)).ToList();
            List<Housemate> nearbyEnemies = nearbyHousemates.Where(h => !HasPositiveOpinionOf(h)).ToList();
            
            List<Housemate> nearbyVictims = WitnessedInfidelities
                .Select(w => w.Victim)
                .Where(w => nearbyHousemates.Contains(w))
                .ToList();


            switch (targetType)
            {
                case CPU_TARGET_TYPE.RANDOM:
                    target = nearbyHousemates.OrderBy(h => rand.Next()).First();
                    break;
                
                case CPU_TARGET_TYPE.RANDOM_FRIENDLY:
                    target = nearbyFriendlies.OrderBy(h => rand.Next()).FirstOrDefault();
                    break;
                
                case CPU_TARGET_TYPE.RANDOM_ENEMY:
                    target = nearbyEnemies.OrderBy(h => rand.Next()).FirstOrDefault();
                    break;

                case CPU_TARGET_TYPE.BEST_FRIEND:
                    target = nearbyHousemates.OrderBy(h => -1 * GetOpinionOf(h)).First();
                    break;

                case CPU_TARGET_TYPE.BEST_FRIEND_EXCLUDING_SO:
                    target = nearbyHousemates.Where(h => h != SO).OrderBy(h => -1 * GetOpinionOf(h)).FirstOrDefault();
                    break;

                case CPU_TARGET_TYPE.WORST_ENEMY:
                    target = nearbyHousemates.OrderBy(h => GetOpinionOf(h)).First();
                    break;

                case CPU_TARGET_TYPE.BEST_FRIEND_WITH_DIRT:
                    target = nearbyVictims.OrderBy(h => -1 * GetOpinionOf(h)).FirstOrDefault();
                    if (target == null)
                    {
                        target = SelectTarget(CPU_TARGET_TYPE.WORST_ENEMY, nearbyHousemates, SO);
                    }
                    break;
                case CPU_TARGET_TYPE.NONE:
                default:
                    throw new Exception($"Target Type is required.");
            }

            if (target == null)
            {
                if (targetType == CPU_TARGET_TYPE.RANDOM)
                {
                    throw new Exception("Could not find a random target!");
                }
                else
                {
                    target = SelectTarget(CPU_TARGET_TYPE.RANDOM, nearbyHousemates, SO);
                }
            }

            return target;
        }

        public int GetActionHistoryCount(ACTION id, int currentDayNum, int numDaysExcludingToday)
        {
            return ActionHistory
                        .Where(h => h.Item1 >= currentDayNum - numDaysExcludingToday)
                        .Where(h => h.Item2 == id)
                        .Count();
        }
    }
}
