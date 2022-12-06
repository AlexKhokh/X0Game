using ProtoBuf;

namespace App.Entities
{
    [ProtoContract]
    public class GameContext
    {
        [ProtoMember(1)]
        public GameBoard GameBoard { get; set; }
        [ProtoMember(2)]
        public bool? Winner { get; set; }
        [ProtoMember(3)]
        public Status Status { get; set; }
        [ProtoMember(4)]
        public string Descr { get; set; }
    }
}
