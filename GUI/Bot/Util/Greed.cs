using GameEngine.Utility;
using System;
using System.Collections.Generic;

namespace AI.Util
{
    internal class Greed
    {
        private int distance = 0;
        private int width = 0;
        private int height = 0;
        private int size = 0;

        private List<Area> blocks;
        private List<Shadow> shadows;

        public Greed(int width, int height, int size, int distance)
        {
            this.width = width;
            this.height = height;
            this.distance = distance;
            this.size = size;
        }

        public int Total()
        {
            return (int)((double)width / size * (double)height / size);
        }

        public void SetBlocks(List<Area> blocks)
        {
            this.blocks = blocks;
        }

        public void SetShadows(List<Shadow> shadows)
        {
            this.shadows = shadows;
        }

        public List<Area> GetNearests(Position from)
        {
            var nearests = new List<Area>();
            var pos = new Position(from.X, from.Y)
            {
                Y = from.Y - distance
            };

            if (IsInGreed(pos) && !IsInBlocks(pos))
            {
                nearests.Add(new Area(pos.X, pos.Y, distance, distance));
            }
            pos.Y = from.Y + distance;
            if (IsInGreed(pos) && !IsInBlocks(pos))
            {
                nearests.Add(new Area(pos.X, pos.Y, distance, distance));
            }
            pos.X = from.X - distance;
            pos.Y = from.Y;
            if (IsInGreed(pos) && !IsInBlocks(pos))
            {
                nearests.Add(new Area(pos.X, pos.Y, distance, distance));
            }
            pos.X = from.X + distance;
            if (IsInGreed(pos) && !IsInBlocks(pos))
            {
                nearests.Add(new Area(pos.X, pos.Y, distance, distance));
            }
            return nearests;
        }

        public bool IsInBlocks(Position pos)
        {
            return IsInAreas(pos, blocks);
        }

        public bool IsInGreed(Position pos)
        {
            return pos.X > size && pos.X < width - size &&
                   pos.Y > size && pos.Y < height - size;
        }

