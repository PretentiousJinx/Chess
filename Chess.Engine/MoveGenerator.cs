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

        private static readonly (int dr, int dc)[] RookDirections = 
        {
            (-1, 0), (1, 0), (0, -1), (0,1)
        };

        private static readonly (int dr, int dc)[] BishopDirections = 
        {
            (-1, -1), (-1, 1), (1, -1), (1, 1)
        };

        private static readonly (int dr, int dc)[] QueenDirections = 
        {
            (-1, 0), (1, 0), (0, -1), (0,1),
            (-1, -1), (-1, 1), (1, -1), (1, 1)
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
                PieceType.Rook => GenerateSlidingMoves(board, from, piece.Value.Color, RookDirections),
                PieceType.Bishop => GenerateSlidingMoves(board, from, piece.Value.Color, BishopDirections),
                PieceType.Queen => GenerateSlidingMoves(board, from, piece.Value.Color, QueenDirections),
                _ => new List<Move>()
            
            };

        }

        public static List<Move> GenerateSlidingMoves(Board board, Square from, PieceColor color, (int dr, int dc)[] directions) 
        {
        
            var moves = new List<Move>();

            foreach (var (dr, dc) in directions) 
            {
            
                var to = new Square(from.Row + dr, from.Col + dc);

                while (to.isOnBoard) 
                {

                    Piece? target = board[to];

                    // Check if target square is empty
                    if (target is null)
                    {
                        // If target square is empty continue
                        moves.Add(new Move(from, to));

                    }
                    else 
                    {
                        // Check if target piece is opposing
                        if (target.Value.Color != color) 
                        {
                            // Enemy occupies target square take
                            moves.Add(new Move(from, to));

                        }
                        // Break stops us from capturing our own piece
                        break;
                    }

                    to = new Square(to.Row + dr, to.Col + dc);

                }

            }

            return moves;

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
