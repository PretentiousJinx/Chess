namespace Chess.Engine
{

    public enum PieceColor {White, Black};
    public enum PieceType {Pawn, Knight, Rook, Bishop, King, Queen};
    public readonly struct Piece { 
    
        public PieceType Type { get; }

        public PieceColor Color { get; }

        public Piece(PieceType type, PieceColor color) { 
        
            Type = type;
            Color = color;
        
        }
    }
}
