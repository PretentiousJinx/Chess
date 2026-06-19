using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class PawnMoveTests
    {
        [Fact]
        public void WhitePawn_OnStartingRank_HadSingleAndDouble() 
        {
        
            var board = new Board();
            var from = new Square(6, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(2, moves.Count);
        }

        [Fact]
        public void WhitePawn_DoubleStepBlocked_OnlyAdvancesOne() 
        {

            var board = new Board();
            var from = new Square(6, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));
            board.SetPiece(new Square(4, 4), new Piece(PieceType.Pawn, PieceColor.Black));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(1, moves.Count);
        }

        [Fact]
        public void Pawn_BlockedDirectlyAhead_CannotAdvanceOrCaptureForward() 
        {

            var board = new Board();
            var from = new Square(6, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));
            board.SetPiece(new Square(5, 4), new Piece(PieceType.Pawn, PieceColor.Black));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Empty(moves);
        }

        [Fact]
        public void WhitePawn_CapturesEnemyDiagonally() 
        {

            var board = new Board();
            var from = new Square(5, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));
            board.SetPiece(new Square(4, 3), new Piece(PieceType.Pawn, PieceColor.Black));
            board.SetPiece(new Square(4, 5), new Piece(PieceType.Pawn, PieceColor.Black));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(3, moves.Count);
            Assert.Contains(moves, m => m.To.Equals(new Square(4, 3)));
            Assert.Contains(moves, m => m.To.Equals(new Square(4, 5)));

        }

        [Fact]
        public void WhitePawn_ReachingLastRank_HasFourPromotions() 
        {

            var board = new Board();
            var from = new Square(1, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.White));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(4, moves.Count);
            Assert.Contains(moves, m => m.Promotion == PieceType.Queen);
            Assert.Contains(moves, m => m.Promotion == PieceType.Bishop);
            Assert.Contains(moves, m => m.Promotion == PieceType.Rook);
            Assert.Contains(moves, m => m.Promotion == PieceType.Knight);

        }

        [Fact]
        public void BlackPawn_OnHomeRank_MovesDownward() 
        {

            var board = new Board();
            var from = new Square(1, 4);
            board.SetPiece(from, new Piece(PieceType.Pawn, PieceColor.Black));

            var moves = MoveGenerator.GenerateMoves(board, from);

            Assert.Equal(2, moves.Count);
            Assert.Contains(moves, m => m.To.Equals(new Square(2, 4)));
            Assert.Contains(moves, m => m.To.Equals(new Square(3, 4)));
        }
    }
}
