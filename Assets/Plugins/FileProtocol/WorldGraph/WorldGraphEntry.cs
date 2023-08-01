using System.Collections.Generic;
using System.IO;
using DofusCoube.ThirdParty.IO.Binary;

namespace DofusCoube.FileProtocol.WorldGraph
{
    public sealed class WorldGraphEntry
    {
        public WorldGraphEntry(string uri)
        {
            if (!File.Exists(uri))
            {
                throw new($"File {uri} doesn't exist");
            }

            FilePath = uri;
            Initialize();
        }

        public Dictionary<string, Vertex> Vertices { get; } = new();

        public Dictionary<string, Edge> Edges { get; } = new();

        public Dictionary<string, List<Edge>> OutgoingEdges { get; } = new();

        public string FilePath { get; }

        private void Initialize()
        {
            var reader = new BigEndianReader(File.ReadAllBytes(FilePath));

            var edgeCount = reader.ReadInt32();

            for (var i = 0; i < edgeCount; i++)
            {
                var from = AddVertex(reader.ReadDouble(), reader.ReadInt32());
                var to   = AddVertex(reader.ReadDouble(), reader.ReadInt32());
                var edge = AddEdge(from, to);

                if (edge == null)
                {
                    continue;
                }

                var transitionCount = reader.ReadInt32();
                for (var j = 0; j < transitionCount; j++)
                {
                    edge.AddTransition(
                        reader.ReadUInt8(),
                        reader.ReadUInt8(),
                        reader.ReadInt32(),
                        reader.ReadStringBytes((ushort)reader.ReadInt32()),
                        reader.ReadDouble(),
                        reader.ReadInt32(),
                        reader.ReadDouble());
                }
            }
        }

        private Vertex AddVertex(double mapId, int zone)
        {
            Vertex? vertex    = null;
            var     vertexUid = Vertex.GetVertexUid(mapId, zone);

            if (Vertices.ContainsKey(vertexUid))
            {
                vertex = Vertices[vertexUid];
            }

            if (vertex != null)
            {
                return vertex;
            }

            vertex = new(mapId, zone);
            Vertices.Add(vertexUid, vertex);

            return vertex;
        }

        private Edge? AddEdge(Vertex from, Vertex to)
        {
            var edge = GetEdge(from, to);
            if (edge != null)
            {
                return edge;
            }

            if (!DoesVertexExist(from) || !DoesVertexExist(to))
            {
                return null;
            }

            edge = new(from, to);

            var internalId = GetInternalEdgeId(from, to);
            Edges.Add(internalId, edge);


            List<Edge>? outgoing = null;
            if (OutgoingEdges.ContainsKey(from.Uid))
            {
                outgoing = OutgoingEdges[from.Uid];
            }

            if (outgoing == null)
            {
                outgoing = new();
                OutgoingEdges.Add(from.Uid, outgoing);
            }

            outgoing.Add(edge);
            return edge;
        }

        private Edge? GetEdge(Vertex from, Vertex to)
        {
            var internalId = GetInternalEdgeId(from, to);

            return Edges.ContainsKey(internalId) ? Edges[internalId] : null;
        }

        private static string GetInternalEdgeId(Vertex from, Vertex to) =>
            from.Uid + "|" + to.Uid;

        private bool DoesVertexExist(Vertex v) =>
            Vertices.ContainsKey(v.Uid);
    }
}