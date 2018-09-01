using System;
using System.Diagnostics;
using System.Threading;
using GameEngine.Entities;
using GameEngine.Main;
using GameEngine.Storages;
using GameEngine.Utility;

namespace GameEngine
{
    internal class GameController
    {
        internal GameState State { get; set; }
        private Level currentLevel;
        private bool isLevelEnded;


        internal GameController()
        {
            currentLevel = null;
            State = new GameState();
            isLevelEnded = false;
        }

        internal bool SelectLevel(string levelName)
        {
            State = new GameState();
            currentLevel = new Level(levelName);
            return currentLevel != null;
        }

        internal bool IsLevelEnded()
        {
            return isLevelEnded;
        }

        internal bool LoadGame()
        {
            bool result = ResourceManager.LoadGameState(State);
            currentLevel = new Level(State.LevelName);
            result &= currentLevel != null;
            return result;
        }
        internal void DoPassiveActions()
        {
            MovingController.MoveBullets(currentLevel, out var killerBullet);

            if(killerBullet != null)                
            {
                isLevelEnded = true;
                State.Statistics.AllScores[killerBullet.OwnerId] += 1;
            }

            currentLevel.Field.RemoveAll(objects => objects.IsDestroyed);
        }

        internal void DoStep(int id, GameActions action)
        {
            var player = (Player)currentLevel.Field.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == id);
            if(player != null)
            {
                MovingController.Upply(currentLevel, player, action, out var killerBullet);

                if (player.IsDestroyed)
                {
                    isLevelEnded = true;
                    State.Statistics.AllScores[killerBullet.OwnerId] += 1;
                }
            }
            currentLevel.Field.RemoveAll(objects => objects.IsDestroyed);
        }
        
        internal string[] GetMap()
        {
            return currentLevel.ConvertToString();
        }
    }
}