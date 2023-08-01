namespace DofusCoube.FileProtocol.WorldGraph
{
    public enum TransitionTypeEnum
    {
        Unspecified = 0,
        Scroll = 1,
        ScrollAction = 2,
        MapEvent = 4,
        MapAction = 8,
        MapObstacle = 16,
        Interactive = 32,
        NpcAction = 64,
    }

    public static class TransitionType
    {
        public static TransitionTypeEnum FromName(string name) =>
            name switch
            {
                "SCROLL"        => TransitionTypeEnum.Scroll,
                "SCROLL_ACTION" => TransitionTypeEnum.ScrollAction,
                "MAP_EVENT"     => TransitionTypeEnum.MapEvent,
                "MAP_ACTION"    => TransitionTypeEnum.MapAction,
                "MAP_OBSTACLE"  => TransitionTypeEnum.MapObstacle,
                "INTERACTIVE"   => TransitionTypeEnum.Interactive,
                "NPC_ACTION"    => TransitionTypeEnum.NpcAction,
                _               => TransitionTypeEnum.Unspecified,
            };
    }
}