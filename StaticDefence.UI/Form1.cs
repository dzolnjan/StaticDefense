using StaticDefence.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace StaticDefence.UI
{
    public partial class Form1 : Form
    {
        Game game;
        Pen myPen;
        Thread gameThread;
        bool running = true;

        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            myPen = new Pen(Color.Black);

            InitGame();

            System.Windows.Forms.Timer Clock = new System.Windows.Forms.Timer();
            Clock.Interval = 10;
            Clock.Start();
            Clock.Tick += new EventHandler(Timer_Tick);
        }

        public void InitGame()
        {
            float roadThicknes = 30;
            float upOffset = 10;

            game = new Game(20) { Money = 10, Life = 50 };

            Level level = new Level() { Count = 10, Money = 5, Speed = 0.6F, Points = 1, SpawnDelayMilis = 1000, Width = 10, Height = 10, HitPoints = 40, Ground = true, Active = true };
            game.Levels.Add(level);
            level = new Level() { Count = 10, Money = 10, Speed = 0.6F, Points = 1, SpawnDelayMilis = 1500, Width = 10, Height = 10, HitPoints = 40, Ground = false, Active = false };
            game.Levels.Add(level);
            level = new Level() { Count = 10, Money = 5, Speed = 0.5F, Points = 1, SpawnDelayMilis = 1000, Width = 10, Height = 10, HitPoints = 60, Ground = true, Active = false };
            game.Levels.Add(level);

            Map map = new Map() { Start = new PointF(0, 10), End = new PointF(450, 170), RoadThickness = 20 };
            game.Map = map;
            map.AddRoad(100, Directions.Right);
            map.AddRoad(50, Directions.Down);
            map.AddRoad(50, Directions.Left);
            map.AddRoad(50, Directions.Down);
            map.AddRoad(150, Directions.Right);
            map.AddRoad(100, Directions.Up);
            map.AddRoad(90, Directions.Right);
            map.AddRoad(50, Directions.Down);
            map.AddRoad(130, Directions.Right);

            map.FlyMiddles.Add(new PointF(200, 30));
            map.FlyMiddles.Add(new PointF(200, 50));
            map.FlyMiddles.Add(new PointF(200, 70));

            // buy towers
            SimpleTower tower = new SimpleTower() { Center = new PointF(200, 300), Price = 10, Range = 100, TargetType = TargetTypes.All, Active = false, Dummy = true };
            game.Towers.Add(tower);
            SonicTower sonicTower = new SonicTower() { Center = new PointF(250, 300), Price = 50, Range = 100, TargetType = TargetTypes.All, Active = false, Dummy = true };
            game.Towers.Add(sonicTower);
            HeavyTower heavyTower = new HeavyTower() { Center = new PointF(300, 300), Price = 20, Range = 100, SlowDuration = 50, SlowPercent = 50, TargetType = TargetTypes.Ground, Active = false, Dummy = true };
            game.Towers.Add(heavyTower);
            TeslaTower teslaTower = new TeslaTower() { Center = new PointF(350, 300), Price = 20, Range = 150, TargetType = TargetTypes.All, Active = false, Dummy = true };
            game.Towers.Add(teslaTower);


            // active towers
            tower = new SimpleTower() { Center = new PointF(150, 80), Range = 100, TargetType = TargetTypes.All, Active = true, Placed = true };
            game.Towers.Add(tower);
            sonicTower = new SonicTower() { Center = new PointF(150, 120), Range = 100, TargetType = TargetTypes.All, Active = true, Placed = true };
            game.Towers.Add(sonicTower);
            heavyTower = new HeavyTower() { Center = new PointF(150, 100), Range = 100, SlowDuration = 10, SlowPercent = 50, TargetType = TargetTypes.Ground, Active = true, Placed = true };
            game.Towers.Add(heavyTower);
            teslaTower = new TeslaTower() { Center = new PointF(150, 50), Range = 200, TargetType = TargetTypes.All, Active = true, Placed = true };
            game.Towers.Add(teslaTower);

            game.Running = true;
        }

        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (running)
            {
                game.Update(false);
                this.SuspendLayout();
                this.Refresh();
                this.ResumeLayout(false);
            }
        }


        public void Updating()
        {
            while (game.Running)
            {
                game.Update(false);

            }
            gameThread.Join();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            running = false;
            base.OnClosing(e);
        }

        protected override void OnPaint(PaintEventArgs paintEvnt)
        {
            base.OnPaint(paintEvnt);
            Graphics gfx = paintEvnt.Graphics;
            game.Draw(gfx, myPen);
            lblLife.Text = game.Life.ToString();
            lblMoney.Text = game.Money.ToString();
            lblPoints.Text = game.Points.ToString();
            lblNextWave.Text = game.NextLevelCounterSeconds.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            game.Running = !game.Running;
            button1.Text = game.Running ? "Pause" : "Play";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitGame();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            List<BaseTower> towers = game.Towers.Where(x => x.Dummy && (float)e.X > x.Center.X - x.Width / 2 && e.X < x.Center.X + x.Width / 2 && e.Y > x.Center.Y - Height / 2 && e.Y < x.Center.Y + Height / 2).ToList();
            if (towers.Count > 0 && towers.First().CanBuyIt(game.Money))
            {
                BaseTower copy = Utils.ObjectCopier.Clone<BaseTower>(towers.First());
                copy.EnableFire(false);
                game.SelectedTower = copy;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (game.SelectedTower != null)
            {
                if (!game.SelectedTower.IsOverlapingRoads(game.Map))
                {
                    if (game.Running)
                        game.TowersToAdd.Add(game.SelectedTower);
                    else // this is only enable tower placing and drwing on if paused
                    {
                        game.Towers.Add(game.SelectedTower);
                        game.SelectedTower.EnableFire(true);
                        Refresh();
                    }

                    game.Money = game.Money - game.SelectedTower.Price;
                    game.SelectedTower = null;


                }
                else
                {
                    game.SelectedTower = null;
                    if (!game.Running)
                        Refresh();
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (game.SelectedTower != null)
            {
                game.SelectedTower.Center = new PointF(e.X, e.Y);
                game.SelectedTower.InvalidPosisiton = game.SelectedTower.IsOverlapingRoads(game.Map);
                if (!game.Running)
                {
                    game.Draw(this.CreateGraphics(), myPen);
                    this.Refresh();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            game.SendNextLevel();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
