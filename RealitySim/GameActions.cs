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
        private void PerformAction(Action action, Housemate housemate, Housemate? target, LOCATION location)
        {
            housemate.Energy -= action.EnergyCost;
            housemate.ActionHistory.Add((currentDayNum, action.Id));

            int threeDayTotal = housemate.ActionHistory
                        .Where(h => h.Item1 >= currentDayNum - 2)
                        .Where(h => h.Item2 == action.Id)
                        .Count();

            string targetName = target == null ? string.Empty : target.Name;
            

            List<Housemate> witnesses = Housemates
                .Where(h => h.currentLocation == housemate.currentLocation)
                .Where(h => h.Name != housemate.Name && h.Name != targetName)
                .Where(h => h.Awake)
                .ToList();

            switch (action.Id)
            {
                case ACTION.WORK_A_SHIFT:
                    housemate.Cash += 100;
                    Console.WriteLine($"{housemate.Name} works a shift at the bagel shop.");
                    if (threeDayTotal >= 3)
                    {
                        Console.WriteLine($"{housemate.Name} has worked ${threeDayTotal.ToString()} shifts in the last 3 days. Viewers are starting to lose interest.");
                        housemate.Karma -= 1;
                    }
                    break;
                case ACTION.GO_TO_BED:
                    housemate.Awake = false;
                    housemate.Energy = HousemateMaxEnergy;
                    Console.WriteLine($"{housemate.Name} goes to sleep.");
                    break;
                case ACTION.GO_TO_WORK:
                    housemate.currentLocation = LOCATION.WORK;
                    Console.WriteLine($"{housemate.Name} walks to the bagel shop.");
                    break;
                case ACTION.GO_TO_THE_CLUB:
                    housemate.currentLocation = LOCATION.CLUB;
                    Console.WriteLine($"{housemate.Name} takes a taxi to the club.");
                    break;
                case ACTION.GO_HOME:
                    housemate.currentLocation = LOCATION.HOUSE;
                    Console.WriteLine($"{housemate.Name} decides to head home.");
                    break;
                case ACTION.PUNCH:
                    Console.WriteLine($"{housemate.Name} starts a fistfight with ${targetName}.");
                    // Viewers like or dislike you depending on whether they like or dislike your target
                    IncrementKarma(housemate, target, false, 3);
                    // Target dislikes you 
                    target.IncrementOpinion(housemate, -3);
                    // Nearby housemates like or dislike you depending on whether they like or dislike your target
                    foreach (Housemate witness in witnesses)
                    {
                        WitnessAction(housemate, target, witness, null, 1, action.Id); 
                    }
                    break;
                case ACTION.FLIRT:
                    Console.WriteLine($"{housemate.Name} flirts with ${targetName}.");
                    // Target likes you
                    target.IncrementOpinion(housemate, 2);

                    // If you are cheating on your SO
                    Housemate? SO = GetSignificantOther(housemate);
                    if (SO != null && SO != target)
                    {
                        Console.WriteLine($"{housemate.Name} is currently in a relationship with {SO.Name}.");
                        // If your SO witnessed the event, the will break up with you.
                        if (witnesses.Contains(SO))
                        {
                            Console.WriteLine($"{SO.Name} is nearby.");
                            Action breakup = Actions.First(a => a.Id == ACTION.BREAK_UP);
                            SO.Energy += breakup.EnergyCost;
                            PerformAction(breakup, SO, null, housemate.currentLocation);
                        }
                        else
                        {
                            foreach(Housemate witness in witnesses)
                            {
                                // Otherwise, this will impact other witnesses opinion of you, and they may tattle on you in the future.
                                WitnessAction(housemate, target, witness, SO, 1, ACTION.FLIRT);
                            }
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private Housemate? GetSignificantOther(Housemate? housemate)
        {
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

        private void IncrementKarma(Housemate housemate, Housemate target, bool positiveRelationship, int positiveChangeAmount)
        {
            string printString = string.Empty;
            bool targetHasPositiveKarma = target.Karma > 0;
            
            if (targetHasPositiveKarma)
            {
                printString += $"Viewers already dislike {target.Name}, ";
            }
            else
            {
                printString += $"{target.Name} is a fan favorite, ";
            }

            if (targetHasPositiveKarma != positiveRelationship)
            {
                printString += $"so ${housemate.Name}'s reputation with audiences declines.";
                housemate.Karma -= Math.Abs(target.Karma);
            }
            else
            {
                printString += $"so ${housemate.Name}'s reputation improves.";
                housemate.Karma += Math.Abs(target.Karma);
            }
            Console.WriteLine(printString);
        }

        private void WitnessAction(Housemate housemate, Housemate target, Housemate witness, Housemate? victim, int positiveChangeAmount, ACTION actionId)
        {
            string printString = $"{witness.Name} is nearby. ";
            bool witnessLikesTarget = witness.HasPositiveOpinionOf(target);
            if (witnessLikesTarget)
            {
                printString += $"He already dislikes {target.Name}, so his opinion of {housemate.Name} improves.";
                witness.IncrementOpinion(housemate, positiveChangeAmount);
            }
            else
            {
                printString += $"He has no beef with {target.Name}, so his opinion of {housemate.Name} declines.";
                witness.IncrementOpinion(housemate, positiveChangeAmount * -1);
            }
            Console.WriteLine(printString);
            
            if (victim != null)
            {
                WitnessedEvents.Add(new WitnessedEvent(witness, housemate, victim, target, actionId));
            }
        }

        private string[] GetNames(List<Housemate> housemates)
        {
            List<string> names = new List<string>();
            foreach (Housemate housemate in housemates)
            {
                names.Add(housemate.Name);
            }
            return names.ToArray();
        }

        /*

        new Action(ACTION.ENTER_A_RELATIONSHIP, "Enter a Relationship", "Become exclusive partners with a housemate.", all, true, 4),
        new Action(ACTION.BREAK_UP, "Break Up", "End your current relationship.", all, false, 8),
        new Action(ACTION.TATTLE, "Tattle", "Tell a housemate that their partner has been unfaithful", all, true, 4),
         */
    }
}
