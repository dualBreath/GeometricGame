using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Storages;
using GameEngine.Utility;
using System;
using System.Collections.Generic;

namespace GameEngine.Main
{
    internal static class MovingController
    {
        private static int playerSpeed = 2;
        private static int bulletSpeed = 2;
        private static int bulletRadius = 2;

        internal static void MoveBullets(Level level)
        {
            var field = (Field)level.Field.Find(elem => elem.Type == ObjectType.Field);

            foreach (Bullet bullet in level.Field.FindAll(obj => obj.Type == ObjectType.Bullet))
            {
                var newX = bullet.Centre.X + (int)Math.Round(bulletSpeed * Math.Cos(bullet.Direction * Math.PI / 180));
                var newY = bullet.Centre.Y + (int)Math.Round(bulletSpeed * Math.Sin(bullet.Direction * Math.PI / 180));
                var newCentre = new Position(newX, newY);                

                if (Mathematics.IsInField(field.Width, field.Height, bullet.Radius, newCentre))
                {
                    MoveBullet(level.Field, bullet, newCentre);
                }
                else
                {
                    bullet.IsDestroyed = true;
                }
            }
        }
        
        private static void MoveBullet(List<IGameObject> field, Bullet bullet, Position newCentre)
        {
            var isCollision = false;
            var collidingObject = CheckCollision(field, bullet, newCentre, out isCollision);
            if (isCollision)
            {
                if (collidingObject.Type == ObjectType.Block)
                {
                    Reflect(collidingObject as Block, bullet);
                }
                else
                {
                    bullet.Centre.X = newCentre.X;
                    bullet.Centre.Y = newCentre.Y;

                    if (collidingObject.Type == ObjectType.Player &&
                    collidingObject.UniqueId != bullet.OwnerId)
                    {
                        (collidingObject as Player).IsDestroyed = true;
                    }
                }
            }
            else
            {
                bullet.Centre.X = newCentre.X;
                bullet.Centre.Y = newCentre.Y;
            }
        }

        private static void Reflect(Block block, Bullet bullet)
        {
            var location = FindRelativeLocation(block, bullet.Centre);
            var newX = 0;
            var newY = 0;
            var newDirection = 0;
            var tg = Math.Tan(bullet.Direction * Math.PI / 180);            
            
            if (location == Location.Top)
            {
                newY = block.Centre.Y - block.Height / 2 - bullet.Radius;
                var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                newX = bullet.Centre.X + dX;
                newDirection = 360 - bullet.Direction;
            }
            if (location == Location.Bottom)
            {
                newY = block.Centre.Y + block.Height / 2 + bullet.Radius;
                var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                newX = bullet.Centre.X + dX;
                newDirection = 360 - bullet.Direction;
            }
            if (location == Location.MiddleRight)
            {
                newX = block.Centre.X + block.Width / 2 + bullet.Radius;
                var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                newY = bullet.Centre.Y + dY;
                if(bullet.Direction > 180)
                {
                    newDirection = 360 - bullet.Direction + 180;
                }
                else
                {
                    newDirection = 180 - bullet.Direction;
                }
            }
            if (location == Location.MiddleLeft)
            {
                newX = block.Centre.X - block.Width / 2 - bullet.Radius;
                var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                newY = bullet.Centre.Y + dY;
                if (bullet.Direction < 180)
                {
                    newDirection = 180 - bullet.Direction;
                }
                else
                {
                    newDirection = 360 - bullet.Direction + 180;
                }
            }
            if (location == Location.TopLeft)
            {
                if (bullet.Direction > 315)
                {
                    newY = block.Centre.Y - block.Height / 2 - bullet.Radius;
                    var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                    newX = bullet.Centre.X + dX;
                    newDirection = 360 - bullet.Direction;
                }
                else
                {
                    newX = block.Centre.X - block.Width / 2 - bullet.Radius;
                    var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                    newY = bullet.Centre.Y + dY;
                    newDirection = 360 - bullet.Direction + 180;
                }
            }
            if (location == Location.TopRight)
            {
                if (bullet.Direction > 225)
                {
                    newY = block.Centre.Y - block.Height / 2 - bullet.Radius;
                    var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                    newX = bullet.Centre.X + dX;
                    newDirection = 360 - bullet.Direction + 180;
                }
                else
                {
                    newX = block.Centre.X + block.Width / 2 + bullet.Radius;
                    var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                    newY = bullet.Centre.Y + dY;
                    newDirection = 360 - bullet.Direction;
                }
            }
            if (location == Location.BottomLeft)
            {
                if (bullet.Direction > 45)
                {
                    newX = block.Centre.X - block.Width / 2 - bullet.Radius;
                    var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                    newY = bullet.Centre.Y + dY;
                    newDirection = 180 - bullet.Direction;
                }
                else
                {
                    newY = block.Centre.Y + block.Height / 2 + bullet.Radius;
                    var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                    newX = bullet.Centre.X + dX;
                    newDirection = 360 - bullet.Direction;
                }
            }
            if (location == Location.BottomRight)
            {
                if (bullet.Direction > 135)
                {
                    newY = block.Centre.Y + block.Height / 2 + bullet.Radius;
                    var dX = (int)Math.Round((bullet.Centre.Y - newY) / tg);
                    newX = bullet.Centre.X + dX;
                    newDirection = 360 - bullet.Direction;
                }
                else
                {
                    newX = block.Centre.X + block.Width / 2 + bullet.Radius;
                    var dY = (int)Math.Round((bullet.Centre.X - newX) * tg);
                    newY = bullet.Centre.Y + dY;
                    newDirection = 180 - bullet.Direction;
                }
            }

            bullet.Direction = newDirection;
            bullet.Centre.X = newX;
            bullet.Centre.Y = newY;
        }

