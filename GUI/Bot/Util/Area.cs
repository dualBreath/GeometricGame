using GameEngine.Utility;

namespace AI.Util
{
    public class Area
    {
        public Position Centre { get; }
        public int Width { get; }
        public int Height { get; }

        public Area(int xPos, int yPos, int width, int height)
        {
            Centre = new Position(xPos, yPos);
            Width = width;
            Height = height;
        }
    }
}