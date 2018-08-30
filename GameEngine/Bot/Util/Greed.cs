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

        public void SetBlocks(List<Area> blocks)
        {
            this.blocks = blocks;
        }

        public void SetDestinations(List<Shadow> destinations)
        {
            this.shadows = destinations;
        }

        public List<Area> GetNearests(Position from)
        {
            var nearests = new List<Area>();
            var pos = new Position(from.X, from.Y);
            pos.Y = from.Y - distance;
            
            if(IsInGreed(pos) && !IsInBlocks(pos))
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

        //public bool IsInGreed(Position pos)
        //{
        //    return pos.X > -1 && pos.X < width &&
        //           pos.Y > -1 && pos.Y < height;
        //}

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
            //if(pos.X == 140 && pos.Y == 30)
            //{

            //}
            var location1 = FindLocation(shadow.FrontPoint1, shadow.Source);
            var location2 = FindLocation(shadow.FrontPoint2, shadow.Source);

            if(location1 == Quater.First && location2 == Quater.First)
            {
                return CheckQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, false);
            }
            if(location1 == Quater.Second && location2 == Quater.Second)
            {
                return CheckQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, false);
            }
            if (location1 == Quater.Third && location2 == Quater.Third)
            {
                return CheckQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false, true);
            }
            if (location1 == Quater.Fourth && location2 == Quater.Fourth)
            {
                return CheckQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, true);
            }
            if(location1 == Quater.First && location2 == Quater.Second ||
               location2 == Quater.First && location1 == Quater.Second)
            {
                return CheckQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true, false);
            }
            if (location1 == Quater.First && location2 == Quater.Fourth ||
               location2 == Quater.First && location1 == Quater.Fourth)
            {
                return CheckQuaterY(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true);
            }
            if (location1 == Quater.Third && location2 == Quater.Second ||
               location2 == Quater.Third && location1 == Quater.Second)
            {
                return CheckQuaterY(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false);
            }
            if (location1 == Quater.First && location2 == Quater.Third ||
                location2 == Quater.First && location1 == Quater.Third)
            {
                if (SolveLine(shadow.FrontPoint1, shadow.FrontPoint2, 0) > shadow.Source.Y)
                {
                    return CheckSemiQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false);
                }
                else
                {
                    return CheckSemiQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true);
                }
            }
            if (location1 == Quater.Second && location2 == Quater.Fourth ||
                location2 == Quater.Second && location1 == Quater.Fourth)
            {
                if (SolveLine(shadow.FrontPoint1, shadow.FrontPoint2, 0) > shadow.Source.Y)
                {
                    return CheckSemiQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, false);
                }
                else
                {
                    return CheckSemiQuaters(shadow.FrontPoint1, shadow.FrontPoint2, shadow.Source, pos, true);
                }
            }

            return true;
        }

        private bool CheckSemiQuaters(Position location1, Position location2, Position source, Position pos, bool reverse)
        {
            var control = false;
            if(reverse)
            {
                if (!LessThanLine(pos, location1, source) && //if (pos.Y >= SolveLine(location1, source, pos.X) &&
                    !LessThanLine(pos, location2, source))    //    pos.Y >= SolveLine(location2, source, pos.X))
                {
                    if (!LessThanLine(pos, location2, location1))//if (pos.Y >= SolveLine(location2, location1, pos.X))
                    {
                        control = true;
                    }
                }
            }
            else
            {
                if (LessThanLine(pos, location1, source) && //if (pos.Y <= SolveLine(location1, source, pos.X) &&                
                    LessThanLine(pos, location2, source))   //    pos.Y <= SolveLine(location2, source, pos.X))
                {
                    if(LessThanLine(pos, location2, location1))//if (pos.Y <= SolveLine(location2, location1, pos.X))
                    {
                        control = true;
                    }
                }
            }

            return control;
        }

        private bool LessThanLine(Position pos, Position line1, Position line2)
        {
            if(line1.X - line2.X == 0)
            {
                return pos.X < line1.X;
            }
            return pos.Y <= SolveLine(line1, line2, pos.X);
        }

        private bool CheckQuaterY(Position location1, Position location2, Position source, Position pos, bool reverseFront)
        {
            var control = false;
            var reverse = location1.X < location2.X;

            if(reverseFront)
            {
                reverse = !reverse;
            }

            if (location1.Y < location2.Y)
            {
                if(!LessThanLine(pos, location1, source) &&     //if (pos.Y >= SolveLine(location1, source, pos.X) &&
                    LessThanLine(pos, location2, source))       //    pos.Y <= SolveLine(location2, source, pos.X))
                {
                    if(reverse)
                    {
                        if (!LessThanLine(pos, location1, location2))//if (pos.Y >= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                    else
                    {
                        if (LessThanLine(pos, location1, location2))//if (pos.Y <= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                }
            }
            else
            {
                if (!LessThanLine(pos, location2, source) && //if (pos.Y >= SolveLine(location2, source, pos.X) &&
                     LessThanLine(pos, location1, source))   //    pos.Y <= SolveLine(location1, source, pos.X))
                    {
                    if (!reverse)
                    {
                        if (!LessThanLine(pos, location1, location2))//if (pos.Y >= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                    else
                    {
                        if (LessThanLine(pos, location1, location2))//if (pos.Y <= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                }
            }
            return control;
        }
        
        private bool CheckQuaters(Position location1, Position location2, Position source, Position pos, bool reverse, bool reverseFront)
        {
            bool control = false;
            var condition = location1.X < location2.X;
            if(reverse)
            {
                condition = !condition;
            }
            if (condition)
            {
                if (!LessThanLine(pos, location1, source) &&    //if(pos.Y >= SolveLine(location1, source, pos.X) &&
                     LessThanLine(pos, location2, source))      //   pos.Y <= SolveLine(location2, source, pos.X))
                    {
                    if(reverseFront)
                    {
                        if (!LessThanLine(pos, location1, location2))//if (pos.Y >= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                    else if (LessThanLine(pos, location1, location2))//if (pos.Y <= SolveLine(location1, location2, pos.X))
                    {
                        control = true;
                    }                 
                }
            }
            else
            {
                if (!LessThanLine(pos, location2, source) &&  //if (pos.Y >= SolveLine(location2, source, pos.X) &&
                     LessThanLine(pos, location1, source))    //    pos.Y <= SolveLine(location1, source, pos.X))
                    {
                    if (reverseFront)
                    {
                        if (!LessThanLine(pos, location1, location2))//if (pos.Y >= SolveLine(location1, location2, pos.X))
                        {
                            control = true;
                        }
                    }
                    else if (LessThanLine(pos, location1, location2))//if (pos.Y <= SolveLine(location1, location2, pos.X))
                    {
                        control = true;
                    }
                }
            }
            return control;
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
                if (point.Y >= supportingPoint.Y)
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