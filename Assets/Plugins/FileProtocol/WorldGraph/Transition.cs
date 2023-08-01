namespace DofusCoube.FileProtocol.WorldGraph
{
    public sealed class Transition
    {
        public Transition(
            int type,
            int direction,
            int skillId,
            string criterion,
            double transitionMapId,
            int cell,
            double id,
            int toZoneId,
            int fromZoneId,
            long fromMapId,
            long toMapid
        )
        {
            Type            = (TransitionTypeEnum)type;
            Direction       = direction;
            SkillId         = skillId;
            Criterion       = criterion;
            TransitionMapId = transitionMapId;
            Cell            = cell;
            Id              = id;
            ToZoneId        = toZoneId;
            FromZoneId      = fromZoneId;
            FromMapId       = fromMapId;
            ToMapid         = toMapid;
        }

        public TransitionTypeEnum Type { get; set; }

        public int Direction { get; set; }

        public int SkillId { get; set; }

        public string Criterion { get; set; }

        public double TransitionMapId { get; set; }

        public int Cell { get; set; }

        public double Id { get; set; }

        public int ToZoneId { get; set; }

        public int FromZoneId { get; set; }

        public long FromMapId { get; }
        public long ToMapid { get; }

        public override string ToString() =>
            "Transition{_type=" + Type + ",_direction=" + Direction + ",_skillId=" + SkillId + ",_criterion=" + Criterion +
            ",_transitionMapId=" + TransitionMapId +
            ",_cell=" + Cell + ",_id=" + Id + "}";
    }
}