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
        List<Action> Actions = new List<Action>();
        List<Housemate> Housemates = new List<Housemate>();
        Random rand = new Random();
        int currentDayNum = 1;
        List<(Housemate, Housemate)> Relationships = new List<(Housemate, Housemate)>();
        List<WitnessedEvent> WitnessedEvents = new List<WitnessedEvent>();
        const string stars = "*******************************************************";


        public Game(int numPlayers)
        {
            InitializeHousemates(numPlayers);
            InitializeActions();

            bool everyoneIsAsleep = false;
            while (!everyoneIsAsleep)
            {
                foreach(Housemate housemate in Housemates.Where(h => h.Awake).ToList())
                {
                    Console.WriteLine(stars);
                    Console.WriteLine($"It is {housemate.Name}'s turn!");

                    List<Housemate> nearbyHousemates = Housemates
                        .Where(h => h.currentLocation == housemate.currentLocation)
                        .Where(h => h != housemate)
                        .ToList();

                    housemate.ShowInfo(BuildRelationshipMatrix(housemate), nearbyHousemates);
                    
                    bool alone = Housemates.Where(h => h.currentLocation == housemate.currentLocation).Count() > 0;
                    
                    List<Action> availableActions = Actions
                        //Valid Location
                        .Where(a => a.ValidLocations.Contains(housemate.currentLocation))
                        //Other housemates in location, or action does not require target
                        .Where(a => !a.RequiresTarget || !alone)
                        //Housemate has enough energy
                        .Where(a => a.EnergyCost <= housemate.Energy)
                        .ToList();
                    
                    Action selectedAction = housemate.SelectAction(availableActions);
                    Housemate? selectedTarget = null;
                    if (selectedAction.RequiresTarget)
                    {
                        selectedTarget = housemate.SelectTarget(selectedAction);
                    }

                    PerformAction(selectedAction, housemate, selectedTarget, housemate.currentLocation);
                }
                
                //Day is over once everyone is asleep
                everyoneIsAsleep = !Housemates.Where(h => h.Awake).Any();
            }
            currentDayNum += 1;
            throw new NotImplementedException();
        }
    }
}
