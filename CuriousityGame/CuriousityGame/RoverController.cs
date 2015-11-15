using System;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace CuriousityGame
{
    namespace CuriousityGame
    {
        public class RoverController
        {
            /*
             * Game created in the XNA-Framework in correlation with the course "Design of Software Systems"-2015
             * Game1 works as a collective place for functionallity for the application
             * Program.cs shows how it's initialized
             * (Initial names of a new XNA game)
             * 
             * Missing functionallity, refactoring and testing is the job for today. 
             * Go to View -> Tast List (and set it to comments if not already there) to see points of interests.
             * 
             * Good luck!
             */
             
            public MarsRover Rover { get; set; }
            public TileMap Tilemap { get; set; }
            private Queue<char> commandQueue = new Queue<char>();

            public void InitializeTerrain()
            {
                this.Tilemap = new TileMap();
            }

            public Orientation GetRoverOrientiation()
            {
                return Rover.Orientation;
            }

            public Point GetRoverPosition()
            {
                int lol = (int) Rover.RoverSize().X;
                return new Point()
                {
                    X = (Rover.Position.X - (int)Rover.RoverSize().X / 2) / (int) Rover.RoverSize().X,
                    Y = (Math.Abs((Rover.Position.Y - Rover.MapSize(1)) / (int) Rover.RoverSize().Y))
                };
            }

            public void MoveRover(string commandSequence)
            {
                foreach (char command in commandSequence.ToLower())
                {
                    commandQueue.Enqueue(command);
                }
                if (Rover.Idle) nextMove();
            }

            public void nextMove()
            {
                if(commandQueue.Count != 0) HandleCommand(commandQueue.Dequeue());
            }

            /// <summary>
            /// Handles a command
            /// </summary>
            /// <param name="command"></param>
            private void HandleCommand(char command)
            {
                switch (command)
                {
                    case 'f': TryMove(Move.Forward); break;
                    case 'b': TryMove(Move.Backward); break;
                    case 'r': Rover.TurnRight(); break;
                    case 'l': Rover.TurnLeft(); break;
                }
            }

            private void TryMove(Move move)
            {
                Point cur = GetRoverPosition();
                Point orientationVector = getForce(GetRoverOrientiation());
                int directionGate = move == Move.Forward ? 1 : -1;
                Point nextMove = new Point(
                    cur.X + orientationVector.X * directionGate, 
                    cur.Y + orientationVector.Y * directionGate);
                if (Tilemap.CheckMove(nextMove))
                {
                    Rover.MoveRover(move);
                }
                else
                {
                    commandQueue.Clear();
                    throw new InvalidMoveException(move);
                }
            }

            private Point getForce(Orientation orientation)
            {
                int x = 0;
                int y = 0;
                switch(orientation)
                {
                    case Orientation.EAST: x++; break;
                    case Orientation.WEST: x--; break;
                    case Orientation.NORTH: y++; break;
                    case Orientation.SOUTH: y--; break;
                }
                return new Point(x, y);
            }
            
        }
    }
}
