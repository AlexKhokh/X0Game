namespace App.Entities
{
    public class GameBoard
    {
        public Guid GameId { get; set; }

        public bool?[] Board { get; set; }
    }
}
