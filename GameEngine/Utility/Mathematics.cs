using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Utility
{
    public static class Mathematics
    {
        public static double Distance(Position pos1, Position pos2)
        {
            return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
        }

        public static bool IsInField(int width, int height, int size, Position centre)
        {
            var control = true;

            control &= centre.X - size >= 0;
            control &= centre.X + size < width;

            control &= centre.Y + size < height;
            control &= centre.Y - size >= 0;

            return control;
        }

        public static double Angle(double a, double b, double c)
        {
            return Math.Acos((a * a + b * b - c * c) / (2 * a * b));
        }
    }
}
