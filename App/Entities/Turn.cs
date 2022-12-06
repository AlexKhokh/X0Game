using ProtoBuf;

namespace App.Entities
{
    [ProtoContract]
    public class Turn
    {
        [ProtoMember(1)]
        public Guid GameId { get; set; }
        [ProtoMember(2)]
        public bool Value { get; set; }
        [ProtoMember(3)]
        public int Cell { get; set; }
    }
}
