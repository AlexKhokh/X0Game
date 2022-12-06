using App.Entities;
using System.Runtime.CompilerServices;

namespace App
{
    public static class Mapper
    {
        public static GameBoard ToGameBoard(this Game game)
        {
            var gameBoard = new GameBoard();
            gameBoard.Board = new bool?[9];
            gameBoard.GameId = game.GameId;
            gameBoard.Board[0] = game.C0;
            gameBoard.Board[1] = game.C1;
            gameBoard.Board[2] = game.C2;
            gameBoard.Board[3] = game.C3;
            gameBoard.Board[4] = game.C4;
            gameBoard.Board[5] = game.C5;
            gameBoard.Board[6] = game.C6;
            gameBoard.Board[7] = game.C7;
            gameBoard.Board[8] = game.C8;

            return gameBoard;
        }
    }
}
