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
        public Dictionary<(Housemate, Housemate), RELATIONSHIP> RelationshipMatrix { get; private set; }
        
        private void BuildRelationshipMatrix()
        {
            for (int i = 0; i < Housemates.Count; i += 1)
            {
                Housemate housemate_one = Housemates[i];
                for (int j = i+1; j < Housemates.Count; j += 1)
                {
                    Housemate housemate_two = Housemates[j];
                    
                }
            }
        }
    }
}
