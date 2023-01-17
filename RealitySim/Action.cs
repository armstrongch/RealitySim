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
        public string Name { get; private set; }
        public string Description { get; private set; }
        public LOCATION[] ValidLocations { get; private set; }
        bool RequiresTarget;
        bool RequiresWitnesses;

        public Action(string actionName, string actionDesc, LOCATION[] validLocations)
            : this(actionName, actionDesc, validLocations, false, false) { }

        public Action(string actionName, string actionDesc, LOCATION[] validLocations, bool requiresTarget, bool requiresWitnesses)
        {
            this.Name = actionName;
            this.Description = actionDesc;
            this.ValidLocations = validLocations;
            this.RequiresTarget = requiresTarget;
            this.RequiresWitnesses = requiresWitnesses;
        }
    }
}
