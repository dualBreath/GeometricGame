using GameEngine.Interfaces;
using GameEngine.Storages;
using GameEngine.Utility;

namespace GameEngine
{
    public class Game : IGame
    {
        private GameController actor;

        public Game()
        {
            actor = new GameController();
        }

        public bool Start()
        {
            if(actor.State.LevelName == null ||
               actor.State.LevelName == "" ||
               actor.State.Statistics == null)
            {
                return false;
            }

            actor.Play();

            return true;
        }

        public bool Step(GameKeys key)
        {
            return actor.Step(key);
        }

        public void Pause()
        {
            actor.Pause();
        }

        public void Resume()
        {
            actor.Resume();
        }

        public bool SaveGame()
        {
            Pause();
            return ResourceManager.SaveGameState(actor.State);
        }

        public bool LoadGame()
        {
            return actor.LoadGame();
        }

        public void SetLevel(string levelName)
        {
            actor.State.LevelName = levelName; 
        }

        public string GetStatistics()
        {
            return actor.State.Statistics.ConvertToString();
        }

        public string[] GetMap()
        {
            return actor.GetMap();
        }

        public void Stop()
        {
            actor.Stop();
        }
    }
}
