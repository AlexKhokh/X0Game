using App.Entities;
using App.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {        
        private readonly IGameProcessor _gameProcessor;

        public GameController(IGameProcessor gameProcessor)
        {
            _gameProcessor = gameProcessor ?? throw new ArgumentNullException(nameof(gameProcessor));
        }

        [HttpGet]        
        public async Task<Guid> Create()
        {
            return await _gameProcessor.Create();
        }

        [HttpGet("{id}")]        
        public async Task<GameBoard> Get(Guid id)
        {
            return await _gameProcessor.Get(id);
        }

        [HttpPost("maketurn")]
        public async Task<GameContext> Turn([FromBody]Turn turn)
        {
           return await _gameProcessor.MakeTurn(turn);
        }        
    }
}