        internal static void Upply(Level level, Player player, GameKeys playerStep)
        {
            if (playerStep == GameKeys.Left || playerStep == GameKeys.Right)
            {
                Rotate(player, playerStep);
            }
            else if (playerStep == GameKeys.Move)
            {
                Move(level, player, playerSpeed);
            }
            else if (playerStep == GameKeys.Shoot)
            {
                SpawnBullet(level, player);
            }
        }

        internal static void SpawnBullet(Level level, Player player)
        {
            var xShift = (int)Math.Round(player.Radius * Math.Cos(player.Direction * Math.PI / 180));
            var yShift = (int)Math.Round(player.Radius * Math.Sin(player.Direction * Math.PI / 180));

            var xPos = xShift + player.Centre.X;
            var yPos = yShift + player.Centre.Y;

            var id = IdManager.GetNextId();

            level.Field.Add(new Bullet(xPos, yPos, bulletRadius, player.Direction, id, player.UniqueId));
        }


        internal static void Move(Level level, Player player, int speed)
         {
            var newX = player.Centre.X + (int)Math.Round(speed * Math.Cos(player.Direction * Math.PI / 180));
            var newY = player.Centre.Y + (int)Math.Round(speed * Math.Sin(player.Direction * Math.PI / 180));
            
            var newCentre = new Position(newX, newY);

            var field = (Field)level.Field.Find(elem => elem.Type == ObjectType.Field);
            var isCollision = false;

            if (Mathematics.IsInField(field.Width, field.Height, player.MaxSize, newCentre))
            {
                var collidingObject = CheckCollision(level.Field, player, newCentre, out isCollision);
                if (!isCollision)
                {
                    player.Centre.X = newCentre.X;
                    player.Centre.Y = newCentre.Y;                    
                }
                else if(collidingObject.Type == ObjectType.Bullet && 
                       (collidingObject as Bullet).OwnerId != player.UniqueId)
                {
                    player.IsDestroyed = true;
                    player.Centre.X = newCentre.X;
                    player.Centre.Y = newCentre.Y;
                }
            }
        }

