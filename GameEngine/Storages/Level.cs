using GameEngine.Interfaces;
using GameEngine.Utility;
using System.Collections.Generic;

namespace GameEngine.Storages
{
    public class Level
    {
        public List<IGameObject> Field { get; }

        public string LevelName { get; }

        public Level(string levelName)
        {
            LevelName = levelName;
            Field = ResourceManager.LoadLevel(levelName);
        }

        public string[] ConvertToString() //need lock
        {
            var result = new string[Field.Count];
            var index = 0;

            foreach(var obj in Field)
            {
                result[index] = obj.ConvertToString();
                index++;
            }
            return result;
        }
    }
}