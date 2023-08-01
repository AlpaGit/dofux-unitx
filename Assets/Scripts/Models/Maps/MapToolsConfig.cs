namespace Models.Maps
{
    public class MapToolsConfig
    {
        public static readonly MapToolsConfig Dofus2Config = new()
        {
            MapGridWidth  = 14,
            MapGridHeight = 20,
            MinXCoord     = 0,
            MaxXCoord     = 33,
            MinYCoord     = -19,
            MaxYCoord     = 13,
        };

        public int MinYCoord { get; set; }
        public int MinXCoord { get; set; }
        public int MaxYCoord { get; set; }
        public int MaxXCoord { get; set; }
        public int MapGridWidth { get; set; }
        public int MapGridHeight { get; set; }

        public MapToolsConfig()
        {
        }
    }
}