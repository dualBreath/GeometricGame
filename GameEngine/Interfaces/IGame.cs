using GameEngine.Utility;

namespace GameEngine.Interfaces
{
    interface IGame
    {
        void DoStep(int id, GameActions key);
        void DoPassiveActions();
        bool SaveGame(string path);
        void LoadLevel(string path, string levelName);
        string[] GetStatistics();
        string[] GetMap();
        string GetInfo();
    }
}
