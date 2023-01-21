
namespace RealitySim
{
    internal static class Enums
    {
        public const int HousemateMaxEnergy = 60;

        public enum LOCATION
        {
            HOUSE,
            WORK,
            CLUB
        }

        public enum ACTION
        {
            WORK_A_SHIFT,
            GO_TO_BED,
            GO_TO_WORK,
            GO_TO_THE_CLUB,
            GO_HOME,
            PUNCH,
            FLIRT,
            ENTER_A_RELATIONSHIP,
            BREAK_UP,
            TATTLE
        }

        public enum RELATIONSHIP
        {
            NEUTRAL,
            FRIENDS,
            ENEMIES,
            DATING,
            ONE_LIKES_TWO,
            TWO_LIKES_ONE
        }
    }
}
