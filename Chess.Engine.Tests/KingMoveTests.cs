using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class KingMoveTests
    {
        [Fact]
        public void King_InTheCenterOfEmptyBoard_HasEightMoves() 
        {

            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.King, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(8, moves.Count);

        }

        [Fact]
        public void King_InCorner_HasThreeMoves() 
        {

            var board = new Board();
            var from = new Square(0, 0);
            board.SetPiece(from, new Piece(PieceType.King, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(3, moves.Count);

        }

        [Fact]
        public void King_CapturesEnemyButNotFriendly() 
        {

            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.King, PieceColor.White));
            board.SetPiece(new Square(4, 5), new Piece(PieceType.Pawn, PieceColor.Black));
            board.SetPiece(new Square(3, 3), new Piece(PieceType.Pawn, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Contains(moves, m => m.To.Equals(new Square(4,5)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(3, 3)));

        }

        [Fact]
        public void King_InStartingPosition_HasNoMoves() 
        {

            var board = Board.CreateStartingPosition();
            var from = new Square(7, 4);

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Empty(moves);

        }
    }
}
