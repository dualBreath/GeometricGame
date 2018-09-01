using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using GameEngine;
using GameEngine.Utility;

namespace GUI
{
    public partial class Form1 : Form
    {
        private Game gameShooter;
        private Thread game;
        private GameActions playerStep;
        private bool stopGame;
        private volatile Bitmap mapImage;

        private double rescaleFactor = 1;//0.01;


        public delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();
            LoadGame();

            game = new Thread(new ThreadStart(Start));
            game.Start();

            stopGame = false;
        }

        private void LoadGame()
        {
            gameShooter = new Game();
            gameShooter.LoadGame();
            
            var map = gameShooter.GetMap();
            mapImage = Drawer.DrawMap(map, mainField.Width, mainField.Height, rescaleFactor);
        }
        
        private void Start()
        {
            var timer = new Stopwatch();
            timer.Start();
            var botStep = GameActions.None;

            while (!stopGame)
            {
                if (timer.ElapsedMilliseconds > 100)
                {
                    var map = gameShooter.GetMap();
                    botStep = AI.Bot.Decide(map, 1);

                    gameShooter.DoStep(0, playerStep);
                    gameShooter.DoStep(1, botStep);
                    gameShooter.DoPassiveActions();
                    
                    mapImage = Drawer.DrawMap(map, mainField.Width, mainField.Height, rescaleFactor);
                    InvokeUpdateImage();

                    if(gameShooter.IsLevelEnded())
                    {
                        gameShooter.SaveGame();
                        LoadGame();
                    }

                    playerStep = GameActions.None;
                    botStep = GameActions.None;
                    timer.Restart();
                }              
            }
        }

        private void InvokeUpdateImage()
        {
            mainField.Image = mapImage;
            mainField.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.P)
            {
                stopGame = true;
            }
            if (e.KeyData == Keys.Up)
            {
                playerStep = GameActions.Move;
            }
            else if (e.KeyData == Keys.Left)
            {
                playerStep = GameActions.Right;
            }
            else if (e.KeyData == Keys.Right)
            {
                playerStep = GameActions.Left;
            }
            else if (e.KeyData == Keys.Space)
            {
                playerStep = GameActions.Shoot;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (game != null)
            {
                game.Abort();
            }
            Application.Exit();
        }

        private void mainField_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            mainField.BeginInvoke(new InvokeDelegate(InvokeUpdateImage));
        }
    }
}
