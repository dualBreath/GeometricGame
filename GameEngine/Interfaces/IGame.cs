using GameEngine.Utility;

namespace GameEngine.Interfaces
{
    interface IGame
    {
        void DoStep(int id, GameActions key);
        void DoPassiveActions();
        bool SaveGame();
        void LoadLevel(string levelName);
        string GetStatistics();
        string[] GetMap();
        string GetInfo();
    }
}
