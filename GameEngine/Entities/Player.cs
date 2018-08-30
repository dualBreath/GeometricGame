using GameEngine.Interfaces;
using GameEngine.Utility;

namespace GameEngine.Entities
{
    class Player : IGameObject, IRound
    {
        private int direction;
        public ObjectType Type => ObjectType.Player;
        public Position Centre { get; set; }        
        public bool IsDestroyed { get; set; }
        public int MaxSize => Radius;
        public int UniqueId { get; set; }

        public int Radius { get; }        

        public int Direction
        {
            get { return direction; }
            set
            {
                var angle = value % 360;
                direction = angle < 0 ? 360 + angle : angle;
            }
        }

        public Player(int xPos, int yPos, int r, int dir, int id)
        {
            Radius = r;
            Direction = dir;
            Centre = new Position(xPos, yPos);
            IsDestroyed = false;
            UniqueId = id;
        }

        public string ConvertToString()
        {
            var result = "";
            result += $"{((int)Type).ToString()};" +
                      $"{UniqueId.ToString()};" +
                      $"{IsDestroyed.ToString()};" +
                      $"{Centre.X.ToString()}:" +
                      $"{Centre.Y.ToString()};" +
                      $"{Radius.ToString()};" +
                      $"{Direction.ToString()}";
            return result;
        }
    }
}
