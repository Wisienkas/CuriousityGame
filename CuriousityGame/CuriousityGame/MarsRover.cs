using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CuriousityGame.CuriousityGame;

namespace CuriousityGame
{

    public enum Orientation
    {
        NORTH   = 0,
        EAST    = 90,
        SOUTH   = 180,
        WEST    = 270
    }


    public class MarsRover : DrawableGameComponent
    {
#region fields
        private Texture2D _curiousity;
        private Point _chasePoint;
        private SpriteBatch _spriteBatch;
        private Orientation _orientation;
        private readonly Vector2 _origin = new Vector2(25,25); //img width/2, img height/2
        private float _degrees;
        private Point _position;
        private RoverController _controller;
#endregion

#region Properties
        public Point Position
        {
            get {return _position ; }
        }

        public Orientation Orientation {
            get { return _orientation; }
        }

        public RoverController Controller { get { return _controller; } }

        public bool Idle { get; internal set; } = true;
#endregion

        public MarsRover(Game game, Point p, Orientation orientation) : base(game)
        {
            // TODO: check if initialized at weird point, and throw exception
            _position.X = p.X + (int)_origin.X;
           _position.Y = FlipYCoordinate(p.Y+(int)_origin.Y);
            _chasePoint = new Point(Position.X, Position.Y);
            _orientation = orientation;
            _degrees = (int)orientation;
            _controller = new RoverController();
            _controller.Rover = this;
            _controller.InitializeTerrain();
        }

#region XNA logic
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _curiousity = Game.Content.Load<Texture2D>("Curiousity2");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            switch ((int)_degrees)
            {
                case 0:
                    _orientation = Orientation.NORTH;
                    break;
                case 90:
                    _orientation = Orientation.EAST;
                    break;
                case 180:
                    _orientation = Orientation.SOUTH;
                    break;
                case 270:
                    _orientation = Orientation.WEST;
                    break;
            }
            if (_position.X < _chasePoint.X)
            {
                _position.X++;
            }
            else if (_position.X > _chasePoint.X)
            {
                _position.X--;
            }
            if (_position.Y < _chasePoint.Y)
            {
                _position.Y++;
            }
            else if (_position.Y > _chasePoint.Y)
            {
                _position.Y--;
            }

            if (!insideMap()) wrapMap();

            if (_position == _chasePoint)
            {
                Idle = true;
                _controller.nextMove();
            }
        }

        private void wrapMap()
        {
            // Handle X
            if(_position.X <= 0)
            {
                _position.X = 524;
                _chasePoint.X = 475;
            }
            else if(_position.X > 500)
            {
                _position.X = -24;
                _chasePoint.X = 25;
            }
            // Handle Y
            if (_position.Y <= 0)
            {
                _position.Y = 524;
                _chasePoint.Y = 475;
            }
            else if (_position.Y > 500)
            {
                _position.Y = -24;
                _chasePoint.Y = 25;
            }
        }

        private bool insideMap()
        {
            return insideMap(Position.X) && insideMap(Position.Y);
        }

        private bool insideMap(int p)
        {
            return p < 525 && p > -25;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if(_spriteBatch != null)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_curiousity, new Vector2(Position.X, Position.Y), null, Color.White, MathHelper.ToRadians(_degrees), _origin, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.End();
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }
#endregion

        public void TurnLeft()
        {
            _degrees -= 90;
            if (_degrees < -1)
            {
                _degrees = 270;
            }
            Idle = false;
        }

        public void TurnRight()
        {
            _degrees += 90;
            if (_degrees > 271)
            {
                _degrees = 0;
            }
            Idle = false;
        }

        private int FlipYCoordinate(int i)
        {
            return (int)TileMap.MapDimensions.Y * Tile.Height-i;
        }

        public void MoveRover(Move m)
        {
            Idle = false;
            switch (m)
            {
                case Move.Forward:
                    switch (_orientation)
                    {
                        case Orientation.NORTH:
                            _chasePoint = new Point(_position.X,_position.Y-Tile.Height);
                            break;
                        case Orientation.EAST:
                            _chasePoint = new Point(_position.X + Tile.Width, _position.Y);
                            break;
                        case Orientation.SOUTH:
                            _chasePoint = new Point(_position.X,_position.Y+Tile.Height);
                            break;
                        case Orientation.WEST:
                            _chasePoint = new Point(_position.X-Tile.Width,_position.Y);
                            break;
                    }
                    break;
                case Move.Backward:
                    switch (_orientation)
                    {
                        case Orientation.NORTH:
                            _chasePoint = new Point(_position.X, _position.Y + Tile.Height);
                            break;
                        case Orientation.EAST:
                            _chasePoint = new Point(_position.X - Tile.Width, _position.Y);
                            break;
                        case Orientation.SOUTH:
                            _chasePoint = new Point(_position.X, _position.Y - Tile.Height);
                            break;
                        case Orientation.WEST:
                            _chasePoint = new Point(_position.X + Tile.Width, _position.Y);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
