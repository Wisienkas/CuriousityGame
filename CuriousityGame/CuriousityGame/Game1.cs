using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CuriousityGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
#region fields
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private TileMap _tileMap;
        private Texture2D _dirt;
        private Texture2D _exit;
        private Texture2D _stone;
        private MarsRover _player;
#endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this) {PreferredBackBufferHeight = 500, PreferredBackBufferWidth = 500};

            Content.RootDirectory = "Content";
        }

#region XNA Logic
        protected override void Initialize()
        {
            _tileMap = new TileMap();
            _player = new MarsRover(this, new Point(0,0),Orientation.NORTH);
            Components.Add(_player);
            _player.MoveRover(Move.Forward); //move one step forward
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _dirt = Content.Load<Texture2D>("Dirt");
            _exit = Content.Load<Texture2D>("Exit");
            _stone = Content.Load<Texture2D>("Stone");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            if(newState.IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            foreach (var v in _tileMap.GetTileList())
            {
                Texture2D text = _dirt;
                if (v.Texture == 2)
                {
                    text = _stone;
                }
                else if (v.Texture == 3)
                {
                    text = _exit;
                }

                _spriteBatch.Draw(text, v.Position, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
#endregion
    }
}
