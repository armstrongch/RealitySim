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
        private void InitializeHousemates(int numPlayers)
        {
            List<(string, string)> names = new List<(string,string)>()
            {
                ("Joe", "Desc"),
                ("Kevin", "Desc"),
                ("Matt", "Desc"),
                ("Sam", "Desc"),
                ("Thad", "Desc"),
                ("Turd", "Desc")
            };

            names = names.OrderBy(n => rand.Next()).ToList();


            for (int i = 0; i < names.Count; i++)
            {
                if (i < numPlayers)
                {
                    Console.WriteLine($"Player {(i+1).ToString()} is {names[i].Item1}: {names[i].Item2}.");
                }
                Housemates.Add(new Housemate(names[i].Item1, LOCATION.HOUSE));
            }
        }

        private void InitializeActions()
        {
            LOCATION[] work = { LOCATION.WORK };
            LOCATION[] club = { LOCATION.CLUB };
            LOCATION[] house = { LOCATION.HOUSE };
            LOCATION[] all = { LOCATION.WORK, LOCATION.CLUB, LOCATION.HOUSE };
            LOCATION[] club_or_house = { LOCATION.CLUB, LOCATION.HOUSE };
            LOCATION[] work_or_house = { LOCATION.WORK, LOCATION.HOUSE };
            LOCATION[] club_or_work = { LOCATION.WORK, LOCATION.CLUB };

            Actions = new List<Action>()
            {
                new Action(ACTION.WORK_A_SHIFT, "Work a Shift.", "Earn $100.", work, false, 20),
                new Action(ACTION.GO_TO_BED, "Go to Bed", "End your day.", house, false, 0),

                new Action(ACTION.GO_TO_WORK, "Go to Work.", "Walk to the bagel shop.", club_or_house),
                new Action(ACTION.GO_TO_THE_CLUB, "Go to the Club", "Take a taxi to the nightclub.", work_or_house),
                new Action(ACTION.GO_HOME, "Go Home", "Head back to the house.", club_or_work),

                new Action(ACTION.PUNCH, "Punch", "Start a fistfight with a housemate.", club_or_work, true, 8),
                new Action(ACTION.FLIRT, "Flirt", "Make a romantic pass at a housemate.", all, true, 4),
                new Action(ACTION.ENTER_A_RELATIONSHIP, "Enter a Relationship", "Become exclusive partners with a housemate.", all, true, 4),
                new Action(ACTION.BREAK_UP, "Break Up", "End your current relationship.", all, false, 8),
                new Action(ACTION.TATTLE, "Tattle", "Tell a housemate that their partner has been unfaithful", all, true, 4),
            };
        }
    }
}
