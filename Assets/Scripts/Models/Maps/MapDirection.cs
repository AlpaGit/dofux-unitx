namespace Models.Maps
{
    public class MapDirection
    {
        public const int InvalidDirection = -1;

        public const int DefaultDirection = 1;

        public const int MapOrthogonalDirectionsCount = 4;

        public const int MapCardinalDirectionsCount = 4;

        public const int MapInterCardinalDirectionsCount = 8;

        public const int MapInterCardinalDirectionsHalfCount = 4;

        public static int[] MapCardinalDirections = { 0, 2, 4, 6, };

        public static int[] MapOrthogonalDirections = { 1, 3, 5, 7, };

        public static readonly int[] MapDirections = { 0, 1, 2, 3, 4, 5, 6, 7, };

        public static bool IsValidDirection(int direction)
        {
            return direction >= 0 && direction < MapDirections.Length;
        }

        public static int GetOppositeDirection(int direction)
        {
            return direction ^ 4;
        }

        public static bool IsCardinal(int direction)
        {
            return (direction & 1) == 0;
        }

        public static bool IsOrthogonal(int direction)
        {
            return (direction & 1) == 1;
        }
        
        public static bool RequireFlipSide(int direction)
        {
            return direction is 3 or 7;
        }
    }
}