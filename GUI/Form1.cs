using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AI;
using GameEngine;
using GameEngine.Utility;

namespace GUI
{
    public partial class Form1 : Form
    {
        private volatile Game gameShooter;
        private volatile GameActions playerStep;
        private volatile bool stopGame;
        private volatile bool pauseGame;
        private volatile Bitmap mapImage;
        private volatile Bot bot;
        private volatile string[] statistics;

        private Thread game;

        private volatile int rescaleFactor = 1;

        private volatile string resourcesDirectoryPath;
        private volatile string gameStateFileName = "game_state.txt";
        private volatile string gameLevelFileName = "level.txt";

        public delegate void InvokeDelegate();

        public Form1()
        {
            InitializeComponent();

            resourcesDirectoryPath = Directory.GetCurrentDirectory() + "\\Resources";
            
            game = new Thread(new ThreadStart(Start))
            {
                IsBackground = true
            };

            stopGame = false;            
        }

        private void Start()
        {
            LoadGame();
            var timer = new Stopwatch();            
            var botStep = GameActions.None;
            var gameStatePath = resourcesDirectoryPath + "\\" + gameStateFileName;

            timer.Start();

            while (!stopGame)
            {
                while (!pauseGame)
                {
                    if (timer.ElapsedMilliseconds > 100)
                    {
                        var map = gameShooter.GetMap();
                        botStep = bot.Decide(map);

                        gameShooter.DoStep(0, playerStep);
                        gameShooter.DoStep(1, botStep);
                        gameShooter.DoPassiveActions();

                        mapImage = Drawer.DrawMap(map, mainField.Width, mainField.Height, rescaleFactor);
                        mainField.BeginInvoke(new InvokeDelegate(InvokeUpdateImage));

                        if (gameShooter.IsLevelEnded())
                        {
                            gameShooter.SaveGame(gameStatePath);
                            LoadGame();
                        }

                        playerStep = GameActions.None;
                        botStep = GameActions.None;
                        timer.Restart();
                    }
                }
            }
        }

        private void LoadGame()
        {
            var gameStatePath = resourcesDirectoryPath + "\\" + gameStateFileName;
            var gameLevelPath = resourcesDirectoryPath + "\\" + gameLevelFileName;
            gameShooter = new Game();
            gameShooter.LoadGame(gameStatePath, gameLevelPath);
            statistics = gameShooter.GetStatistics();
            textBoxStatistics.BeginInvoke(new InvokeDelegate(InvokeUpdateStatistics));

            var map = gameShooter.GetMap();
            mapImage = Drawer.DrawMap(map, mainField.Width, mainField.Height, rescaleFactor);
            bot = new Bot(1);
            bot.CreateMovingController(map);
        }

        private void InvokeUpdateImage()
        {
            mainField.Image = mapImage;
            mainField.Invalidate();
        }

        private void InvokeUpdateStatistics()
        {
            textBoxStatistics.Text = "";
            foreach (var line in statistics)
            {
                textBoxStatistics.Text += line + Environment.NewLine;
            }
            textBoxStatistics.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.P)
            {
                pauseGame = !pauseGame;
            }
            if (e.KeyData == Keys.W)
            {
                playerStep = GameActions.Move;
            }
            else if (e.KeyData == Keys.A)
            {
                playerStep = GameActions.Left;
            }
            else if (e.KeyData == Keys.D)
            {
                playerStep = GameActions.Right;
            }
            else if (e.KeyData == Keys.Q)
            {
                playerStep = GameActions.FastLeft;
            }
            else if (e.KeyData == Keys.E)
            {
                playerStep = GameActions.FastRight;
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

        private void buttonStart_MouseClick(object sender, MouseEventArgs e)
        {
            buttonStart.Visible = false;
            game.Start();
        }

        private void buttonPause_MouseClick(object sender, MouseEventArgs e)
        {
            if(pauseGame)
            {
                buttonPause.Text = "Pause";
            }
            else
            {
                buttonPause.Text = "Resume";
            }
            pauseGame = !pauseGame;
        }
    }
}
