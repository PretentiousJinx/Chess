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
                PieceType.King => GenerateKingMoves(board, from, piece.Value.Color),
                PieceType.Pawn => GeneratePawnMoves(board, from, piece.Value.Color),
                _ => new List<Move>()

            };

        }

        private static List<Move> GeneratePawnMoves(Board board, Square from, PieceColor color)
        {

            var moves = new List<Move>();

            int forward = (color == PieceColor.White) ? -1 : 1;
            int startRow = (color == PieceColor.White) ? 6 : 1;
            int promotionRow = (color == PieceColor.White) ? 0 : 7;

            var oneAhead = new Square(from.Row + forward, from.Col);
            if (oneAhead.isOnBoard && board[oneAhead] is null)
            {

                AddPawnMove(moves, from, oneAhead, promotionRow);


                if (from.Row == startRow)
                {

                    var twoAhead = new Square(from.Row + 2 * forward, from.Col);
                    if (board[twoAhead] is null)
                    {

                        moves.Add(new Move(from, twoAhead));

                    }

                }

            }

            foreach (int dc in new[] { -1, 1 })
            {

                var diagonal = new Square(from.Row + forward, from.Col + dc);

                if (!diagonal.isOnBoard)
                    continue;

                Piece? target = board[diagonal];
                if (target is not null && target.Value.Color != color)
                    AddPawnMove(moves, from, diagonal, promotionRow);

            }

            return moves;

        }

        private static void AddPawnMove(List<Move> moves, Square from, Square to, int promotionRow)
        {

            if (to.Row == promotionRow)
            {

                moves.Add(new Move(from, to, PieceType.Queen));
                moves.Add(new Move(from, to, PieceType.Rook));
                moves.Add(new Move(from, to, PieceType.Bishop));
                moves.Add(new Move(from, to, PieceType.Knight));
            }
            else
            {

                moves.Add(new Move(from, to));
            }
        }

        private static List<Move> GenerateSlidingMoves(Board board, Square from, PieceColor color, (int dr, int dc)[] directions)
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
        private static List<Move> GenerateKnightMoves(Board board, Square from, PieceColor color)
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

        private static List<Move> GenerateKingMoves(Board board, Square from, PieceColor color)
        {
            var moves = new List<Move>();

            // Same eight directions as the queen — the king just doesn't slide.
            foreach (var (dr, dc) in QueenDirections)
            {
                var to = new Square(from.Row + dr, from.Col + dc);
                if (!to.isOnBoard)
                    continue;

                Piece? target = board[to];
                if (target is null || target.Value.Color != color)
                    moves.Add(new Move(from, to));
            }

            return moves;
        }

        public static bool isSquareAttacked(Board board, Square target, PieceColor byColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = board[row, col];
                    if (piece is null || piece.Value.Color != byColor)
                        continue;

                    var from = new Square(row, col);

                    if (piece.Value.Type == PieceType.Pawn)
                    {
                        // Pawns attack only their two forward diagonals — not where they move.
                        int forward = (byColor == PieceColor.White) ? -1 : 1;
                        if (target.Row == from.Row + forward &&
                            (target.Col == from.Col - 1 || target.Col == from.Col + 1))
                            return true;
                    }
                    else
                    {
                        // Every other piece attacks exactly the squares it can move to.
                        foreach (var move in GenerateMoves(board, from))
                            if (move.To.Equals(target))
                                return true;
                    }
                }
            }

            return false;
        }

        public static bool isInCheck(Board board, PieceColor color)
        {
            Square king = FindKing(board, color);
            PieceColor enemy = (color == PieceColor.White) ? PieceColor.Black : PieceColor.White;
            return isSquareAttacked(board, king, enemy);
        }

        public static List<Move> GenerateLegalMoves(Board board, Square from)
        {
            var legal = new List<Move>();
            Piece? piece = board[from];
            if (piece is null)
                return legal;

            PieceColor color = piece.Value.Color;

            foreach (var move in GenerateMoves(board, from))
            {
                Board copy = board.Clone();
                copy.MakeMove(move);
                if (!isInCheck(copy, color))   // the move is legal only if it doesn't expose our king
                    legal.Add(move);
            }

            return legal;
        }

        public static List<Move> GenerateAllLegalMoves(Board board, PieceColor color)
        {
            var moves = new List<Move>();

            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    Piece? piece = board[row, col];
                    if (piece is null || piece.Value.Color != color)
                        continue;

                    moves.AddRange(GenerateLegalMoves(board, new Square(row, col)));
                }

            return moves;
        }

        private static Square FindKing(Board board, PieceColor color)
        {
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                {
                    Piece? p = board[row, col];
                    if (p is not null && p.Value.Type == PieceType.King && p.Value.Color == color)
                        return new Square(row, col);
                }

            throw new System.InvalidOperationException($"No {color} king on the board.");
        }
    }

}
