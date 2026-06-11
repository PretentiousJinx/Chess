using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chess.Engine;

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
        private Point? _selectedSquare;


        // Board color
        private static readonly Color White = new(240, 217, 181);
        private static readonly Color Black = new(181, 136, 99);

        private Board _board;

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

            var Clicked = new Point(col, row);

            if (_selectedSquare is null)
            {

                if (_board[row, col] != null)
                {

                    _selectedSquare = Clicked;

                }

            }
            else
            {

                Point From = _selectedSquare.Value;

                if (Clicked != From)
                {

                    _board.Move(From.Y, From.X, Clicked.Y, Clicked.X);

                }

                _selectedSquare = null;
            }

        }

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            for (int row = 0; row < 8; row++) {

                for (int col = 0; col < 8; col++) { 
                
                    var rect = new Rectangle( col * TileSize, row * TileSize, TileSize, TileSize );

                    bool isLight = (row + col) % 2 == 0;
                    _spriteBatch.Draw(_pixel, rect, isLight ? White : Black);

                    Piece? piece = _board[row, col];
                    if (piece == null) continue;

                    string Glyph = GlyphFor(piece.Value.Type);
                    Color textColor = piece.Value.Color == PieceColor.White ? Color.White : Color.Black;

                    if (_selectedSquare is Point sel && sel.X == col && sel.Y == row)
                    {

                        _spriteBatch.Draw(_pixel, rect, Color.Yellow * .45f);

                    }

                    Vector2 size = _font.MeasureString(Glyph);

                    var pos = new Vector2(

                        rect.X + (TileSize - size.X) / 2f,
                        rect.Y + (TileSize - size.Y) / 2f

                        );

                    _spriteBatch.DrawString(_font, Glyph, pos, textColor);

                }
            
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
