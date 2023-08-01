using System.Collections.Generic;

namespace DofusCoube.FileProtocol.WorldGraph
{
    public sealed class Edge
    {
        public Edge(Vertex from, Vertex to)
        {
            From        = from;
            To          = to;
            Transitions = new();
        }

        public Vertex From { get; set; }

        public Vertex To { get; set; }

        public List<Transition> Transitions { get; set; }

        public void AddTransition(int dir, int type, int skill, string criterion, double transitionMapId, int cell,
            double id) =>
            Transitions.Add(new(type, dir, skill, criterion, transitionMapId, cell, id, To.ZoneId, From.ZoneId,
                (long)From.MapId, (long)To.MapId));


        public int CountTransitionWithValidDirections()
        {
            var count = 0;

            foreach (var transition in Transitions)
            {
                if (transition.Direction != -1)
                {
                    count++;
                }
            }

            return count;
        }

        public override string ToString() =>
            "Edge{_from=" + From + ",_to=" + To + ",_transitions=" + Transitions + "}";
    }
}