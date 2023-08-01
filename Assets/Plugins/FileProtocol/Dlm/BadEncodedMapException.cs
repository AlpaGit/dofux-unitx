using System;

namespace DofusCoube.FileProtocol.Dlm
{
    public sealed class BadEncodedMapException : Exception
    {
        public BadEncodedMapException(Exception exception, long mapId)
            : base("Bad encoded map", exception) =>
            MapId = mapId;

        private long MapId { get; }
    }
}