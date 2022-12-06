using System.ComponentModel.DataAnnotations;

namespace App.Entities
{
    public class Game
    {
        public Game() { GameId = Guid.NewGuid(); }

        [Key]
        public Guid GameId { get; set; }
        public bool? C0 { get; set; }
        public bool? C1 { get; set; }
        public bool? C2 { get; set; }
        public bool? C3 { get; set; }
        public bool? C4 { get; set; }
        public bool? C5 { get; set; }
        public bool? C6 { get; set; }
        public bool? C7 { get; set; }
        public bool? C8 { get; set; }
    }
}
