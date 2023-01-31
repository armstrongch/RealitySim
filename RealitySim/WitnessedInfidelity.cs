using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal class WitnessedInfidelity
    {
        public Housemate Perpetrator { get; private set; }
        public Housemate Victim { get; private set; }
        public Housemate? Target { get; private set; }
        
        // Witness saw Perpetrator engage in Event with Target. This will impact Victim.
        public WitnessedInfidelity(Housemate perp, Housemate victim, Housemate? target)
        {
            this.Perpetrator = perp;
            this.Victim = victim;
            this.Target = target;
        }
    }
}
