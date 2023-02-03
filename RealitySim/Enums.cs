
namespace RealitySim
{
    internal static class Enums
    {
        public const int HousemateMaxEnergy = 60;

        public enum LOCATION
        {
            HOUSE,
            WORK,
            CLUB,
            DREAMLAND
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
            TATTLE_TO,
            BUY_COFFEE,
            DRINK_COFFEE,
            BUY_A_SHOT,
            TAKE_A_SHOT,
            BUY_A_ROUND_OF_SHOTS,
        }

        public enum RELATIONSHIP
        {
            FRIEND,
            ENEMY,
            LIKE_AND_DISLIKED_BY,
            DISLIKE_AND_LIKED_BY,
            DATING
        }

        public enum CPU_TARGET_TYPE
        {
            RANDOM,
            RANDOM_FRIENDLY,
            RANDOM_ENEMY,
            BEST_FRIEND,
            BEST_FRIEND_EXCLUDING_SO,
            WORST_ENEMY,
            BEST_FRIEND_WITH_DIRT,
            NONE
        }
    }
}
