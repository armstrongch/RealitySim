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
                .ToList();

            switch (action.Id)
            {
                case ACTION.WORK_A_SHIFT:
                    housemate.Cash += 100;
                    Console.WriteLine($"{housemate.Name} works a shift at the bagel shop.");
                    if (threeDayTotal >= 3)
                    {
                        Console.WriteLine($"{housemate.Name} has worked {threeDayTotal.ToString()} shifts in the last 3 days. Viewers are starting to lose interest.");
                        housemate.Karma -= 1;
                    }
                    break;
                case ACTION.GO_TO_BED:
                    housemate.Awake = false;
                    housemate.currentLocation = LOCATION.DREAMLAND;
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
                    Console.WriteLine($"{housemate.Name} starts a fistfight with {targetName}.");
                    // Viewers like or dislike you depending on whether they like or dislike your target
                    IncrementKarma(housemate, target, false, 3);
                    // Target dislikes you 
                    target.IncrementOpinion(housemate, -3);
                    // Nearby housemates like or dislike you depending on whether they like or dislike your target
                    ReactToInteraction(housemate, target, witnesses, false, 1, ACTION.PUNCH);
                    break;
                case ACTION.FLIRT:
                    Console.WriteLine($"{housemate.Name} flirts with {targetName}.");
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
                            //Action breakup = Actions.First(a => a.Id == ACTION.BREAK_UP);
                            //SO.Energy += breakup.EnergyCost;
                            //PerformAction(breakup, SO, null, housemate.currentLocation);
                            throw new NotImplementedException("What should happen when your SO sees you cheating?");
                        }
                        else
                        {
                            // Otherwise, this will impact other witnesses opinion of you, and they may tattle on you in the future.
                            ReactToInteraction(housemate, SO, witnesses, false, 1, ACTION.FLIRT);
                            WitnessAction(housemate, target, witnesses, SO, ACTION.FLIRT);
                        }
                    }
                    break;
                case ACTION.ENTER_A_RELATIONSHIP:
                    //If you are trying to start a relationship with your current SO for some reason
                    if (AreCurrentlyDating(housemate, target))
                    {
                        Console.WriteLine($"{targetName} is falling deeper in love with {housemate.Name}.");
                        target.IncrementOpinion(housemate, 2);
                        housemate.IncrementOpinion(target, 2);
                    }
                    else
                    {
                        Console.WriteLine($"{housemate.Name} wants to make it official with {targetName}.");

                        Housemate? targetSO = GetSignificantOther(target);
                        bool targetIsSingle = targetSO == null;

                        if (!targetIsSingle)
                        {
                            Console.WriteLine($"{targetName} is currently dating {targetSO.Name}.");
                            bool targetLikesSO = target.HasPositiveOpinionOf(targetSO);
                            if (targetLikesSO)
                            {
                                Console.WriteLine($"This erodes {targetSO.Name}'s opinion of {housemate.Name}");
                                targetSO.IncrementOpinion(housemate, -3);
                                targetIsSingle = false;
                            }
                            else
                            {
                                Console.WriteLine($"{targetName} is dissatisfied with his relationship.");
                                Action breakup = Actions.First(a => a.Id == ACTION.BREAK_UP);
                                target.Energy += breakup.EnergyCost;
                                PerformAction(breakup, target, null, housemate.currentLocation);
                            }
                        }

                        if (targetIsSingle && target.HasPositiveOpinionOf(housemate))
                        {
                            Console.WriteLine($"{targetName} is into {housemate.Name} too, and they enter into a relationship.");
                            Relationships.Add((housemate, target));
                            IncrementKarma(housemate, target, true, 1);
                        }
                        else
                        {
                            Console.WriteLine($"{targetName} rejects {housemate.Name}");
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
                printString += $"{target.Name} is a fan favorite, "; 
            }
            else
            {
                printString += $"Viewers already dislike {target.Name}, ";
            }

            if (targetHasPositiveKarma != positiveRelationship)
            {
                printString += $"so {housemate.Name}'s reputation with audiences declines.";
                housemate.Karma -= Math.Abs(target.Karma);
            }
            else
            {
                printString += $"so {housemate.Name}'s reputation with audiences improves.";
                housemate.Karma += Math.Abs(target.Karma);
            }
            Console.WriteLine(printString);
        }

        private void WitnessAction(Housemate housemate, Housemate target, List<Housemate> witnesses, Housemate victim, ACTION actionId)
        {

            Console.WriteLine($"{GetNames(witnesses)} will remember this.");
            
            foreach (Housemate witness in witnesses)
            {
                WitnessedEvents.Add(new WitnessedEvent(witness, housemate, victim, target, actionId));
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
                    int changeAmount = (witness.HasPositiveOpinionOf(target) ^ positiveAction ? -1 : 1) * positiveChangeAmount;
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

        /*
        new Action(ACTION.BREAK_UP, "Break Up", "End your current relationship.", all, false, 8),
        new Action(ACTION.TATTLE, "Tattle", "Tell a housemate that their partner has been unfaithful", all, true, 4),
         */
    }
}
