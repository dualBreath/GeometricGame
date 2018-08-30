using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GameEngine;
using GameEngine.Utility;

namespace GUI
{
    public partial class Form1 : Form
    {
        private volatile Game gameShooter;
        private volatile string[] map;
        private Thread game;

        public Form1()
        {
            InitializeComponent();
            LoadGame();

            game = new Thread(new ThreadStart(Start));
            game.Start();
        }

        private void LoadGame()
        {
            gameShooter = new Game();
            gameShooter.LoadGame();

            var img = new Bitmap(mainField.Width, mainField.Height);
            mainField.Image = img;

            map = gameShooter.GetMap();
            Drawer.DrawMap(map, mainField);
        }
        
        private void Start()
        {
            gameShooter.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
            {
                gameShooter.Step(GameKeys.Move);
            }
            else if (e.KeyData == Keys.Left)
            {
                gameShooter.Step(GameKeys.Left);
            }
            else if (e.KeyData == Keys.Right)
            {
                gameShooter.Step(GameKeys.Right);
            }
            else if (e.KeyData == Keys.Space)
            {
                gameShooter.Step(GameKeys.Shoot);
            }
        }

        private void mainField_Paint(object sender, PaintEventArgs e)
        {
            if (gameShooter != null && gameShooter.GetMap() != null)
            {
                var nextMap = gameShooter.GetMap();
                if (map != nextMap)
                {
                    map = nextMap;
                    Drawer.DrawMap(map, mainField);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gameShooter != null)
            {
                gameShooter.Stop();
            }
            if (game != null)
            {
                game.Abort();
            }
            Application.Exit();
        }
    }
}
