namespace GameEngine.Storages
{
    internal class GameState
    {
        internal string LevelName { get; set; }
        internal Statistics Statistics { get; set; }
        internal string[] Map { get; set; }

        public GameState()
        {
            Statistics = new Statistics();
        }
    }
}