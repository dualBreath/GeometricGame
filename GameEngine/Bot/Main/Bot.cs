using AI.Algorithm;
using AI.Util;
using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Storages;
using GameEngine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
    public static class Bot
    {
        private static int botId = 0;
        
        public static void SetBotId(int id)
        {
            botId = id;
        }

        public static GameKeys Decide(Level level)
        {
            var player = (Player)level.Field.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == 0);
            var bot = (Player)level.Field.Find(elem => elem.Type == ObjectType.Player && elem.UniqueId == 1);
            var field = (Field)level.Field.Find(elem => elem.Type == ObjectType.Field);
            int dist = player.Radius;

            var blocked = CreateBlockedAreas(level.Field, dist);
            var destinations = CreateDestinationAreas(level.Field, player.Centre, player.Radius / 2);           

            Greed greed = new Greed(field.Width, field.Height, player.Radius, dist);
            var step = GameKeys.None;
            
            greed.SetBlocks(blocked);
            greed.SetDestinations(destinations);

            if (greed.IsInDestinations(bot.Centre))
            {
                step = Aim(bot, player);
            }
            else
            {
                step = Go(bot, player, greed);
            }

            return step;
        }

        private static GameKeys Aim(Player bot, Player player)
        {
            if (IsRightDirection(bot, player.Centre))
            {
                return GameKeys.Shoot;
            }
            else
            {
                return TurnToAim(bot, player.Centre);
            }
        }

        private static GameKeys TurnToAim(Player bot, Position aim)
        {
            var wantedAngle = CalcAngle(bot.Centre, aim);

            if(wantedAngle - bot.Direction > 0)
            {
                return GameKeys.Right;
            }
            return GameKeys.Left;
        }

        private static double CalcAngle(Position centre, Position aim)
        {
            var dx = aim.X - centre.X;
            var dy = aim.Y - centre.Y;

            var wantedAngle = 0.0;

            if (dx == 0)
            {
                wantedAngle = dy < 0 ? 270 : 90;
            }
            else
            {
                var a = Math.Atan(dy / dx) * 180 / Math.PI;
                if (a < 0)
                {
                    wantedAngle = dx > 0 ? 360 + a : 180 + a;
                }
                else
                {
                    wantedAngle = dx > 0 ? a : 180 + a;
                }
            }

            return wantedAngle;
        }

        private static bool IsRightDirection(Player bot, Position aim)
        {
            var angle = CalcAngle(bot.Centre, aim);

            return angle - bot.Direction < 1;
        }

        private static GameKeys Go(Player bot, Player player, Greed greed)
        {
            var step = LeeSearch.FindFirstStep(bot.Centre, greed);
            if(step == null)
            {
                return GameKeys.None;
            }
            else
            {
                if(IsRightDirection(bot, step))
                {
                    return GameKeys.Move;
                }
                else
                {
                    return TurnToAim(bot, step);
                }
            }
        }

        private static List<Area> CreateBlockedAreas(List<IGameObject> field, int dist)
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

        private static List<Shadow> CreateDestinationAreas(List<IGameObject> field, Position player, int dist)
        {
            var blocks = field.Where(elem => elem.Type == ObjectType.Block)
                              .OrderBy(block => Mathematics.Distance(block.Centre, player));

            var shadows = new List<Shadow>();

            foreach(var block in blocks)
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
