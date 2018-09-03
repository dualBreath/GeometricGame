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
        private List<Pair<int, int>> nearests;


        public Greed(int width, int height, int size, int distance)
        {
            this.width = width;
            this.height = height;
            this.distance = distance;
            this.size = size;

            nearests = new List<Pair<int, int>>()
            {
                //new Pair<int, int>(-1, -1),                
                //new Pair<int, int>(1, -1),

                new Pair<int, int>(0, -1),
                new Pair<int, int>(-1, 0),
                new Pair<int, int>(1, 0),
                new Pair<int, int>(0, 1),

                //new Pair<int, int>(-1, 1),                
                //new Pair<int, int>(1, 1),
            };
        }

        public int Total()
        {
            return (int)((double)width / distance * height / distance);
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
            var result = new List<Area>();
            var baseX = from.X;
            var baseY = from.Y;

            foreach (var near in nearests)
            {
                var pos = new Position(baseX + near.First * distance, baseY + near.Second * distance);

                if (IsInGreed(pos) && !IsInBlocks(pos))
                {
                    result.Add(new Area(pos.X, pos.Y, distance, distance));
                }
            }

            return result;
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
                if(Algorithms.IsCloses(shadow, pos))
                {
                    return false;
                }
            }
            return true;
        }
        
        private bool IsInAreas(Position pos, List<Area> areas)
        {
            foreach(var area in areas)
            {
                if (Algorithms.IsInArea(pos, area))
                {
                    return true;
                }
            }
            return false;
        }
    }
}