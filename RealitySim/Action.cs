using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealitySim.Enums;

namespace RealitySim
{
    internal class Action
    {
        public ACTION Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public LOCATION[] ValidLocations { get; private set; }
        public bool RequiresTarget { get; private set; }
        public int EnergyCost { get; set; }

        public Action(ACTION Id, string actionName, string actionDesc, LOCATION[] validLocations)
            : this(Id, actionName, actionDesc, validLocations, false, 1) { }

        public Action(ACTION id, string actionName, string actionDesc, LOCATION[] validLocations, bool requiresTarget, int energyCost)
        {
            this.Id = id;
            this.Name = actionName;
            this.Description = actionDesc;
            this.ValidLocations = validLocations;
            this.RequiresTarget = requiresTarget;
            this.EnergyCost = energyCost;
        }
    }
}
