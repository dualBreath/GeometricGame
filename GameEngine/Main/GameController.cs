using System.Diagnostics;
using GameEngine.Entities;
using GameEngine.Main;
using GameEngine.Storages;
using GameEngine.Utility;

namespace GameEngine
{
    internal class GameController
    {
        internal GameState State { get; set; }

        private bool pause;
        private bool stop;
        private bool keyLock;
        private Level currentLevel;

        private GameKeys playerStep = GameKeys.None;

        public GameController()
        {
            pause = false;
            stop = false;
            keyLock = false;
            playerStep = GameKeys.None;
            currentLevel = null;
            State = new GameState();
        }

        public bool Step (GameKeys key)
        {
            if (!keyLock)
            {
                playerStep = key;
                keyLock = true;
                return true;
            }
            return false;
        }
        
        private void ResetStep()
        {
            keyLock = false;
            playerStep = GameKeys.None;
        }

        internal void Play()
        {
            var player = (Player)currentLevel.Field.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == 0);
            var enemy = (Player)currentLevel.Field.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == 1);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            while (!stop)
            {
                while(!pause)
                {
                    //Thread.Sleep(1);
                    var botStep = GameKeys.None; //*/AI.Bot.Decide(currentLevel);
                    if(stopWatch.ElapsedMilliseconds > 50)
                    {
                        MovingController.MoveBullets(currentLevel);
                        stopWatch.Restart();
                    }
                    keyLock = true;
                    MovingController.Upply(currentLevel, player, playerStep);
                    ResetStep();
                    MovingController.Upply(currentLevel, enemy, botStep);

                    //if() destroyed both Stat +1 +1
                    //if() or one Stat +1
                    //if() or different Stat +1
                    //pause = true;
                    //stop = true;

                    currentLevel.Field.RemoveAll(objects => objects.IsDestroyed);                    
                    Updatemap();
                }
                stopWatch.Reset();
            }
            stopWatch.Stop();
        }

        private void Updatemap()
        {
            if (currentLevel != null && State != null)
            {
                var m = currentLevel.ConvertToString();
                
                //lock (State.Map)
                //{
                      State.Map = m;
                //}
            }
        }

        internal bool LoadGame()
        {
            bool result = ResourceManager.LoadGameState(State);
            currentLevel = new Level(State.LevelName);
            result &= currentLevel != null;
            Updatemap();
            return result;
        }

        internal void Pause()
        {
            pause = true;
        }

        internal void Stop()
        {
            pause = true;
            stop = true;
        }

        internal void Resume()
        {
            pause = false;
        }

        internal string[] GetMap()
        {
            if(State != null)
            {
                return State.Map;
            }
            return null;
        }
    }
}