        public bool IsInDestinations(Position pos)
        {
            foreach(var shadow in shadows)
            {
                if(IsCloses(shadow, pos))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsCloses(Shadow shadow, Position pos)
        {
            var location1 = FindLocation(shadow.FrontPoint1, shadow.Source);
            var location2 = FindLocation(shadow.FrontPoint2, shadow.Source);


            if(location1 == Quater.First && location2 == Quater.First)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, true);
                //return CheckTopCorner(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false);
            }
            if (location1 == Quater.Second && location2 == Quater.Second)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, true);
                //return CheckTopCorner(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true);
            }
            if (location1 == Quater.Third && location2 == Quater.Third)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, false);
                //return CheckBottomCorner(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false);
            }
            if (location1 == Quater.Fourth && location2 == Quater.Fourth)
            {
                return CheckCorners(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, false);
                //return CheckBottomCorner(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true);
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

        private bool CheckHalfSpaceInv(Position location1, Position location2, Position source, Position pos)
        {
            return !LessThanLine(pos, location1, source) &&
                   !LessThanLine(pos, location2, source) &&
                   !LessThanLine(pos, location1, location2);
        }

        private bool CheckHalfSpace(Position location1, Position location2, Position source, Position pos)
        {
            return LessThanLine(pos, location1, source) &&
                   LessThanLine(pos, location2, source) &&
                   LessThanLine(pos, location1, location2);
        }

        private bool CheckHalfSpaceY(Position location1, Position location2, Position source, Position pos)
        {
            var control = false;
            var reverseFront = source.X < location1.X;

            if (!LessThanLine(pos, location1, source) &&
                 LessThanLine(pos, location2, source))
            {

                if(location1.X < location2.X)
                {
                    if(reverseFront)
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

        //private bool CheckBottomCorner(Position location1, Position location2, Position source, Position pos, bool reverse)
        //{
        //    bool control = false;
        //    var loc = location1.X < location2.X;
        //    if (reverse)
        //    {
        //        loc = !loc;
        //    }

        //    if (loc)
        //    {
        //        if (!LessThanLine(pos, location1, source) &&
        //             LessThanLine(pos, location2, source) &&
        //            !LessThanLine(pos, location1, location2))
        //        {
        //            control = true;
        //        }
        //    }
        //    else
        //    {
        //        if ( LessThanLine(pos, location1, source) &&
        //            !LessThanLine(pos, location2, source) &&
        //            !LessThanLine(pos, location1, location2))
        //        {
        //            control = true;
        //        }
        //    }

        //    return control;
        //}

        private bool CheckCorners(Position location1, Position location2, Position source, Position pos, bool isLeftHalf, bool isUpperHalf)
        {
            var k1 = 0.0;
            var k2 = 0.0;
            bool control = false;

            if (location1.X - source.X == 0)
            {
                k1 = int.MaxValue;
            }
            else if(location2.X - source.X == 0)
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
                if( LessThanLine(pos, source, location1) &&
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
                if(!LessThanLine(pos, source, location1) &&
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

        //private bool CheckFirstFirst(Position location1, Position location2, Position source, Position pos)
        //{
        //    var k1 = Math.Abs((location1.Y - source.Y) / (location1.X - source.X));
        //    var k2 = Math.Abs((location2.Y - source.Y) / (location2.X - source.X));
        //    bool control = false;

        //    if (k1 > k2)
        //    {
        //        var tmpLoc = location1;
        //        location1 = location2;
        //        location2 = tmpLoc;
        //    }

        //    if ( LessThanLine(pos, source, location1) &&
        //        !LessThanLine(pos, source, location2))
        //    {
        //        if (location1.X < location2.X)
        //        {
        //            control = !LessThanLine(pos, location1, location2);
        //        }
        //        else
        //        {
        //            control = LessThanLine(pos, location1, location2);
        //        }
        //    }

        //    return control;
        //}

        //private bool CheckSecondSecond(Position location1, Position location2, Position source, Position pos)
        //{
        //    var k1 = Math.Abs((location1.Y - source.Y) / (location1.X - source.X));
        //    var k2 = Math.Abs((location2.Y - source.Y) / (location2.X - source.X));
        //    bool control = false;

        //    if (k1 > k2)
        //    {
        //        var tmpLoc = location1;
        //        location1 = location2;
        //        location2 = tmpLoc;
        //    }

        //    if ( LessThanLine(pos, source, location1) &&
        //        !LessThanLine(pos, source, location2))
        //    {
        //        if (location1.X < location2.X)
        //        {
        //            control = LessThanLine(pos, location1, location2);
        //        }
        //        else
        //        {
        //            control = !LessThanLine(pos, location1, location2);
        //        }
        //    }

        //    return control;
        //}

        //private bool CheckThirdThird(Position location1, Position location2, Position source, Position pos)
        //{
        //    var k1 = Math.Abs((location1.Y - source.Y) / (location1.X - source.X));
        //    var k2 = Math.Abs((location2.Y - source.Y) / (location2.X - source.X));
        //    bool control = false;

        //    if (k1 > k2)
        //    {
        //        var tmpLoc = location1;
        //        location1 = location2;
        //        location2 = tmpLoc;
        //    }

        //    if (!LessThanLine(pos, source, location1) &&
        //         LessThanLine(pos, source, location2))
        //    {
        //        if (location1.X < location2.X)
        //        {
        //            control = !LessThanLine(pos, location1, location2);
        //        }
        //        else
        //        {
        //            control = LessThanLine(pos, location1, location2);
        //        }
        //    }

        //    return control;
        //}


        //private bool CheckFourthFourth(Position location1, Position location2, Position source, Position pos)
        //{
        //    var k1 = Math.Abs((location1.Y - source.Y) / (location1.X - source.X));
        //    var k2 = Math.Abs((location2.Y - source.Y) / (location2.X - source.X));
        //    bool control = false;

        //    if (k1 > k2)
        //    {
        //        var tmpLoc = location1;
        //        location1 = location2;
        //        location2 = tmpLoc;
        //    }

        //    if (!LessThanLine(pos, source, location1) &&
        //         LessThanLine(pos, source, location2))
        //    {
        //        if (location1.X < location2.X)
        //        {
        //            control = LessThanLine(pos, location1, location2);
        //        }
        //        else
        //        {
        //            control = !LessThanLine(pos, location1, location2);
        //        }
        //    }
            
        //    return control;
        //}

        //private bool CheckTopCorner(Position location1, Position location2, Position source, Position pos, bool reverse)
        //{
            


        //    bool control = false;
        //    var loc = location1.X < location2.X;
        //    if(reverse)
        //    {
        //        loc = !loc;
        //    }

        //    if (loc)
        //    {
        //        if (!LessThanLine(pos, location1, source) &&
        //             LessThanLine(pos, location2, source) &&
        //             LessThanLine(pos, location1, location2))
        //        {
        //            control = true;
        //        }
        //    }
        //    else
        //    {
        //        if ( LessThanLine(pos, location1, source) &&
        //            !LessThanLine(pos, location2, source) &&
        //             LessThanLine(pos, location1, location2))
        //        {
        //            control = true;
        //        }
        //    }

        //    return control;
        //}

        private bool LessThanLine(Position pos, Position line1, Position line2)
        {
            if(line1.X - line2.X == 0)
            {
                return pos.X < line1.X;
            }
            return pos.Y <= SolveLine(line1, line2, pos.X);
        }

        private int SolveLine(Position location1, Position location2, int x)
        {
            return (location2.Y - location1.Y) * (x - location1.X) / (location2.X - location1.X) + location1.Y;
        }

        private Quater FindLocation(Position point, Position supportingPoint)
        {
            if(point.X >= supportingPoint.X)
            {
                if(point.Y >= supportingPoint.Y)
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

        private bool IsInAreas(Position pos, List<Area> areas)
        {
            foreach(var area in areas)
            {
                var xLeft = area.Centre.X - area.Width / 2;
                var xRight = area.Centre.X + area.Width / 2;
                var yTop = area.Centre.Y - area.Height / 2;
                var yBottom = area.Centre.Y + area.Height / 2;

                if ( pos.X > xLeft && pos.X < xRight &&
                     pos.Y > yTop && pos.Y < yBottom )
                {
                    return true;
                }
            }
            return false;
        }
    }
}