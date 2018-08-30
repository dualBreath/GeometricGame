using GameEngine.Interfaces;
using GameEngine.Storages;
using System.Collections.Generic;

namespace GameEngine.Utility
{
    static class IdManager
    {
        private static int nextId = 0;

        public static void SetStartId(int id)
        {
            nextId = id;
        }

        public static int GetNextId()
        {
            return nextId++;
        }

        public static bool IsCorrectIds(Level level)
        {
            var checkingSet = new HashSet<int>();
            var control = true;

            foreach (IGameObject obj in level.Field)
            {
                if(checkingSet.Contains(obj.UniqueId))
                {
                    control = false;
                    break;
                }
                else
                {
                    checkingSet.Add(obj.UniqueId);
                }
            }
            return control;
        }

        public static void ResetAndReplaceId(Level level)
        {
            nextId = 0;

            foreach (IGameObject obj in level.Field)
            {
                obj.UniqueId = nextId;
                nextId += 1;
            }
        }
    }
}
