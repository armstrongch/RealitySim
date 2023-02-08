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
        public void EndOfDayWrapup()
        {
            Console.WriteLine(stars);
            Console.WriteLine($"All of the housemates are asleep. Day {currentDayNum} is over, " +
                $"and episode {currentDayNum} of Gremlin Island is on the air!");
            List<Housemate> OrderedByKarma = Housemates.OrderBy(h => h.Karma * -1).ToList();
            List<Housemate> MostPopular = Housemates.Where(h => h.Karma == OrderedByKarma.First().Karma).ToList();
            List<Housemate> LeastPopular = Housemates.Where(h => h.Karma == OrderedByKarma.Last().Karma).ToList();

            if (MostPopular.Count == 1)
            {
                Housemate fanFavorite = MostPopular.First();
                Console.WriteLine($"In a viewer poll, {fanFavorite.Name} was voted the fan favorite, and earns a $200 bonus.");
                fanFavorite.Cash += 200;
            }
            else
            {
                Console.WriteLine($"In a viewer poll, {GetNames(MostPopular)} were fan favorite housemates, and each earns a $100 bonus.");
                foreach (Housemate h in MostPopular)
                {
                    h.Cash += 100;
                }
            }

            if (LeastPopular.Count == 1)
            {
                Housemate villain = LeastPopular.First();
                Console.WriteLine($"{villain.Name} was the least popular housemate. " +
                    $"The producers love housemates that stir the pot and cause drama, so {villain.Name} earns a $150 bonus.");
                villain.Cash += 150;
            }
            else
            {
                Console.WriteLine($"{GetNames(LeastPopular)} were all equally hated among viewers. The producers love housemates that stir the pot and cause drama, so they each earn a $75 bonus.");
                foreach (Housemate h in LeastPopular)
                {
                    h.Cash += 75;
                }
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Full Viewer Poll Results:");
            int rank = 1;
            foreach (Housemate h in OrderedByKarma)
            {
                string plus = h.Karma > 0 ? "+" : string.Empty;
                Console.WriteLine($"{rank.ToString()}. {h.Name} ({plus}{h.Karma.ToString()})");
                rank += 1;
            }

            Dictionary<Housemate, int> HousemateOpinionRanking = new Dictionary<Housemate, int>();
            foreach (Housemate h in Housemates)
            {
                int ranking = Housemates.Select(x => x.GetOpinionOf(h)).Sum();
                HousemateOpinionRanking.Add(h, ranking);
            }

            Console.WriteLine(string.Empty);
            List<Housemate> OrderedByOpinion = Housemates.OrderBy(h => HousemateOpinionRanking[h] * -1).ToList();
            Console.WriteLine($"Among the housemates, {OrderedByOpinion.First().Name} is the most popular, " +
                $"and {OrderedByOpinion.Last().Name} is the least popular.");
            Console.WriteLine("Full Popularity Ranking:");
            rank = 1;
            foreach (Housemate h in OrderedByOpinion)
            {
                int opinion = HousemateOpinionRanking[h];
                string plus = opinion > 0 ? "+" : string.Empty;
                Console.WriteLine($"{rank.ToString()}. {h.Name} ({plus}{opinion.ToString()})");
                rank += 1;
            }

            Console.WriteLine(string.Empty);
            List<Housemate> OrderedByCash = Housemates.OrderBy(h => h.Cash * -1).ToList();
            Console.WriteLine("Net Worth Ranking:");
            rank = 1;
            foreach (Housemate h in OrderedByCash)
            {
                Console.WriteLine($"{rank.ToString()}. {h.Name} - ${h.Cash.ToString()}.00");
                rank += 1;
            }
        }
    }
}
