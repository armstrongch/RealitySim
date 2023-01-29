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

            Console.WriteLine(stars);
            PerformAction(ENTER_A_RELATIONSHIP, Housemates[0], Housemates[1], LOCATION.HOUSE);
            Console.WriteLine(stars);
            PerformAction(ENTER_A_RELATIONSHIP, Housemates[1], Housemates[2], LOCATION.HOUSE);
            Console.WriteLine(stars);
            PerformAction(BREAK_UP, Housemates[3], null, LOCATION.HOUSE);
            Console.WriteLine(stars);

            Console.WriteLine("Press ENTER to Exit");
            Console.ReadLine();
        }
    }
}
