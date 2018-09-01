namespace GameEngine.Storages
{
    public class GameInfo
    {
        public string Vesion { get; }
        public string Name { get; }

        public GameInfo(string name, string version)
        {
            Vesion = version;
            Name = name;
        }

        public string ConvertToString()
        {        
            return $"Name:{Name};version:{Vesion}";
        }
    }
}