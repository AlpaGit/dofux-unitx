using Models;

namespace Components.Maps
{
    [System.Serializable]
    public class MapBasicInformation
    {
        public long id;
        public long leftNeighbourId;
        public long rightNeighbourId;
        public long topNeighbourId;
        public long bottomNeighbourId;
    
        public SerializableDictionary<uint, uint> identifiedElements = new();
        public SerializableDictionary<ushort, ushort> cells = new();
    }
}