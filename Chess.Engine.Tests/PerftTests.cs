using Chess.Engine;
using Xunit;

namespace Chess.Engine.Tests
{
    public class PerftTests
    {
        [Theory]
        [InlineData(1, 20)]
        [InlineData(2, 400)]
        [InlineData(3, 8902)]
        [InlineData(4, 197281)]
        public void StartingPosition_MatchesReferenceCounts(int depth, long expected)
        {
            var board = Board.CreateStartingPosition();

            long nodes = Perft.Count(board, PieceColor.White, depth);

            Assert.Equal(expected, nodes);
        }
    }
}