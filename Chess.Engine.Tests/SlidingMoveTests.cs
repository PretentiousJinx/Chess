using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class SlidingMoveTests
    {
        [Fact]
        public void Rook_OnEmptyBoard_HasFourteenMoves() 
        {
        
            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.Rook, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(14, moves.Count);

        }

        [Fact]
        public void Bishop_InCorner_HasSevenMoves() 
        {

            var board = new Board();
            var from = new Square(0, 0);
            board.SetPiece(from, new Piece(PieceType.Bishop, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(7, moves.Count);

        }

        [Fact]
        public void Queen_InCenterOfEmptyBoard_HasTwentySevenMoves() 
        {

            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.Queen, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(27, moves.Count);
        }

        [Fact]
        public void Rook_StopsBeforeFriendlyPiece() 
        {

            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.Rook, PieceColor.White));
            board.SetPiece(new Square(4, 6), new Piece(PieceType.Pawn, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Contains(moves, m => m.To.Equals(new Square(4, 5)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(4, 6)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(4, 7)));
        }

        [Fact]
        public void Rook_CapturesEnemyButNotBeyond() 
        {

            var board = new Board();
            var from = new Square(4, 4);
            board.SetPiece(from, new Piece(PieceType.Rook, PieceColor.White));
            board.SetPiece(new Square(4, 6), new Piece(PieceType.Pawn, PieceColor.Black));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Contains(moves, m => m.To.Equals(new Square(4, 6)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(4, 7)));
        }

        [Fact]
        public void Rook_OnStartingSquare_IsBoxedIn() 
        {

            var board = Board.CreateStartingPosition();
            var from = new Square(7, 0);

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Empty(moves);
        }
    }
}
