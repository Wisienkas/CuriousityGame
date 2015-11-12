using Microsoft.Xna.Framework;

namespace CuriousityGame
{
    public class Tile
    {
        public Vector2 Position;
        public static int Height = 50;
        public static int Width = 50;
        public int Texture;
        
        public Tile(int tileType,int x, int y)
        {
            Position = new Vector2(x, y);
            Texture = tileType;
        }
    }
}
