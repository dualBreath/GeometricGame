using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Utility;
using System;

namespace AI.Util
{
    public static class Algorithms
    {
        public static Area CreateBlockedArea(IGameObject obj, int buffer)
        {
            if (obj.Type == ObjectType.Block)
            {
                var xPos = obj.Centre.X;
                var yPos = obj.Centre.Y;
             
                var block = obj as Block;
                var width = block.Width + 2 * buffer;
                var height = block.Height + 2 * buffer;

                return new Area(xPos, yPos, width, height);
            }
            return null;
        }

        public static Shadow CreateShadow(Position player, Block block, int buffer)
        {
            FindCorners(block, player, buffer, out var corner1, out var corner2);
            return new Shadow(corner1, corner2, player);
        }

        public static double CalcAngle(Position aPos, Position bPos, Position cPos)
        {
            var a = Mathematics.Distance(aPos, cPos);
            var b = Mathematics.Distance(aPos, bPos);
            var c = Mathematics.Distance(bPos, cPos);

            var angle = Mathematics.Angle(a, b, c);
            return angle;
        }

        public static bool IsInArea(Position pos, Area area)
        {
            var xLeft = area.Centre.X - area.Width / 2;
            var xRight = area.Centre.X + area.Width / 2;
            var yTop = area.Centre.Y - area.Height / 2;
            var yBottom = area.Centre.Y + area.Height / 2;

            if (pos.X > xLeft && pos.X < xRight &&
                    pos.Y > yTop && pos.Y < yBottom)
            {
                return true;
            }
            return false;
        }

        public static Quater FindLocation(Position point, Position supportingPoint)
        {
            if (point.X >= supportingPoint.X)
            {
                if (point.Y >= supportingPoint.Y)
                {
                    return Quater.Fourth;
                }
                else
                {
                    return Quater.First;
                }
            }
            else
            {
                if (point.Y <= supportingPoint.Y)
                {
                    return Quater.Second;
                }
                else
                {
                    return Quater.Third;
                }
            }
        }

        public static bool LessThanLine(Position pos, Position line1, Position line2)
        {
            if (line1.X - line2.X == 0)
            {
                return pos.X < line1.X;
            }
            return pos.Y <= SolveLine(line1, line2, pos.X);
        }

        public static int SolveLine(Position location1, Position location2, int x)
        {
            return (location2.Y - location1.Y) * (x - location1.X) / (location2.X - location1.X) + location1.Y;
        }

