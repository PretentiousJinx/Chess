using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class LegalMoveTests
    {
        [Fact]
        public void StartingPosition_NeitherKingInCheck()
        {
            var board = Board.CreateStartingPosition();
            Assert.False(MoveGenerator.isInCheck(board, PieceColor.White));
            Assert.False(MoveGenerator.isInCheck(board, PieceColor.Black));
        }

        [Fact]
        public void BishopPinnedByRook_HasNoLegalMoves()
        {
            var board = new Board();
            board.SetPiece(new Square(4, 0), new Piece(PieceType.King, PieceColor.White));
            board.SetPiece(new Square(4, 3), new Piece(PieceType.Bishop, PieceColor.White));
            board.SetPiece(new Square(4, 7), new Piece(PieceType.Rook, PieceColor.Black));

            var moves = MoveGenerator.GenerateLegalMoves(board, new Square(4, 3));

            Assert.Empty(moves); // any diagonal move leaves the rank and exposes the king
        }

        [Fact]
        public void King_CannotMoveIntoAttackedSquares()
        {
            var board = new Board();
            board.SetPiece(new Square(4, 4), new Piece(PieceType.King, PieceColor.White));
            board.SetPiece(new Square(0, 3), new Piece(PieceType.Rook, PieceColor.Black)); // controls column 3

            var moves = MoveGenerator.GenerateLegalMoves(board, new Square(4, 4));

            Assert.Equal(5, moves.Count); // 8 neighbors minus the 3 on the rook's file
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(4, 3)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(3, 3)));
            Assert.DoesNotContain(moves, m => m.To.Equals(new Square(5, 3)));
        }

        [Fact]
        public void WhenKingInCheck_OnlyTheBlockingMoveIsLegal()
        {
            var board = new Board();
            board.SetPiece(new Square(4, 4), new Piece(PieceType.King, PieceColor.White));
            board.SetPiece(new Square(4, 0), new Piece(PieceType.Rook, PieceColor.Black)); // checks along the rank
            board.SetPiece(new Square(1, 2), new Piece(PieceType.Rook, PieceColor.White));  // can interpose at (4,2)

            Assert.True(MoveGenerator.isInCheck(board, PieceColor.White));

            var rookMoves = MoveGenerator.GenerateLegalMoves(board, new Square(1, 2));

            Assert.Single(rookMoves);                         // only blocking resolves the check
            Assert.Equal(new Square(4, 2), rookMoves[0].To);
        }
    }
}