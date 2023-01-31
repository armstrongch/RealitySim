using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal partial class Game
    {
        private void ChallengeRelationship(Housemate victim, Housemate perp)
        {
            Console.WriteLine($"{victim.Name} ({perp.Name}'s current partner) is devastated.");
            victim.IncrementOpinion(perp, -3);
            IncrementKarma(perp, victim, false, 3);

            if (!victim.HasPositiveOpinionOf(perp))
            {
                DoOtherAction(victim, ACTION.BREAK_UP, null);
            }
        }
        
        private void DoOtherAction(Housemate housemate, ACTION id, Housemate? target)
        {
            Action action = Actions.Where(a => a.Id == id).First();
            housemate.Energy += action.EnergyCost;
            PerformAction(action, housemate, target, housemate.currentLocation);
        }

        private Housemate? GetSignificantOther(Housemate? housemate)
        {
            if (housemate == null)
            {
                return null;
            }
            
            for (int i = 0; i < Relationships.Count(); i += 1)
            {
                if (Relationships[i].Item1 == housemate)
                {
                    return Relationships[i].Item2;
                }
                else if (Relationships[i].Item2 == housemate)
                {
                    return Relationships[i].Item1;
                }
            }
            
            return null;
        }

        private void BreakUp(Housemate housemate)
        {
            foreach((Housemate, Housemate) rel in Relationships)
            {
                if ((rel.Item1 == housemate) || (rel.Item2 == housemate))
                {
                    Relationships.Remove(rel);
                    break;
                }
            }
        }

        private void IncrementKarma(Housemate housemate, Housemate target, bool positiveRelationship, int positiveChangeAmount)
        {
            string printString = string.Empty;
            bool targetHasPositiveKarma = target.Karma > 0;

            if (targetHasPositiveKarma)
            {
                printString += $"{target.Name} is a fan favorite, ";
            }
            else
            {
                printString += $"Viewers already dislike {target.Name}, ";
            }

            if (targetHasPositiveKarma != positiveRelationship)
            {
                printString += $"so {housemate.Name}'s reputation with audiences declines.";
                housemate.Karma -= Math.Abs(positiveChangeAmount);
            }
            else
            {
                printString += $"so {housemate.Name}'s reputation with audiences improves.";
                housemate.Karma += Math.Abs(positiveChangeAmount);
            }
            Console.WriteLine(printString);
        }

        private void WitnessAction(Housemate housemate, Housemate target, List<Housemate> witnesses, Housemate victim, ACTION actionId)
        {

            if (witnesses.Count > 0)
            {
                Console.WriteLine($"{GetNames(witnesses)} will remember this.");
            }

            foreach (Housemate witness in witnesses)
            {
                housemate.WitnessedInfidelities.Add(new WitnessedInfidelity(housemate, victim, target));
            }
        }

        private void ReactToInteraction(Housemate housemate, Housemate target, List<Housemate> witnesses, bool positiveAction, int positiveChangeAmount, ACTION actionId)
        {
            if (witnesses.Count > 0)
            {
                string isAre = witnesses.Count == 1 ? "is" : "are";
                Console.WriteLine($"{GetNames(witnesses)} {isAre} nearby.");

                List<Housemate> likers = witnesses.Where(w => w.HasPositiveOpinionOf(target)).ToList();
                List<Housemate> dislikers = witnesses.Where(w => !w.HasPositiveOpinionOf(target)).ToList();

                string declines = "declines";
                string improves = "improves";

                if (likers.Count > 0)
                {
                    string hasHave = likers.Count == 1 ? "has" : "have";
                    string hisTheir = likers.Count == 1 ? "his" : "their";

                    string likerString = $"{GetNames(likers)} {hasHave} no beef with {target.Name}, " +
                        $"so {hisTheir} opinion of {housemate.Name} {(positiveAction ? improves : declines)}.";

                    Console.WriteLine(likerString);
                }

                if (dislikers.Count > 0)
                {
                    string dislikes = dislikers.Count == 1 ? "dislikes" : "dislike";
                    string hisTheir = dislikers.Count == 1 ? "his" : "their";
                    string dislikerString = $"{GetNames(dislikers)} already {dislikes} {target.Name}, " +
                        $"so {hisTheir} opinion of {housemate.Name} {(positiveAction ? declines : improves)}.";

                    Console.WriteLine(dislikerString);
                }

                foreach (Housemate witness in witnesses)
                {
                    int changeAmount = (witness.HasPositiveOpinionOf(target) ^ positiveAction ? -1 : 1) * Math.Abs(positiveChangeAmount);
                    witness.IncrementOpinion(housemate, changeAmount);
                }
            }
            else
            {
                Console.WriteLine("There are no witnesses.");
            }
        }

        private bool AreCurrentlyDating(Housemate housemate1, Housemate housemate2)
        {
            return Relationships.Contains((housemate1, housemate2)) || Relationships.Contains((housemate2, housemate1));
        }

        private string GetNames(List<Housemate> housemates)
        {
            List<string> names = housemates.Select(h => h.Name).ToList();
            if (names.Count == 0)
            {
                throw new Exception("Can't get names from a list containing 0 housemates.");
            }
            else if (names.Count == 1)
            {
                return names.First();
            }
            else if (names.Count == 2)
            {
                return $"{names[0]} and {names[1]}";
            }
            else
            {
                names[names.Count - 1] = "and " + names[names.Count - 1];
                return String.Join(", ", names);
            }
        }
    }
}
