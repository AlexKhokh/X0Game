using App.Entities;

namespace App.Interfaces
{
    public interface IGameProcessor
    {
       public Task<Guid> Create();

       public Task<GameBoard> Get(Guid id);

        public Task<GameContext> MakeTurn(Turn turn);       
    }
}
