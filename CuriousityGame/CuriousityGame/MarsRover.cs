using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CuriousityGame.CuriousityGame;
using CuriousityGame.Exceptions;

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
            _controller = new RoverController();
            _controller.Rover = this;
            _controller.InitializeTerrain();
            _position.X = p.X + (int)_origin.X;
           _position.Y = FlipYCoordinate(p.Y+(int)_origin.Y);
            if(!Controller.Tilemap.CheckMove(Controller.GetRoverPosition()))
            {
                throw new InitializationException(p);
            }
            _chasePoint = new Point(Position.X, Position.Y);
            _orientation = orientation;
            _degrees = (int)orientation;
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
                try
                {
                    _controller.nextMove();
                } catch (InvalidMoveException e)
                {
                    // Here futher exception handling can be done.
                }
            }
        }

        private void wrapMap()
        {
            // Handle X
            if(_position.X <= 0)
            {
                _position.X = (int) (MapSize(0) + (_origin.X - 1));
                _chasePoint.X = (int) (MapSize(0) - _origin.X);
            }
            else if(_position.X > MapSize(0))
            {
                _position.X = (int) (-1 * (_origin.X - 1));
                _chasePoint.X = (int) _origin.X;
            }
            // Handle Y
            if (_position.Y <= 0)
            {
                _position.Y = (int)(MapSize(0) + (_origin.Y - 1));
                _chasePoint.Y = (int)(MapSize(0) - _origin.Y);
            }
            else if (_position.Y > MapSize(0))
            {
                _position.Y = (int)(-1 * (_origin.Y - 1));
                _chasePoint.Y = (int)_origin.Y;
            }
        }

        private bool insideMap()
        {
            return insideMap(Position.X, 0) && insideMap(Position.Y, 1);
        }

        private bool insideMap(int p, int dimension)
        {
            var originSize = dimension == 0 ? _origin.X : _origin.Y;
            return p < MapSize(dimension) + originSize && p > -1 * originSize;
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
            return MapSize(1) - i;
        }

        public int MapSize(int dimension)
        {
            if (dimension == 0) return (int) Controller.Tilemap.GetMapDimensions().X * Tile.Width;
            if (dimension == 1) return (int) Controller.Tilemap.GetMapDimensions().Y * Tile.Height;
            return 0;
        }

        public Vector2 RoverSize()
        {
            return new Vector2(_origin.X * 2, _origin.Y * 2);
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