        public static bool IsCloses(Shadow shadow, Position pos)
        {
            var location1 = FindLocation(shadow.FrontPoint1, shadow.Source);
            var location2 = FindLocation(shadow.FrontPoint2, shadow.Source);


            if (location1 == Quater.First && location2 == Quater.First)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, true);
            }
            if (location1 == Quater.Second && location2 == Quater.Second)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, true);
            }
            if (location1 == Quater.Third && location2 == Quater.Third)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, false);
            }
            if (location1 == Quater.Fourth && location2 == Quater.Fourth)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, false);
            }

            if (location1 == Quater.First && location2 == Quater.Fourth ||
                location2 == Quater.First && location1 == Quater.Fourth ||
                location1 == Quater.Second && location2 == Quater.Third ||
                location2 == Quater.Second && location1 == Quater.Third)
            {
                if (shadow.FrontPoint1.Y < shadow.FrontPoint2.Y)
                {
                    return CheckHalfSpaceY(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos);
                }
                else
                {
                    return CheckHalfSpaceY(shadow.FrontPoint2, shadow.FrontPoint1, shadow.Source, pos);
                }
            }
            if (location1 == Quater.First && location2 == Quater.Third ||
                location2 == Quater.First && location1 == Quater.Third ||
                location1 == Quater.Second && location2 == Quater.Fourth ||
                location2 == Quater.Second && location1 == Quater.Fourth ||
                location1 == Quater.First && location2 == Quater.Second ||
                location2 == Quater.First && location1 == Quater.Second ||
                location1 == Quater.Third && location2 == Quater.Fourth ||
                location2 == Quater.Third && location1 == Quater.Fourth)
            {
                if (SolveLine(shadow.FrontPoint1, shadow.FrontPoint2, 0) > shadow.Source.Y)
                {
                    return CheckHalfSpaceInv(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos);
                }
                else
                {
                    return CheckHalfSpace(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos);
                }
            }

            return true;
        }

        private static void FindCorners(Block block, Position player, int buffer, out Position corner1, out Position corner2)
        {
            var leftTop = new Position(block.Centre.X - block.Width / 2 - buffer, block.Centre.Y - block.Height / 2 - buffer);
            var rightTop = new Position(block.Centre.X + block.Width / 2 + buffer, block.Centre.Y - block.Height / 2 - buffer);
            var leftBottom = new Position(block.Centre.X - block.Width / 2 - buffer, block.Centre.Y + block.Height / 2 + buffer);
            var rightBottom = new Position(block.Centre.X + block.Width / 2 + buffer, block.Centre.Y + block.Height / 2 + buffer);

            var angleLTRB = CalcAngle(player, leftTop, rightBottom);
            var angleLTLB = CalcAngle(player, leftTop, leftBottom);
            var angleLTRT = CalcAngle(player, leftTop, rightTop);
            var angleLBRT = CalcAngle(player, leftBottom, rightTop);
            var angleLBRB = CalcAngle(player, leftBottom, rightBottom);
            var angleRTRB = CalcAngle(player, rightTop, rightBottom);

            var max = angleLTRB;
            var first = leftTop;
            var second = rightBottom;
            if (angleLTLB > max)
            {
                max = angleLTLB;
                first = leftTop;
                second = leftBottom;
            }
            if (angleLTRT > max)
            {
                max = angleLTRT;
                first = leftTop;
                second = rightTop;
            }
            if (angleLBRT > max)
            {
                max = angleLBRT;
                first = leftBottom;
                second = rightTop;
            }
            if (angleLBRB > max)
            {
                max = angleLBRB;
                first = leftBottom;
                second = rightBottom;
            }
            if (angleRTRB > max)
            {
                max = angleRTRB;
                first = rightTop;
                second = rightBottom;
            }
            corner1 = first;
            corner2 = second;
        }

        private static bool CheckHalfSpaceInv(Position location1, Position location2, Position source, Position pos)
        {
            return !LessThanLine(pos, location1, source) &&
                   !LessThanLine(pos, location2, source) &&
                   !LessThanLine(pos, location1, location2);
        }

        private static bool CheckHalfSpace(Position location1, Position location2, Position source, Position pos)
        {
            return LessThanLine(pos, location1, source) &&
                   LessThanLine(pos, location2, source) &&
                   LessThanLine(pos, location1, location2);
        }

        private static bool CheckHalfSpaceY(Position location1, Position location2, Position source, Position pos)
        {
            var control = false;
            var reverseFront = source.X < location1.X;

            if (!LessThanLine(pos, location1, source) &&
                 LessThanLine(pos, location2, source))
            {

                if (location1.X < location2.X)
                {
                    if (reverseFront)
                    {
                        control = LessThanLine(pos, location1, location2);
                    }
                    else
                    {
                        control = !LessThanLine(pos, location1, location2);
                    }
                }
                else
                {
                    if (reverseFront)
                    {
                        control = !LessThanLine(pos, location1, location2);
                    }
                    else
                    {
                        control = LessThanLine(pos, location1, location2);
                    }
                }
            }
            return control;
        }

        private static bool CheckCorners(Position location1, Position location2, Position source, Position pos, bool isLeftHalf, bool isUpperHalf)
        {
            var k1 = 0.0;
            var k2 = 0.0;
            bool control = false;

            if (location1.X - source.X == 0)
            {
                k1 = int.MaxValue;
            }
            else if (location2.X - source.X == 0)
            {
                k2 = int.MaxValue;
            }
            else
            {
                k1 = Math.Abs((double)(location1.Y - source.Y) / (location1.X - source.X));
                k2 = Math.Abs((double)(location2.Y - source.Y) / (location2.X - source.X));
            }

            if (k1 > k2)
            {
                var tmpLoc = location1;
                location1 = location2;
                location2 = tmpLoc;
            }

            var condition = location1.X < location2.X;
            if (isLeftHalf)
            {
                condition = !condition;
            }

            if (isUpperHalf)
            {
                if ( LessThanLine(pos, source, location1) &&
                    !LessThanLine(pos, source, location2))
                {
                    if (condition)
                    {
                        control = !LessThanLine(pos, location1, location2);
                    }
                    else
                    {
                        control = LessThanLine(pos, location1, location2);
                    }
                }
            }
            else
            {
                if (!LessThanLine(pos, source, location1) &&
                     LessThanLine(pos, source, location2))
                {
                    if (condition)
                    {
                        control = LessThanLine(pos, location1, location2);
                    }
                    else
                    {
                        control = !LessThanLine(pos, location1, location2);
                    }
                }
            }
            return control;
        }
    }
}
