using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class KnightMoveTests
    {
        [Fact]
        public void Knight_InCenterOfEmptyBoard_HasEightMoves() 
        {

            // Arrange
            var board = new Board();
            var from = new Square(4, 4);

            board.SetPiece(from, new Piece(PieceType.Knight, PieceColor.White));

            // Act
            var moves = MoveGenerator.GenerateMoves(board, from);


            // Assert
            Assert.Equal(8, moves.Count);

        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 7)]
        [InlineData(7, 0)]
        [InlineData(7, 7)]
        public void Knight_InAnyCorner_HasTwoMoves(int row, int col) 
        {

            var board = new Board();
            var from = new Square(row, col);

            board.SetPiece(from, new Piece(PieceType.Knight, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(2, moves.Count);

        }

        [Fact]
        public void Knight_CannotCaptureOwnPiece() 
        {

            var board = new Board();
            var from = new Square(4, 4);

            board.SetPiece(from, new Piece(PieceType.Knight, PieceColor.White));

            var friendly = new Square(6, 5);
            board.SetPiece(friendly, new Piece(PieceType.Pawn, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(7, moves.Count);
            Assert.DoesNotContain(moves, m => m.To.Equals(friendly));

        }

        [Fact]
        public void Knight_OnStartingSquare_HasTwoMoves() 
        {

            var board = Board.CreateStartingPosition();
            var from = new Square(7, 1);

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(2, moves.Count);
        }
    }
}
