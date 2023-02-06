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
        private void RunTests()
        {
            //SHOT_Tests();
            //TATTLE_ON_Tests();
            DrunkenFlirtingTargetSelection_Tests();


            Console.WriteLine(stars);
            Console.WriteLine("PRESS ENTER TO EXIT");
            Console.ReadLine();
        }
        public void DrunkenFlirtingTargetSelection_Tests()
        {
            Housemate h1 = Housemates[0];
            LOCATION HOUSE = LOCATION.HOUSE;
            Action BUY_A_SHOT = Actions.Where(a => a.Id == ACTION.BUY_A_SHOT).First();
            Action GO_TO_THE_CLUB = Actions.Where(a => a.Id == ACTION.GO_TO_THE_CLUB).First();
            Action GO_TO_BED = Actions.Where(a => a.Id == ACTION.GO_TO_BED).First();

            Console.WriteLine(stars);
            Console.WriteLine("Test: Flirting when everyone is asleep");
            Console.WriteLine(stars);

            foreach(Housemate h in Housemates)
            {
                if (h != h1)
                {
                    PerformAction(GO_TO_BED, h, null, HOUSE);
                    Console.WriteLine(stars);
                }
            }

            h1.Cash = 999;
            for (int i = 0; i < 15; i += 1)
            {
                PerformAction(BUY_A_SHOT, h1, null, HOUSE);
                Console.WriteLine(stars);
            }

            //COPIED FROM MAIN LOOP, I want to see what available actions there are in this scenario.
            List<Housemate> nearbyHousemates = Housemates
                            .Where(h => h.currentLocation == h1.currentLocation)
                            .Where(h => h != h1)
                            .ToList();

            bool alone = nearbyHousemates.Count == 0;

            List<Action> availableActions = Actions
                            //Valid Location
                            .Where(a => a.ValidLocations.Contains(h1.currentLocation))
                            //Other housemates in location, or action does not require target
                            .Where(a => !a.RequiresTarget || !alone)
                            //Housemate has enough energy
                            .Where(a => a.EnergyCost <= h1.Energy)
                            .ToList();

            foreach (Action action in availableActions)
            {
                if (action.Id != ACTION.GO_TO_BED)
                {
                    PerformAction(action, h1, null, HOUSE);
                    Console.WriteLine(stars);
                }
            }

            PerformAction(GO_TO_BED, h1, null, HOUSE);
            Console.WriteLine(stars);
        }

        public void SHOT_Tests()
        {
            Housemate h1 = Housemates[0];
            Housemate h2 = Housemates[1];
            Housemate h3 = Housemates[2];
            Housemate h4 = Housemates[3];
            Housemate h5 = Housemates[4];
            LOCATION CLUB = LOCATION.CLUB;
            LOCATION HOUSE = LOCATION.HOUSE;
            Action BUY_A_SHOT = Actions.Where(a => a.Id == ACTION.BUY_A_SHOT).First();
            Action GO_HOME = Actions.Where(a => a.Id == ACTION.GO_HOME).First();
            Action GO_TO_BED = Actions.Where(a => a.Id == ACTION.GO_TO_BED).First();
            Action BUY_A_ROUND_OF_SHOTS = Actions.Where(a => a.Id == ACTION.BUY_A_ROUND_OF_SHOTS).First();
            Action FLIRT = Actions.Where(a => a.Id == ACTION.FLIRT).First();


            Console.WriteLine(stars);
            Console.WriteLine("Test: Can't afford a shot.");
            Console.WriteLine(stars);
            h1.currentLocation = CLUB;
            h1.Cash = 0;
            PerformAction(BUY_A_SHOT, h1, null, CLUB);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Take a shot.");
            Console.WriteLine(stars);
            h1.Cash = 200;
            PerformAction(BUY_A_SHOT, h1, null, CLUB);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Buy a round of shots.");
            Console.WriteLine(stars);
            h1.Cash = 200;
            h2.currentLocation = CLUB;
            h3.currentLocation = CLUB;
            h4.currentLocation = CLUB;
            PerformAction(BUY_A_ROUND_OF_SHOTS, h1, null, CLUB);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Get wasted.");
            h2.currentLocation = HOUSE;
            h3.currentLocation = HOUSE;
            h4.currentLocation = HOUSE;
            for (int i = 0; i < 10; i += 1)
            {
                Console.WriteLine(stars);
                PerformAction(BUY_A_SHOT, h1, null, CLUB);
            }
            Console.WriteLine(stars);
            PerformAction(GO_HOME, h1, null, CLUB);
            Console.WriteLine(stars);
            PerformAction(FLIRT, h1, h2, HOUSE);

        }

        private void TATTLE_ON_Tests()
        {
            Housemate h1 = Housemates[0];
            Housemate h2 = Housemates[1];
            Housemate h3 = Housemates[3];
            Housemate h4 = Housemates[4];

            Action TATTLE_TO = Actions.Where(a => a.Id == ACTION.TATTLE_TO).First();

            LOCATION House = LOCATION.HOUSE;

            Console.WriteLine(stars);
            Console.WriteLine("Test: Spread rumor to single housemate");
            Console.WriteLine(stars);
            PerformAction(TATTLE_TO, h1, h2, House);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Tell single housemate of past infidelity");
            Console.WriteLine(stars);
            h1.WitnessedInfidelities.Add(new WitnessedInfidelity(h3, h2, h4));
            PerformAction(TATTLE_TO, h1, h2, House);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Lie about infidelity to a housemate in a relationship");
            Console.WriteLine(stars);
            Relationships.Add((h2, h3));
            PerformAction(TATTLE_TO, h1, h2, House);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Expose a cheating scandal");
            Console.WriteLine(stars);
            Relationships.Clear();
            Relationships.Add((h2, h3));
            h1.WitnessedInfidelities.Add(new WitnessedInfidelity(h3, h2, h4));
            PerformAction(TATTLE_TO, h1, h2, House);

            Console.WriteLine(stars);
            Console.WriteLine("Test: Tattle on self");
            Console.WriteLine(stars);
            Relationships.Clear();
            Relationships.Add((h1, h2));
            PerformAction(TATTLE_TO, h1, h2, House);
        }
    }
}
