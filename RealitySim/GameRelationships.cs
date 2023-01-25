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
        private List<(Housemate, RELATIONSHIP)> BuildRelationshipMatrix(Housemate self)
        {
            List<(Housemate, RELATIONSHIP)> RelationshipMatrix = new List<(Housemate, RELATIONSHIP)>();
            
            foreach (Housemate other in Housemates.Where(h => h != self))
            {
                bool i_like_them = self.HasPositiveOpinionOf(other);
                bool they_like_me = other.HasPositiveOpinionOf(self);
                bool dating = GetSignificantOther(self) == other;

                RELATIONSHIP rel;

                if (dating)
                {
                    rel = RELATIONSHIP.DATING;
                }
                else if (i_like_them)
                {
                    if (they_like_me)
                    {
                        rel = RELATIONSHIP.FRIEND;
                    }
                    else
                    {
                        rel = RELATIONSHIP.LIKE_AND_DISLIKED_BY;
                    }
                }
                else
                {
                    if (they_like_me)
                    {
                        rel = RELATIONSHIP.DISLIKE_AND_LIKED_BY;
                    }
                    else
                    {
                        rel = RELATIONSHIP.ENEMY;
                    }
                }

                RelationshipMatrix.Add((other, rel));
            }

            return RelationshipMatrix;
        }
    }
}
