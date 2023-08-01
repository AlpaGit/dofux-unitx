namespace DofusCoube.FileProtocol.WorldGraph
{
    public sealed class Vertex
    {
        public Vertex(double mapId, int zoneId)
        {
            MapId  = mapId;
            ZoneId = zoneId;
        }

        public double MapId { get; set; }

        public int ZoneId { get; set; }

        public string Uid => GetVertexUid(MapId, ZoneId);

        public static string GetVertexUid(double mapId, int zoneId) =>
            mapId + "|" + zoneId;

        public static double GetMapId(string vertexUid) =>
            double.Parse(vertexUid.Split('|')[0]);

        public static int GetZoneId(string vertexUid) =>
            int.Parse(vertexUid.Split('|')[1]);

        public override string ToString() =>
            "Vertex{_mapId=" + MapId + ",_zoneId" + ZoneId + "}";
    }
}