using App.Entities;
using App.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace App
{
    public class GameProcessor : IGameProcessor
    {        
        private readonly X0DBContext _context;
        private readonly bool?[,] _extBoard;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);        

        public GameProcessor(X0DBContext context)
        {
            _context = context;            
            _extBoard = new bool?[3, 3];
            InitExtBoard();            
        }
        
        public static int[][] winsCombinations = new int[][] { 
           new int[3] {0,1,2},
           new int[3] {3,4,5},
           new int[3] {6,7,8 },
           new int[3] {0,3,6 },
           new int[3] {1,4,7 },
           new int[3] {2,5,8},
           new int[3] { 0,4,8},
           new int[3] {2,4,6}
        };
        

        private void InitExtBoard()
        {            
            for(int i = 0; i <3; i++)
            {
                for(int j = i; j<3; j++)
                {
                    _extBoard[i,j] = null;
                }
            }
        }

        public async Task<Guid> Create()
        {
            InitExtBoard();
            var game = new Game();
            await Create(game);
            return game.GameId;
        }

        public async Task<GameBoard> Get(Guid id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(x => x.GameId == id)
                ?? throw new Exception("game doesn't exist.");
            return game.ToGameBoard();
        }

        public async Task<GameContext> MakeTurn(Turn turn)
        {            
            var game = await _context.Games.FirstOrDefaultAsync(x => x.GameId == turn.GameId)
                        ?? throw new ArgumentNullException("game doesn't exist.");
            var board = game.ToGameBoard().Board;
            var isGameOver = CheckWin(board, out bool? winner);
            if (isGameOver)
                return new GameContext
                {
                    GameBoard = game.ToGameBoard(),
                    Status = Status.Finished,
                    Winner = winner,
                    Descr = "game is over"
                };
            else
            {
                if (CheckFieldsFill(board))
                    return new GameContext
                    {
                        GameBoard = game.ToGameBoard(),
                        Status = Status.Finished,
                        Winner = winner,
                        Descr = "game is over"
                    };
            }
            try
            {
                await _semaphore.WaitAsync();
                using (var tran = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted))
                {
                    await Task.Delay(5000);
                    try
                    {
                        var turnAllow = CheckTurn(turn, game);
                        if (!turnAllow) throw new CustomException("It's not your turn.");

                        var currentCellValue = game.GetType().GetProperties()
                            .Where(p => p.Name == $"C{turn.Cell}")
                            .Select(p => p.GetValue(game))
                            .FirstOrDefault();

                        if (currentCellValue is not null) throw new CustomException("Cell is already filled");

                        PropertyInfo pi = game.GetType().GetProperty($"C{turn.Cell}");
                        pi.SetValue(game, turn.Value);
                        await _context.SaveChangesAsync();
                        await tran.CommitAsync();
                    }
                    catch (CustomException cex)
                    {
                        await tran.RollbackAsync();
                        return new GameContext
                        {
                            GameBoard = game.ToGameBoard(),
                            Status = Status.InProcess,
                            Winner = null,
                            Descr = cex.Message,
                        };
                    }
                    catch (Exception ex)
                    {
                        await tran.RollbackAsync();
                        throw;
                    }
                }

                var updatedGame = await _context.Games.FirstOrDefaultAsync(x => x.GameId == turn.GameId)
                    ?? throw new Exception("game doesn't exist.");
                isGameOver = CheckWin(updatedGame.ToGameBoard().Board, out bool? updWinner);

                if (isGameOver)
                    return new GameContext
                    {
                        GameBoard = updatedGame.ToGameBoard(),
                        Status = Status.Finished,
                        Winner = updWinner,
                        Descr = "game is over"
                    };
                else
                {
                    if (CheckFieldsFill(board))
                    {
                        return new GameContext
                        {
                            GameBoard = game.ToGameBoard(),
                            Status = Status.Finished,
                            Winner = winner,
                            Descr = "game is over"
                        };
                    }
                    else
                    {
                        return new GameContext
                        {
                            GameBoard = updatedGame.ToGameBoard(),
                            Status = Status.InProcess,
                            Winner = null                            
                        };
                    }
                }
                
            }
            finally
            {
                _semaphore.Release();                
            }            
        }

        private bool CheckTurn(Turn turn, Game game)
        {
            var board = game.ToGameBoard().Board;

            var xcells = board                
                .Where(v => v.HasValue && v.Value == true)
                .Count();                

            var ocells = board
                .Where(x => x.HasValue && x.Value == false)
                .Count();

            if (turn.Value && xcells == ocells) return true;
            if (!turn.Value && xcells > ocells) return true;
            return false;
        }

        private bool CheckWin(bool?[] board, out bool? winner)
        {
            foreach(var combination in winsCombinations)
            {
                if ((board[combination[0]].HasValue && board[combination[1]].HasValue
                    && board[combination[2]].HasValue
                    )
                   && ((bool)board[combination[0]] && (bool)board[combination[1]]
                    && (bool)board[combination[2]]
                    ))
                {
                    winner = (bool)board[combination[0]];
                    return true;
                }                    
            }
            winner = null;
            return false;
        }

        private bool CheckFieldsFill(bool?[] board)
        {            
            var filled = true;
            foreach(var cell in board)
            {
                filled &= cell.HasValue;
            }
            return filled;
        }

        private async Task Create(Game game, CancellationToken ct = default)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync(ct);
        }
    }
}
