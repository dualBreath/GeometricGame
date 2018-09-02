using GameEngine.Interfaces;
using GameEngine.Storages;
using GameEngine.Utility;

namespace GameEngine
{
    public class Game : IGame
    {
        private GameController actor;
        private GameInfo info;

        public Game()
        {
            info = new GameInfo("Geometric", "1.0.1");
            actor = new GameController();
        }
       
        public void DoStep(int id, GameActions action)
        {
            actor.DoStep(id, action);
        }

        public void DoPassiveActions()
        {
            actor.DoPassiveActions();
        }

        public bool IsLevelEnded()
        {
            return actor.IsLevelEnded();
        }

        public bool SaveGame(string path)
        {
            return ResourceManager.SaveGameState(path, actor.State);
        }
        
        public void LoadLevel(string path, string levelName)
        {
            actor.SelectLevel(path, levelName);
        }

        public void LoadGame(string statePath, string levelPath)
        {
            actor.LoadGame(statePath, levelPath);
        }

        public string[] GetStatistics()
        {
            return actor.State.Statistics.ConvertToString();
        }

        public string GetInfo()
        {
            return info.ConvertToString();
        }

        public string[] GetMap()
        {
            return actor.GetMap();
        }
    }
}
