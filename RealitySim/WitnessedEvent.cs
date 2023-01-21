using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal class WitnessedEvent
    {
        public Housemate Witness { get; private set; }
        public Housemate Perpetrator { get; private set; }
        public Housemate Victim { get; private set; }
        public Housemate? Target { get; private set; }
        public ACTION Event { get; private set; }
        
        // Witness saw Perpetrator engage in Event with Target. This will impact Victim.
        public WitnessedEvent(Housemate witness, Housemate perp, Housemate victim, Housemate? target, ACTION action)
        {
            this.Witness = witness;
            this.Perpetrator = perp;
            this.Victim = victim;
            this.Target = target;
            this.Event = action;
        }
    }
}
