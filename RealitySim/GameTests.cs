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
            Action ENTER_A_RELATIONSHIP = Actions.Where(a => a.Id == ACTION.ENTER_A_RELATIONSHIP).First();
            Action BREAK_UP = Actions.Where(a => a.Id == ACTION.BREAK_UP).First();
            Action BUY_COFFEE = Actions.Where(a => a.Id == ACTION.BUY_COFFEE).First();
            Action GO_TO_WORK = Actions.Where(a => a.Id == ACTION.GO_TO_WORK).First();
            Action WORK_A_SHIFT = Actions.Where(a => a.Id == ACTION.WORK_A_SHIFT).First();

            Console.WriteLine(stars);
            PerformAction(GO_TO_WORK, Housemates[0], null, Housemates[0].currentLocation);
            Console.WriteLine(stars);
            PerformAction(BUY_COFFEE, Housemates[0], null, Housemates[0].currentLocation);
            Console.WriteLine(stars);
            PerformAction(WORK_A_SHIFT, Housemates[0], null, Housemates[0].currentLocation);
            Console.WriteLine(stars);
            for (int i = 0; i < 10; i += 1)
            {
                PerformAction(BUY_COFFEE, Housemates[0], null, Housemates[0].currentLocation);
                Console.WriteLine(stars);
            }
            

            Console.WriteLine("Press ENTER to Exit");
            Console.ReadLine();
        }
    }
}
