namespace Chess.Engine
{
        public readonly struct Square 
        {

            public int Row { get; }
            public int Col { get; }

            public Square(int row, int col) 
            {

                Row = row;
                Col = col;

            }

            public bool isOnBoard => Row >= 0 && Row < 8 && Col >= 0 && Col < 8;

    }

        public readonly struct Move
        {

            public Square From { get; }
            public Square To { get; }

            public PieceType? Promotion { get; }

            public Move(Square from, Square to, PieceType? promotion = null)
            {

                From = from;
                To = to;
                Promotion = promotion;

            }

        }

    }
