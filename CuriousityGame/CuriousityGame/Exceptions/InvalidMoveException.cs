using System;

namespace CuriousityGame
{
    [Serializable]
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(Move m) : base(String.Format(""))
        {
            
        }
    }
}
