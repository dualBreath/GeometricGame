using System.Collections.Generic;

namespace GameEngine.Storages
{
    public class Statistics
    {
        public Dictionary<int, int> AllScores { get; }
        
        public Statistics()
        {
            AllScores = new Dictionary<int, int>();
        }

        public void AddOrUpdate(int id, int newScore)
        {
           AllScores[id] = newScore;
        }

        internal string[] ConvertToString()
        {
            var result = new string[AllScores.Count];
            var index = 0;
            foreach(var pair in AllScores)
            {
                result[index++] = $"id:{pair.Key};score:{pair.Value}";
            }
            return result;
        }
    }
}