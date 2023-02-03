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
                ("Joe", "My name is Joe!"),
                ("Kevin", "My name is Kevin!"),
                ("Matt", "My name is Matt!"),
                ("Sam", "My name is Sam!"),
                ("Thad", "My name is Thad!"),
                /*("Turd", "My name is Turd!"),
                ("Eggbert", "My name is Eggbert!"),
                ("Fuckston", "My name is Fuckston!")*/
            };

            names = names.OrderBy(n => rand.Next()).ToList();


            for (int i = 0; i < names.Count; i++)
            {
                int? playerNum = null;
                if (i < numPlayers)
                {
                    Console.WriteLine($"Player {(i + 1).ToString()} is {names[i].Item1}: \"{names[i].Item2}\"");
                    playerNum = i + 1;
                }
                Housemates.Add(new Housemate(names[i].Item1, LOCATION.HOUSE, playerNum));
                
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
            LOCATION[] nowhere = { }; //ONLY CALLED BY OTHER ACTIONS
            string none = "IF YOU ARE SEEING THIS, THERE IS A BUG IN THE GAME!";

            Actions = new List<Action>()
            {
                new Action(ACTION.WORK_A_SHIFT, "Work a Shift.", "Earn $100.", work, false, 20, CPU_TARGET_TYPE.NONE),
                new Action(ACTION.GO_TO_BED, "Go to Bed", "End your day.", house, false, 0, CPU_TARGET_TYPE.NONE),

                new Action(ACTION.GO_TO_WORK, "Go to Work.", "Walk to the bagel shop.", club_or_house),
                new Action(ACTION.GO_TO_THE_CLUB, "Go to the Club", "Take a taxi to the nightclub.", work_or_house),
                new Action(ACTION.GO_HOME, "Go Home", "Head back to the house.", club_or_work, false, 0, CPU_TARGET_TYPE.NONE),

                new Action(ACTION.BUY_COFFEE, "Buy a Cup of Coffee", "Pay $5 for a caffenated beverate to boost your energy.", work),
                new Action(ACTION.DRINK_COFFEE, none, none, nowhere),

                new Action(ACTION.BUY_A_SHOT, "Buy a Shot of Alcohol", "Pay $10 for a big sip of hard liquor.", club),
                new Action(ACTION.TAKE_A_SHOT, none, none, nowhere),

                new Action(ACTION.PUNCH, "Punch", "Start a fistfight with a housemate.", all, true, 8, CPU_TARGET_TYPE.WORST_ENEMY),
                new Action(ACTION.FLIRT, "Flirt", "Make a romantic pass at a housemate.", all, true, 4, CPU_TARGET_TYPE.RANDOM_FRIENDLY),
                new Action(ACTION.ENTER_A_RELATIONSHIP, "Enter a Relationship", "Become exclusive partners with a housemate.", all, true, 4, CPU_TARGET_TYPE.BEST_FRIEND_EXCLUDING_SO),
                new Action(ACTION.BREAK_UP, "Break Up", "End your current relationship.", all, false, 8, CPU_TARGET_TYPE.NONE),
                new Action(ACTION.TATTLE_TO, "Tattle", "Tell a housemate that their partner has been unfaithful", all, true, 4, CPU_TARGET_TYPE.BEST_FRIEND_WITH_DIRT),
            };
        }
    }
}
