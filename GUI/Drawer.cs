using GameEngine.Utility;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    internal static class Drawer
    {       
        internal static void DrawMap(string[] map, PictureBox mainField)
        {
            Bitmap result = new Bitmap(mainField.Width, mainField.Height);

            foreach (var line in map)
            {
                ParseAndDraw(line, result);
            }
            mainField.Image = result;
            mainField.Invalidate();
        }

        private static void ParseAndDraw(string line, Bitmap img)
        {
            var param = line.Split(';');

            var type = int.Parse(param[0]);
            var pos = param[3].Split(':');
            var xPos = int.Parse(pos[0]);
            var yPos = int.Parse(pos[1]);

            Graphics g = Graphics.FromImage(img);
            Pen p = new Pen(Color.Black);

            if((ObjectType)type == ObjectType.Block)
            { 
                var w = int.Parse(param[4]);
                var h = int.Parse(param[5]);
                g.DrawRectangle(p, xPos - w/2, yPos - h/2, w, h);
            }
            if((ObjectType)type == ObjectType.Bullet)
            {
                var r = int.Parse(param[4]);
                g.DrawEllipse(p, xPos - r, yPos - r, r * 2, r * 2);
            }
            if ((ObjectType)type == ObjectType.Field)
            {
                var w = int.Parse(param[4]);
                var h = int.Parse(param[5]);
                g.DrawRectangle(p, xPos, yPos, w, h);
            }
            if ((ObjectType)type == ObjectType.Player)
            {
                var r = int.Parse(param[4]);
                var r1 = r / 5;
                var angle = int.Parse(param[5]);
                g.DrawEllipse(p, xPos - r, yPos - r, r * 2, r * 2);

                var xPosGun = xPos + (int)Math.Round(r * Math.Cos(angle * Math.PI / 180));
                var yPosGun = yPos + (int)Math.Round(r * Math.Sin(angle * Math.PI / 180));
                g.FillEllipse(Brushes.Black, xPosGun - r1, yPosGun - r1, r1 * 2, r1 * 2);
            }
        }
    }
}