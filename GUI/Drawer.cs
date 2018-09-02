using GameEngine.Utility;
using System;
using System.Drawing;

namespace GUI
{
    internal static class Drawer
    {       
        internal static Bitmap DrawMap(string[] map, int width, int height, double rescaleFactor)
        {
            Bitmap result = new Bitmap(width, height);

            foreach (var line in map)
            {
                ParseAndDraw(line, result, rescaleFactor);
            }

            return result;
        }

        private static void ParseAndDraw(string line, Bitmap img, double rescaleFactor)
        {
            var param = line.Split(';');

            var type = int.Parse(param[0]);
            var pos = param[3].Split(':');
            var xPos = (int)(rescaleFactor * int.Parse(pos[0]));
            var yPos = (int)(rescaleFactor * int.Parse(pos[1]));

            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.Black);

            if((ObjectType)type == ObjectType.Block)
            { 
                var w = (int)(rescaleFactor * int.Parse(param[4]));
                var h = (int)(rescaleFactor * int.Parse(param[5]));
                g.DrawRectangle(p, xPos - w/2, yPos - h/2, w, h);
            }
            if((ObjectType)type == ObjectType.Bullet)
            {
                var r = (int)(rescaleFactor * int.Parse(param[4]));
                g.DrawEllipse(p, xPos - r, yPos - r, r * 2, r * 2);
            }
            if ((ObjectType)type == ObjectType.Field)
            {
                var w = (int)(rescaleFactor * int.Parse(param[4]));
                var h = (int)(rescaleFactor * int.Parse(param[5]));
                g.DrawRectangle(p, xPos, yPos, w, h);
            }
            if ((ObjectType)type == ObjectType.Player)
            {
                var r = (int)(rescaleFactor * int.Parse(param[4]));
                var r1 = r / 5;
                var angle = int.Parse(param[5]);
                g.DrawEllipse(p, xPos - r, yPos - r, r * 2, r * 2);

                var xPosGun = xPos + (int)Math.Round(r * Math.Cos(angle * Math.PI / 180));
                var yPosGun = yPos + (int)Math.Round(r * Math.Sin(angle * Math.PI / 180));
                g.FillEllipse(Brushes.Black, xPosGun - r1, yPosGun - r1, r1 * 2, r1 * 2);

                var id = param[1];
                Rectangle rect = new Rectangle(xPos - r/2, yPos - r/2, r, r);

                g.DrawString(id, new Font("Times New Roman", 18.0f), Brushes.Black, rect);
            }
        }
    }
}