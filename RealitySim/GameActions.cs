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

            switch(action.Id)
            {
                case ACTION.WORK_A_SHIFT:
                    housemate.Cash += 100;
                    Console.WriteLine($"{housemate.Name} works a shift at the bagel shop.");
                    break;
                case ACTION.GO_TO_BED:
                    housemate.Awake = false;
                    housemate.Energy = HousemateMaxEnergy;
                    Console.WriteLine($"{housemate.Name} goes to sleep.");
                    break;
                case ACTION.GO_TO_WORK:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }
    }
}
