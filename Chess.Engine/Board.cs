using System;

namespace Chess.Engine
{
    public class Board 
    {

        private readonly Piece?[,] _squares = new Piece?[8, 8];

        public Piece? this[int row, int col] => _squares[row, col];

        public Piece? this[Square square] => _squares[square.Row, square.Col];

        public void SetPiece(Square square, Piece? piece) 
            => _squares[square.Row, square.Col] = piece;

        public static Board CreateStartingPosition() 
        {

            char[,] layout =
            {

            {'r', 'n', 'b', 'q', 'k', 'b', 'n', 'r' },
            {'p', 'p', 'p', 'p', 'p', 'p', 'p', 'p' },
            {'.', '.', '.', '.', '.', '.', '.', '.' },
            {'.', '.', '.', '.', '.', '.', '.', '.' },
            {'.', '.', '.', '.', '.', '.', '.', '.' },
            {'.', '.', '.', '.', '.', '.', '.', '.' },
            {'P', 'P', 'P', 'P', 'P', 'P', 'P', 'P' },
            {'R', 'N', 'B', 'Q', 'K', 'B', 'N', 'R' },

            };

            var board = new Board();

            for (int row = 0; row < 8; row++) 
            {

                for (int col = 0; col < 8; col++) 
                {

                    board._squares[row, col] = PieceFromChar(layout[row, col]);

                }

            }

            return board;

        }

        public void Move(int fromRow, int fromCol, int toRow, int toCol)
        {

            _squares[toRow, toCol] = _squares[fromRow, fromCol];
            _squares[fromRow, fromCol] = null;

        }

        public Board Clone() 
        {
            var copy = new Board();
            Array.Copy(_squares, copy._squares, _squares.Length);
            return copy;
        }

        public void MakeMove(Move move) 
        {

            Piece? moving = _squares[move.From.Row, move.From.Col];
            _squares[move.From.Row, move.From.Col] = null;

        if (move.Promotion is PieceType promotion && moving is Piece p)
            _squares[move.To.Row, move.To.Col] = new Piece(promotion, p.Color);
        else
            _squares[move.To.Row, move.To.Col] = moving;
        
        }

        private static Piece? PieceFromChar(char c) 
        {

            if (c == '.') return null;
        
            PieceColor color = char.IsUpper(c) ? PieceColor.White : PieceColor.Black;
            PieceType type = char.ToLower(c) switch
            {

                'p' => PieceType.Pawn,
                'r' => PieceType.Rook,
                'n' => PieceType.Knight,
                'k' => PieceType.King,
                'q' => PieceType.Queen,
                'b' => PieceType.Bishop,
                _ => throw new ArgumentException($"Unknown piece char: {c}")
            };
            return new Piece(type, color);
        }

    }

}
