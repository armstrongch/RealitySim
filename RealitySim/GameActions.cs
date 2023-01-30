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

            string targetName = target == null ? string.Empty : target.Name;
            
            Housemate? SO = GetSignificantOther(housemate);

            List<Housemate> witnesses = Housemates
                .Where(h => h.currentLocation == housemate.currentLocation)
                .Where(h => h.Name != housemate.Name && h.Name != targetName)
                .ToList();

            switch (action.Id)
            {
                case ACTION.BUY_COFFEE:
                    if (housemate.Cash >= 5)
                    {
                        housemate.Cash -= 5;
                        DoOtherAction(housemate, ACTION.DRINK_COFFEE, null);
                    }
                    else
                    {
                        Console.WriteLine($"{housemate.Name} cannot afford a $5 cup of coffee.");
                    }
                    break;
                case ACTION.DRINK_COFFEE:
                    int coffeesDrankToday = housemate.GetActionHistoryCount(ACTION.DRINK_COFFEE, currentDayNum, 0) - 1;
                    if (coffeesDrankToday == 0)
                    {
                        Console.WriteLine($"{housemate.Name} spends $5 on a cup of coffee, and gains an additional 20 energy.");
                        housemate.Energy += 20;
                    }
                    else
                    {
                        int energyGain = Convert.ToInt32(Math.Max(1, 20 / (2 * coffeesDrankToday)));
                        housemate.Energy += energyGain;
                        string cups = coffeesDrankToday == 1 ? "cup" : "cups";

                        Console.WriteLine($"{housemate.Name} spends $5 on another cup of coffee. " +
                            $"He has already drank {coffeesDrankToday.ToString()} {cups} today, so it only restores {energyGain.ToString()} energy.");
                    }
                    break;
                case ACTION.WORK_A_SHIFT:
                    housemate.Cash += 100;
                    Console.WriteLine($"{housemate.Name} works a shift at the bagel shop.");
                    int shiftsWorkedThisWeek = housemate.GetActionHistoryCount(ACTION.WORK_A_SHIFT, currentDayNum, 4);
                    if (shiftsWorkedThisWeek >= 3)
                    {
                        Console.WriteLine($"{housemate.Name} has worked {shiftsWorkedThisWeek.ToString()} shifts in the last week. Viewers are starting to lose interest.");
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
                    if (SO != null && SO != target)
                    {
                        // If your SO witnessed the event, the will break up with you.
                        if (witnesses.Contains(SO))
                        {
                            Console.WriteLine($"{SO.Name} ({housemate.Name}'s current partner) is devastated.");
                            SO.IncrementOpinion(housemate, -3);
                            IncrementKarma(housemate, SO, false, 3);

                            if (!SO.HasPositiveOpinionOf(housemate))
                            {
                                Action breakup = Actions.First(a => a.Id == ACTION.BREAK_UP);
                                SO.Energy += breakup.EnergyCost;
                                PerformAction(breakup, SO, null, housemate.currentLocation);
                            }
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
                        Housemate? housemateSO = GetSignificantOther(housemate);
                        if (housemateSO != null)
                        {
                            Console.WriteLine($"{housemate.Name} wants to start a relationship with {targetName}, but he is already dating {housemateSO.Name}.");
                            DoOtherAction(housemate, ACTION.FLIRT, target);
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
                    }

                    break;
                case ACTION.BREAK_UP:
                    if (SO != null)
                    {
                        Console.WriteLine($"{housemate.Name} has decided to break up with {SO.Name}.");
                        if (SO.HasPositiveOpinionOf(housemate))
                        {
                            Console.WriteLine($"{SO.Name} is heartbroken.");
                            SO.IncrementOpinion(housemate, -2);
                            IncrementKarma(housemate, SO, false, 2);
                        }
                        else
                        {
                            Console.WriteLine($"{SO.Name} wasn't really into {housemate.Name} anyway, so there are no hard feelings.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{housemate.Name} declares that he is single and ready to mingle.");
                        IncrementKarma(housemate, housemate, true, 1);
                    }
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        /*
        new Action(ACTION.BREAK_UP, "Break Up", "End your current relationship.", all, false, 8),
        new Action(ACTION.TATTLE, "Tattle", "Tell a housemate that their partner has been unfaithful", all, true, 4),
         */
    }
}