        private static IGameObject CheckCollision(List<IGameObject> field, IRound obj, Position newCentre, out bool isCollision)
        {
            foreach (var gameObj in field)
            {
                if (gameObj.UniqueId != (obj as IGameObject).UniqueId)
                {
                    if (gameObj.Type == ObjectType.Block ||
                    gameObj.Type == ObjectType.Bullet ||
                    gameObj.Type == ObjectType.Player)
                    {
                        var diff = Mathematics.Distance(newCentre, gameObj.Centre) - gameObj.MaxSize - obj.Radius;
                        if (diff < 0)
                        {
                            if (gameObj.Type == ObjectType.Block)
                            {
                                if (PreciseСalculationCollision(gameObj as Block, obj, newCentre))
                                {
                                    isCollision = true;
                                    return gameObj;
                                }
                                isCollision = false;
                                return null;
                            }
                            isCollision = true;
                            return gameObj;
                        }
                    }
                }
            }
            isCollision = false;
            return null;
        }

        private static bool PreciseСalculationCollision(Block block, IRound obj, Position newCentre)
        {
            var location = FindRelativeLocation(block, newCentre);

            if (location == Location.Top || location == Location.Bottom)
            {
                var diff = Math.Abs(block.Centre.Y - newCentre.Y) - obj.Radius - block.Height / 2;
                return diff < 0;
            }
            else if (location == Location.MiddleLeft || location == Location.MiddleRight)
            {
                var diff = Math.Abs(block.Centre.X - newCentre.X) - obj.Radius - block.Width / 2;
                return diff < 0;
            }
            else if(location == Location.TopLeft)
            {
                var cornerPos = new Position(block.Centre.X - block.Width / 2, block.Centre.Y - block.Height / 2);
                var diff = Mathematics.Distance(cornerPos, newCentre) - obj.Radius;
                return diff < 0;
            }
            else if (location == Location.TopRight)
            {
                var cornerPos = new Position(block.Centre.X + block.Width / 2, block.Centre.Y - block.Height / 2);
                var diff = Mathematics.Distance(cornerPos, newCentre) - obj.Radius;
                return diff < 0;
            }
            else if (location == Location.BottomRight)
            {
                var cornerPos = new Position(block.Centre.X + block.Width / 2, block.Centre.Y + block.Height / 2);
                var diff = Mathematics.Distance(cornerPos, newCentre) - obj.Radius;
                return diff < 0;
            }
            else if (location == Location.BottomLeft)
            {
                var cornerPos = new Position(block.Centre.X - block.Width / 2, block.Centre.Y + block.Height / 2);
                var diff = Mathematics.Distance(cornerPos, newCentre) - obj.Radius;
                return diff < 0;
            }
            return false;
        }

        private static Location FindRelativeLocation(Block block, Position newCentre)
        {
            if(newCentre.X <= block.Centre.X + block.Width / 2 && 
               newCentre.X >= block.Centre.X - block.Width / 2)
            {
                return newCentre.Y >= block.Centre.Y ? Location.Bottom : Location.Top;
            }
            else if(newCentre.Y <= block.Centre.Y + block.Height / 2 &&
                    newCentre.Y >= block.Centre.Y - block.Height / 2)
            {
                return newCentre.X >= block.Centre.X ? Location.MiddleRight : Location.MiddleLeft;               
            }
            else if(newCentre.X >= block.Centre.X)
            {
                return newCentre.Y >= block.Centre.Y ? Location.BottomRight : Location.TopRight;
            }
            else
            {
                return newCentre.Y >= block.Centre.Y ? Location.BottomLeft : Location.TopLeft;
            }
        }

        internal static void Rotate(Player player, GameKeys playerStep)
        {
            if (playerStep == GameKeys.Left)
            {
                var angle = player.Direction - 1;
                player.Direction = angle;
            }
            else if (playerStep == GameKeys.Right)
            {
                var angle = player.Direction + 1;
                player.Direction = angle;
            }
        }
    }
}
