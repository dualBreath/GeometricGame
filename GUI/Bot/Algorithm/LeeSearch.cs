using AI.Util;
using GameEngine.Utility;
using System.Collections.Generic;

namespace AI.Algorithm
{
    static class LeeSearch
    {
        public static Position FindFirstStep(Position start, Greed greed)
        {
            var waves = new List<HashSet<Position>>();
            waves.Add(new HashSet<Position>());
            var finish = new Position(0, 0);
            bool isFind = FindFinish(waves, start, greed, out finish);

            if(!isFind)
            {
                return null;
            }

            Position firstStep = ReestablishPath(waves, start, finish, greed);

            return firstStep;
        }

        private static Position ReestablishPath(List<HashSet<Position>> waves, Position start, Position finish, Greed greed)
        {
            var current = finish;
            var level = waves.Count - 2;
            var step = current;
            var isBreak = false;

            while(!isBreak && level > -1)
            {
                isBreak = true;
                var neighbors = greed.GetNearests(current);
                foreach(var neighbor in neighbors)
                {
                    if (waves[level].Contains(neighbor.Centre))
                    {
                        isBreak = false;
                        level--;
                        step = current;
                        current = neighbor.Centre;
                        break;
                    }
                }
            }
            if(isBreak)
            {
                return null;
            }
            return step;
        }

        private static bool FindFinish(List<HashSet<Position>> waves, Position start, Greed greed, out Position finish)
        {
            int level = 0;
            var count = 1;
            waves[0].Add(start);
            var total = greed.Total();
            var totalCount = 0;

            while (count > 0 && totalCount <= total)
            {
                count = 0;
                waves.Add(new HashSet<Position>());

                foreach (var point in waves[level])
                { 
                    var neighbors = greed.GetNearests(point);
                    
                    foreach (var neighbor in neighbors)
                    {  
                        if (!waves[level].Contains(neighbor.Centre))
                        {
                            if (greed.IsInDestinations(neighbor.Centre) && 
                               !greed.IsInBlocks(neighbor.Centre) &&
                                greed.IsInGreed(neighbor.Centre))
                            {
                                finish = new Position(neighbor.Centre.X, neighbor.Centre.Y); ;
                                return true;
                            }
                            else
                            {
                                count++;
                                totalCount += 1;
                                waves[level + 1].Add(neighbor.Centre);
                            }
                        }
                    }
                }
                level++;
            }

            finish = null;
            return false;
        }
    }
}
