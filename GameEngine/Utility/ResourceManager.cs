using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Storages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameEngine.Utility
{
    internal static class ResourceManager
    {
        private static string directory = Directory.GetCurrentDirectory();

        public static bool SaveGameState(GameState gameState)
        {
            throw new NotImplementedException();
        }

        public static bool LoadGameState(GameState gameState)
        {
            var filename = "game_state.txt";
            var path = $"{directory}\\{filename}";
            string[] state = File.ReadAllLines(path);

            return TryParseGameState(state, gameState);
        }

        public static List<IGameObject> LoadLevel(string levelName)
        {
            var filename = $"{levelName}.txt";
            var path = $"{directory}\\{filename}";
            string[] objects = File.ReadAllLines(path);
            var result = new List<IGameObject>();

            if(objects == null || objects.Length < 5)
            {
                return null;
            }

            foreach(var obj in objects)
            {
                if(!TryParse(obj, result))
                {
                    return null;
                }
            }

            return result;
        }

        private static bool TryParseGameState(string[] state, GameState gameState)
        {
            if (state == null || state.Length < 2)
            {
                return false;
            }
            //"id:1;score:2"
            gameState.LevelName = state[0];

            foreach(var line in state.Skip(1))
            {
                if(!TryParseScore(line, out var id, out var score))
                {
                    gameState.Statistics.AllScores.Clear();
                    return false;
                }
                gameState.Statistics.AllScores[id] = score;
            }
            return true;
        }

        private static bool TryParseScore(string line, out int id, out int score)
        {
            var idAndScore = line.Split(';');
            id = 0;
            score = 0;

            if (idAndScore.Length != 2) { return false; }

            var ids = idAndScore[0].Split(':');
            var scores = idAndScore[1].Split(':');

            if(ids.Length != 2 || scores.Length != 2) { return false; }

            if (int.TryParse(ids[1], out var _id) &&
               int.TryParse(scores[1], out var _score))
            {
                id = _id;
                score = _score;
                return true;
            }
            return false; 
        }

        private static bool TryParse(string obj, List<IGameObject> result)
        {
            var parameters = obj.Split(';');

            if(parameters.Length < 3) { return false; }

            if (int.TryParse(parameters[0], out var type))
            {
                var gameObj = CreateFromType((ObjectType)type, parameters);
                if(gameObj != null)
                {
                    result.Add(gameObj);
                    return true;
                }
            }
            return false;
        }

        private static IGameObject CreateFromType(ObjectType type, string[] parameters)
        {
            if (type == ObjectType.Block)
            {
                return CreateBlock(parameters);                
            }
            if (type == ObjectType.Bullet)
            {
                return CreateBullet(parameters);
            }
            //if (type == ObjectType.Enemy)
            //{
            //    return CreateEnemy(parameters);
            //}
            if (type == ObjectType.Player)
            {
                return CreatePlayer(parameters);
            }
            if (type == ObjectType.Field)
            {
                return CreateField(parameters);
            }
            return null;
        }

        private static Field CreateField(string[] parameters)
        {
            if (parameters.Length != 6) { return null; }
           
            var control = true;

            control &= int.TryParse(parameters[1], out var id);
            control &= bool.TryParse(parameters[2], out var isDestroyed);
            control &= int.TryParse(parameters[4], out var w);
            control &= int.TryParse(parameters[5], out var h);

            if (!control) { return null; }

            var field = new Field(w, h, id)
            {
                IsDestroyed = isDestroyed
            };
            return field;
        }

        private static Player CreatePlayer(string[] parameters)
        {
            if(parameters.Length != 6) { return null; }

            var position = parameters[3].Split(':');            
            var control = true;

            if (position.Length != 2) { return null; }

            control &= int.TryParse(position[0], out var xPos);
            control &= int.TryParse(position[1], out var yPos);
            control &= int.TryParse(parameters[1], out var id);
            control &= bool.TryParse(parameters[2], out var isDestroyed);
            control &= int.TryParse(parameters[4], out var r);
            control &= int.TryParse(parameters[5], out var dir);                  

            if (!control) { return null; }

            var player = new Player(xPos, yPos, r, dir, id)
            {
                IsDestroyed = isDestroyed
            };
            return player; 
        }

        private static Bullet CreateBullet(string[] parameters)
        {
            if (parameters.Length != 7) { return null; }
            
            var position = parameters[3].Split(':');
            var control = true;

            if (position.Length != 2) { return null; }

            control &= int.TryParse(position[0], out var xPos);
            control &= int.TryParse(position[1], out var yPos);
            control &= int.TryParse(parameters[1], out var id);
            control &= bool.TryParse(parameters[2], out var isDestroyed);
            control &= int.TryParse(parameters[4], out var r);
            control &= int.TryParse(parameters[5], out var dir);
            control &= int.TryParse(parameters[6], out var ownerId);

            if (!control) { return null; }

            var bullet = new Bullet(xPos, yPos, r, dir, id, ownerId)
            {
                IsDestroyed = isDestroyed
            };
            return bullet;
        }

        public static Block CreateBlock(string[] parameters)
        {
            if (parameters.Length != 6) { return null; }
            
            var position = parameters[3].Split(':');
            var control = true;

            if(position.Length != 2) { return null; }

            control &= int.TryParse(position[0], out var xPos);
            control &= int.TryParse(position[1], out var yPos);
            control &= int.TryParse(parameters[1], out var id);
            control &= bool.TryParse(parameters[2], out var isDestroyed);
            control &= int.TryParse(parameters[4], out var w);
            control &= int.TryParse(parameters[5], out var h);

            if(!control) { return null; }

            var block = new Block(w, h, xPos, yPos, id)
            {
                IsDestroyed = isDestroyed
            };
            return block;
        }
    }
}