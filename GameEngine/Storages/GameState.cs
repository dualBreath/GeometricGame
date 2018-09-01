namespace GameEngine.Storages
{
    public class GameState
    {
        internal string LevelName { get; set; }
        internal Statistics Statistics { get; set; }

        public GameState()
        {
            Statistics = new Statistics();
        }
    }
}