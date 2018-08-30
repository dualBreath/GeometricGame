using GameEngine.Utility;

namespace GameEngine.Interfaces
{
    public interface IGameObject
    {
        Position Centre { get; set; }

        ObjectType Type { get; }

        bool IsDestroyed { get; }

        int MaxSize { get; }

        int UniqueId { get; set; }

        string ConvertToString();
    }
}