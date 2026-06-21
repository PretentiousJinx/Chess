namespace Chess.Engine
{
    public static class Perft
    {
        public static long Count(Board board, PieceColor sideToMove, int depth)
        {
            if (depth == 0)
                return 1;

            long nodes = 0;
            PieceColor next = (sideToMove == PieceColor.White) ? PieceColor.Black : PieceColor.White;

            foreach (var move in MoveGenerator.GenerateAllLegalMoves(board, sideToMove))
            {
                Board copy = board.Clone();
                copy.MakeMove(move);
                nodes += Count(copy, next, depth - 1);
            }

            return nodes;
        }
    }
}