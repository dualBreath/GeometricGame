using AI.Algorithm;
using AI.Util;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    class MovingController
    {
        private Stack<Position> currentPath;
        private Position lastPlayerPos;
        private Greed greed;
        private int bigRotate = 5;

        public MovingController(List<IGameObject> allObjects, int id)
        {
            var player = (Player)allObjects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId != id);
            var bot = (Player)allObjects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == id);
            var field = (Field)allObjects.Find(elem => elem.Type == ObjectType.Field);
            var dist = bot.Radius;

            lastPlayerPos = new Position(player.Centre.X, player.Centre.Y);

            greed = new Greed(field.Width, field.Height, player.Radius, dist);//2 * dist);            
            greed.SetBlocks(CreateBlockedAreas(allObjects, dist));
            greed.SetShadows(CreateShadows(allObjects, player.Centre, player.Radius / 2));

            currentPath = LeeSearch.FindPath(bot.Centre, greed);
        }

        internal GameActions GetDecision(List<IGameObject> allObjects, int id)
        {
            var player = (Player)allObjects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId != id);
            var bot = (Player)allObjects.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == id);
            var field = (Field)allObjects.Find(elem => elem.Type == ObjectType.Field);
            var step = GameActions.None;

            if (!player.Centre.Equal(lastPlayerPos))
            {
                greed.SetShadows(CreateShadows(allObjects, player.Centre, player.Radius / 2));
                currentPath = LeeSearch.FindPath(bot.Centre, greed);
            }

            if (greed.IsInDestinations(bot.Centre))
            {
                step = Aim(bot, player);
            }
            else
            {
                step = Go(bot, currentPath);
            }

            return step;
        }

        private GameActions Go(Player bot, Stack<Position> path)
        {
            var step = GameActions.None;

            if (path != null)
            {
                var pos = path.Peek();
                if (bot.Centre.X == pos.X &&
                    bot.Centre.Y == pos.Y)
                {
                    path.Pop();
                }

                if (path.Count != 0)
                {
                    if (IsRightDirection(bot, path.Peek()))
                    {
                        step = GameActions.Move;
                    }
                    else
                    {
                        step = TurnToAim(bot, path.Peek());
                    }
                }
            }
            return step;
        }

        private GameActions Aim(Player bot, Player player)
        {
            if (IsRightDirection(bot, player.Centre))
            {
                return GameActions.Shoot;
            }
            else
            {
                return TurnToAim(bot, player.Centre);
            }
        }

        private GameActions TurnToAim(Player bot, Position aim)
        {
            var wantedAngle = Mathematics.CalcAngle(bot.Centre, aim);
            var isBigRotate = Math.Abs(bot.Direction - wantedAngle) > bigRotate;

            if (bot.Direction <= 180)
            {
                if (wantedAngle >= bot.Direction &&
                    wantedAngle <= bot.Direction + 180)
                {
                    return isBigRotate ? GameActions.FastRight : GameActions.Right;
                }
                else
                {
                    return isBigRotate ? GameActions.FastLeft : GameActions.Left;
                }
            }
            else
            {
                if (wantedAngle >= bot.Direction - 180 &&
                    wantedAngle <= bot.Direction)
                {
                    return isBigRotate ? GameActions.FastLeft : GameActions.Left;
                }
                else
                {
                    return isBigRotate ? GameActions.FastRight : GameActions.Right;
                }
            }
        }

        private bool IsRightDirection(Player bot, Position aim)
        {
            var angle = Mathematics.CalcAngle(bot.Centre, aim);

            return Math.Abs(angle - bot.Direction) < 1;
        }

        private List<Area> CreateBlockedAreas(List<IGameObject> field, int dist)
        {
            var blocks = new List<Area>();
            
            foreach (var obj in field)
            {
                var area = Algorithms.CreateBlockedArea(obj, dist);
                if (area != null)
                {
                    blocks.Add(area);
                }
            }

            return blocks;
        }

        private List<Shadow> CreateShadows(List<IGameObject> field, Position player, int dist)
        {
            var blocks = field.Where(elem => elem.Type == ObjectType.Block)
                              .OrderBy(block => Mathematics.Distance(block.Centre, player));

            var shadows = new List<Shadow>();

            foreach (var block in blocks)
            {
                var shadow = Algorithms.CreateShadow(player, block as Block, dist);
                if (shadow != null)
                {
                    shadows.Add(shadow);
                }
            }
            return shadows;
        }
    }
}
