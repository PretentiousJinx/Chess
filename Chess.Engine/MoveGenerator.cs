using System.Collections.Generic;

namespace Chess.Engine
{
    public static class MoveGenerator 
    {

        private static readonly (int dr, int dc)[] KnightOffsets =
        {

            (-2, -1), (-2, 1), (-1, -2), (-1, 2), 
            (1, -2), (1, 2), (2, -1), (2, 1)
        };

        public static List<Move> GenerateMoves(Board board, Square from) 
        {

            Piece? piece = board[from];
            if (piece is null) 
            {

                return new List<Move>();

            }

            return piece.Value.Type switch
            {
                PieceType.Knight => GenerateKnightMoves(board, from, piece.Value.Color),
                _ => new List<Move>()
            
            };

        }

        public static List<Move> GenerateKnightMoves(Board board, Square from, PieceColor color) 
        {
        
            var moves = new List<Move>();

            foreach (var (dr, dc) in KnightOffsets) 
            {
            
                var to = new Square(from.Row + dr, from.Col + dc);

                if (!to.isOnBoard) 
                {

                    continue;

                }

                Piece? target = board[to];
                if (target is null || target.Value.Color != color) 
                {

                    moves.Add(new Move(from, to));

                }

            }

            return moves;
        }
    }
}
