using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Utility;
using System.Collections.Generic;

namespace AI
{
    public class Bot
    {
        private int botId = 0;
        private MovingController actor;

        public Bot(int id)
        {
            botId = id;
        }

        public bool CreateMovingController(string[] map)
        {
            var allObjects = ResourceManager.CreateObjects(map);
            if (IsCorrectField(allObjects, botId))
            {
                actor = new MovingController(allObjects, botId);
            }
            return false;
        }

        public GameActions Decide(string[] map)
        {
            var allObjects = ResourceManager.CreateObjects(map);
            var step = GameActions.None;

            if (IsCorrectField(allObjects, botId))
            {
                step = actor.GetDecision(allObjects, botId);
            }

            return step;
        }

        private bool IsCorrectField(List<IGameObject> objects, int id)
        {
            var player = (Player)objects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId != id);
            var bot = (Player)objects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == id);
            var field = (Field)objects.Find(elem => elem.Type == ObjectType.Field);

            return player != null && bot != null && field != null;
        }
    }
}
