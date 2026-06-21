using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chess.Engine;
using System.Collections.Generic;

namespace Chess
{
    public class Game1 : Game
    {
        // Board Size definition
        private const int TileSize = 80;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private SpriteFont _font;

        private MouseState _previousMouse;
        private Board _board;
        private PieceColor _sideToMove = PieceColor.White;
        private Square? _selectedSquare;
        private List<Move> _legalMoves = new();
        private Square? _checkedKingSquare;
        private bool _gameOver;


        // Board color
        private static readonly Color White = new(240, 217, 181);
        private static readonly Color Black = new(181, 136, 99);
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialize board graphics
            _graphics.PreferredBackBufferWidth = 8 * TileSize;
            _graphics.PreferredBackBufferHeight = 8 * TileSize;
            _graphics.ApplyChanges();

            _board = Board.CreateStartingPosition();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _font = Content.Load<SpriteFont>("PieceFont");
        }

        private void HandleClick(int pixelX, int pixelY)
        {

            var client = Window.ClientBounds;
            int boardX = pixelX * (8 * TileSize) / client.Width;
            int boardY = pixelY * (8 * TileSize) / client.Height;

            int col = boardX / TileSize;
            int row = boardY / TileSize;

            if (col < 0 || col > 7 || row < 0 || row > 7)
            {

                // Clicked outside the board
                return;

            }

            var clicked = new Square(row, col);

            if (_selectedSquare is not null)
            {
                Move? chosen = FindLegalMove(clicked);
                if (chosen is Move move)
                {

                    _board.MakeMove(move);
                    _sideToMove = Opponent(_sideToMove);
                    ClearSelection();
                    UpdateGameState();
                    return;
                
                }

            }

            Piece? piece = _board[clicked];

            if (piece is not null)
            {

                _selectedSquare = clicked;
                _legalMoves = MoveGenerator.GenerateLegalMoves(_board, clicked);

            }
            else
                ClearSelection();
                
        }

        private Move? FindLegalMove(Square to) 
        {
        
            Move? fallback = null;

            foreach (var move in _legalMoves) 
            {

                if (!move.To.Equals(to)) 
                    continue;

                if (move.Promotion == PieceType.Queen)
                    return move;
                fallback ??= move;
            
            }

            return fallback;
        
        }

        private void ClearSelection() 
        {

            _selectedSquare = null;
            _legalMoves.Clear();
        
        }

        private void UpdateGameState() 
        {

            _checkedKingSquare = null;

            bool inCheck = MoveGenerator.isInCheck(_board, _sideToMove);
            if (inCheck)
                _checkedKingSquare = FindTheKing(_sideToMove);

            var moves = MoveGenerator.GenerateAllLegalMoves(_board, _sideToMove);
            if (moves.Count == 0)
            {

                _gameOver = true;
                Window.Title = inCheck
                    ? $"Checkmate - {Opponent(_sideToMove)} wins!"
                    : "Stalemate - draw";

            }
            else 
            {

                Window.Title = inCheck
                    ? $"{_sideToMove} to move - Check!"
                    : $"{_sideToMove} to move";
            
            }
        
        }

        private Square? FindTheKing(PieceColor color) 
        {

            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++) 
                {

                    Piece? p = _board[row, col];
                    if (p is not null && p.Value.Type == PieceType.King && p.Value.Color == color)
                        return new Square(row, col);


                }

            return null;

        }

        private static PieceColor Opponent(PieceColor color)
            => color == PieceColor.White ? PieceColor.White : color;

        private static string GlyphFor(PieceType type) => type switch
        {

            PieceType.Pawn => "P",
            PieceType.Knight => "N",
            PieceType.Rook => "R",
            PieceType.Bishop => "B",
            PieceType.Queen => "Q",
            PieceType.King => "K",
            _ => "?"
        };


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState mouse = Mouse.GetState();
            

            if (mouse.LeftButton == ButtonState.Pressed && 
                _previousMouse.LeftButton == ButtonState.Released) {

                System.Diagnostics.Debug.WriteLine($"Click Detected at: {mouse.X}, {mouse.Y}");
                HandleClick(mouse.X, mouse.Y);
            
            }
            _previousMouse = mouse;
            base.Update(gameTime);
        
        }

        private void DrawMoveDot(Rectangle tile) 
        {

            int dot = TileSize / 4;
            var dotRect = new Rectangle(
                tile.X + (TileSize - dot) / 2,
                tile.Y + (TileSize - dot) / 2,
                dot, dot
                );
            _spriteBatch.Draw(_pixel, dotRect, Color.Black * 0.35f);
        
        }

        private bool isLegalDestination(int row, int col) 
        {

            foreach (var move in _legalMoves)
                if (move.To.Row == row && move.To.Col == col)
                    return true;
            return false;
        
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            for (int row = 0; row < 8; row++) {

                for (int col = 0; col < 8; col++) { 
                
                    var rect = new Rectangle( col * TileSize, row * TileSize, TileSize, TileSize );

                    bool isLight = (row + col) % 2 == 0;
                    _spriteBatch.Draw(_pixel, rect, isLight ? White : Black);

                    if (_checkedKingSquare is Square ck && ck.Row == row && ck.Col == col)
                        _spriteBatch.Draw(_pixel, rect, Color.Red * 0.5f);

                    if (_selectedSquare is Square sel && sel.Row == row && sel.Col == col)
                        _spriteBatch.Draw(_pixel, rect, Color.Yellow * 0.45f);

                    Piece? piece = _board[row, col];
                    if (piece is not null)
                    {

                        string Glyph = GlyphFor(piece.Value.Type);
                        Color textColor = piece.Value.Color == PieceColor.White ? Color.White : Color.Black;

                        Vector2 size = _font.MeasureString(Glyph);

                        var pos = new Vector2(

                            rect.X + (TileSize - size.X) / 2f,
                            rect.Y + (TileSize - size.Y) / 2f

                            );

                        _spriteBatch.DrawString(_font, Glyph, pos, textColor);

                    }

                    if (isLegalDestination(row, col)) 
                    {

                        if (piece is null)
                            DrawMoveDot(rect);
                        else
                            _spriteBatch.Draw(_pixel, rect, Color.Red * 0.4f);
                    
                    }

                }
            
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
