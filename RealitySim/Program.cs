using System;
using static RealitySim.Input;
namespace RealitySim;
class Program
{   
    static void Main(string[] args)
    {
        bool testMode = false;
        bool stressTestMode = false;
        

        if (!stressTestMode)
        {
            Console.WriteLine("Welcome to GREMLIN ISLAND: The world's finest reality show simulator.");
            Console.WriteLine("Up to 5 players will play as young, charistmatic housemates whose lives are televised 24-7.");
            int numPlayers = int.Parse(GetInput(
                "How many human players are there?",
                new string[] { "0", "1", "2", "3", "4", "5" }
            ));
            Game game = new Game(numPlayers, testMode, true);
        }
        else
        {
            for (int i = 0; i < 1000; i += 1)
            {
                try
                {
                    Game game = new Game(0, false, false);
                }
                catch (NotImplementedException ex)
                {
                    //do nothing
                }
            }
        }

    }
}