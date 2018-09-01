using GameEngine.Utility;

namespace AI.Util
{
    public class Shadow
    {
        public Position FrontPoint1 { get; }
        public Position FrontPoint2 { get; }
        public Position Source { get; }
       
        public Shadow(Position front1, Position front2, Position source)
        {
            FrontPoint1 = new Position(front1.X, front1.Y);
            FrontPoint2 = new Position(front2.X, front2.Y);
            
            Source = new Position(source.X, source.Y);
        }

    }
}
