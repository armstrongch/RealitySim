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
            /*
            Action ENTER_A_RELATIONSHIP = Actions.Where(a => a.Id == ACTION.ENTER_A_RELATIONSHIP).First();
            Action BREAK_UP = Actions.Where(a => a.Id == ACTION.BREAK_UP).First();
            Action BUY_COFFEE = Actions.Where(a => a.Id == ACTION.BUY_COFFEE).First();
            Action GO_TO_WORK = Actions.Where(a => a.Id == ACTION.GO_TO_WORK).First();
            Action WORK_A_SHIFT = Actions.Where(a => a.Id == ACTION.WORK_A_SHIFT).First();
            Action FLIRT = Actions.Where(a => a.Id == ACTION.FLIRT).First();
            */

            TATTLE_ON_Tests();

            Console.ReadLine();
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
        }
    }
}
