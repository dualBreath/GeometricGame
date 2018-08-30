using GameEngine.Utility;

namespace GameEngine.Interfaces
{
    interface IGame
    {
        bool Start();
        void Stop();
        bool Step(GameKeys key);
        void Pause();
        void Resume();
        bool SaveGame();
        bool LoadGame();
        void SetLevel(string levelName);
        string GetStatistics();
        string[] GetMap();
    }
}
