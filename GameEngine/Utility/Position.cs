namespace GameEngine.Utility
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equal(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            //long num = X << 32 + Y;
            //return num.GetHashCode();
            return 25000 * X + Y;
        }

        public override bool Equals(object obj)
        {
            Position pos = obj as Position;

            if (pos == null)
                return false;

            return X == pos.X && Y == pos.Y;
        }
    }
}