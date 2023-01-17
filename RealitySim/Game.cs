using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal class Game
    {
        List<Action> Actions = new List<Action>();;
        List<Housemate> Housemates = new List<Housemate>();
        Random rand = new Random();
        
        public Game()
        {
            InitializeHousemates();
            InitializeActions();

            bool everyoneIsAsleep = false;
            while (!everyoneIsAsleep)
            {
                everyoneIsAsleep = !Housemates.Where(h => h.Awake).Any();
            }
        }

        private void InitializeHousemates()
        {
            List<string> names = new List<string>()
            {
                "Joe", "Kevin", "Matt", "Sam", "Thad", "Turd"
            };
            names = names.OrderBy(n => rand.Next()).ToList();

            for (int i = 0; i < names.Count; i++)
            {
                Housemates.Add(new Housemate(names[i], LOCATION.HOUSE));
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
                new Action("Work a Shift.", "Earn $100.", work ),
                new Action("Go to Bed", "End your day.", house),

                new Action("Go to Work.", "Walk to the bagel shop.", club_or_house ),
                new Action("Go to the Club", "Take a taxi to the nightclub.", work_or_house),
                new Action("Go Home", "Head back to the house.", club_or_work),
                
                new Action("Punch", "Start a fistfight with a housemate.", club_or_work, true, false),
                new Action("Flirt", "Make a romantic pass at a housemate.", all, true, true),
                new Action("Enter a Relationship", "Become exclusive partners with a housemate.", all, true, false),
                new Action("Break Up", "End your current relationship.", all),
                new Action("Tattle", "Tell a housemate that their partner has been unfaithful", all, true, false),
            };
        }
    }
}
