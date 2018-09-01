using GameEngine.Entities;
using GameEngine.Interfaces;
using GameEngine.Utility;

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

        internal static Shadow CreateShadow(Position player, Block block, int buffer)
        {
            FindCorners(block, player, buffer, out var corner1, out var corner2);
            return new Shadow(corner1, corner2, player);
        }

        private static void FindCorners(Block block, Position player, int buffer, out Position corner1, out Position corner2)
        {
            var leftTop = new Position(block.Centre.X - block.Width / 2 - buffer, block.Centre.Y - block.Height / 2 - buffer);
            var rightTop = new Position(block.Centre.X + block.Width / 2 + buffer, block.Centre.Y - block.Height / 2 - buffer);
            var leftBottom = new Position(block.Centre.X - block.Width / 2 - buffer, block.Centre.Y + block.Height / 2 + buffer);
            var rightBottom = new Position(block.Centre.X + block.Width / 2 + buffer, block.Centre.Y + block.Height / 2 + buffer);

            var angleLT = CalcAngle(player, leftTop, block.Centre);
            var angleRT = CalcAngle(player, rightTop, block.Centre);
            var angleLB = CalcAngle(player, leftBottom, block.Centre);
            var angleRB = CalcAngle(player, rightBottom, block.Centre);

            var max = angleLT + angleRT;
            var first = leftTop;
            var second = rightBottom;
            if (angleLT + angleLB > max)
            {
                max = angleLT + angleLB;
                first = leftTop;
                second = leftBottom;
            }
            if (angleLT + angleRB > max)
            {
                max = angleLT + angleRB;
                first = leftTop;
                second = rightBottom;
            }
            if (angleRB + angleRT > max)
            {
                max = angleRB + angleRT;
                first = rightBottom;
                second = rightTop;
            }
            if (angleRB + angleLB > max)
            {
                max = angleRB + angleLB;
                first = rightBottom;
                second = leftBottom;
            }
            if (angleRT + angleLB > max)
            {
                max = angleRT + angleLB;
                first = rightTop;
                second = leftBottom;
            }
            corner1 = first;
            corner2 = second;
        }

        private static double CalcAngle(Position aPos, Position bPos, Position cPos)
        {
            var a = Mathematics.Distance(aPos, cPos);
            var b = Mathematics.Distance(aPos, bPos);
            var c = Mathematics.Distance(bPos, cPos);

            var angle = Mathematics.Angle(a, b, c);
            return angle;
        }
    }
}
