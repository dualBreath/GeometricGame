using GameEngine.Interfaces;
using GameEngine.Utility;

namespace GameEngine.Entities
{
    class Bullet : IGameObject, IRound
    {
        public ObjectType Type => ObjectType.Bullet;
        public Position Centre { get; set; }        
        public bool IsDestroyed { get; set; }
        public int UniqueId { get; set; }
        public int MaxSize => Radius;

        public int Radius { get; }       

        public int Direction { get; set; }
        public int OwnerId { get; }

        public Bullet(int xPos, int yPos, int r, int dir, int id, int ownerId)
        {
            Radius = r;
            OwnerId = ownerId;
            Centre = new Position(xPos, yPos);
            Direction = dir;
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
                      $"{Direction.ToString()};"+
                      $"{OwnerId.ToString()}";
            return result;
        }
    }
}
