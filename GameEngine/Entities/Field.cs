using GameEngine.Interfaces;
using GameEngine.Utility;
using System;

namespace GameEngine.Entities
{
    public class Field : IGameObject, ISquare
    {
        public ObjectType Type => ObjectType.Field;
        public Position Centre { get; set; }
        public bool IsDestroyed { get; set; }
        public int MaxSize => (int)Math.Round(Math.Sqrt(Width * Width / 4 + Height * Height / 4));
        public int UniqueId { get; set; }

        public int Width { get; }
        public int Height { get; }        

        public Field(int width, int height, int id)
        {
            Width = width;
            Height = height;
            Centre = new Position(0, 0);
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
                      $"{Width.ToString()};" +
                      $"{Height.ToString()}";
            return result;
        }
    }
}
