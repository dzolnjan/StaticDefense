using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace StaticDefence.Core
{
    public class Game
    {
        public bool Running { get; set; }
        public int Points { get; set; }
        public int Life { get; set; }
        public decimal Money { get; set; }
        public int NextLevelCounterSeconds { get; set; }
        protected int _designatedNextLevelCounterSeconds { get; set; }
        public Map Map;

        System.Windows.Forms.Timer Clock = new System.Windows.Forms.Timer();
        
        public List<BaseEnemy> Enemies { get; set; }
        public List<BaseBullet> Bullets { get; set; }
        public List<BaseTower> Towers { get; set; }
        public List<BaseTower> TowersToAdd { get; set; }
        public List<Level> Levels { get; set; }

        public BaseTower SelectedTower { get; set; }

        protected bool Updating { get; set; }

        public Game(int nextWaveCounterSec)
        {
            _designatedNextLevelCounterSeconds = nextWaveCounterSec;
            NextLevelCounterSeconds = nextWaveCounterSec;

            Clock.Interval = 1000;
            Clock.Start();
            Clock.Tick += new EventHandler(Timer_Tick);

            Map = new Map();

            Enemies = new List<BaseEnemy>();
            Bullets = new List<BaseBullet>();
            Towers = new List<BaseTower>();
            TowersToAdd = new List<BaseTower>();
            Levels = new List<Level>();
        }


        public void Timer_Tick(object sender, EventArgs eArgs)
        {
            if (Running)
            {
                NextLevelCounterSeconds--;
            }
        }

        public void SendNextLevel()
        {
            //Level level = null;
            if (Levels.Where(x => !x.Active).Count() > 0)
            {
                Points = Points + NextLevelCounterSeconds;
                Levels.First(x => !x.Active).Active = true;
                NextLevelCounterSeconds = _designatedNextLevelCounterSeconds;

            }
        }

        public void SpawnEnemies(int hitPoints, int height, int width, bool ground, int count, int milis)
        {
            Thread spawnThread =
         new Thread(unused => SpawnEnemiesStart(hitPoints, height, width, ground, count, milis));
            spawnThread.Start();

        }

        private void SpawnEnemiesStart(int hitPoints, int height, int width, bool ground, int count, int milis)
        {
            while (count > 0)
            {
                count--;
                if (ground)
                {
                    BaseEnemy enemy = new GroundEnemy(1.3F, hitPoints, 100, this.Map) { Height = height, Width = width };
                    while (Updating)
                    {
                    }
                    this.Enemies.Add(enemy);
                }
                else
                {
                    BaseEnemy enemy = new FlyingEnemy(1.3F, hitPoints, 100, this.Map) { Height = height, Width = width };
                    while (Updating)
                    {
                    }
                    this.Enemies.Add(enemy);
                }

                Thread.Sleep(milis);
            }
        }

        public void Update(bool force)
        {
            if (!Running && !force)
                return;

            if (NextLevelCounterSeconds == 0)
            {
                SendNextLevel();
            }

            foreach (var item in Levels)
            {
                if (item.Active && item.CanSpawn())
                {
                    Enemies.Add(item.SpawnOne(Map));
                }
            }

            // remove destroyed enemies and calc money and points
            List<BaseEnemy> remove = Enemies.Where(x => x.HitPoints < 1 || x.EndReached).ToList();
            foreach (var item in remove)
            {
                if (item.EndReached)
                    Life--;
                if (item.HitPoints < 1)
                {
                    Money = Money + item.Money;
                    Points = Points + item.Points;
                }

                Enemies.Remove(item);
            }

            foreach (var item in Enemies)
            {
                item.Move(Map);
            }


            // add new bought towes
            foreach (var item in TowersToAdd)
            {
                item.EnableFire(true);
                Towers.Add(item);
            }
            TowersToAdd.Clear();

            // tower firing
            foreach (var item in Towers)
            {
                if (item.Dummy)
                {
                    item.CanBuy = item.CanBuyIt(Money);
                }
                else
                {
                    if (item.CanFire())
                    {
                        List<BaseBullet> bb = item.TryFire(Enemies);

                        Bullets.AddRange(bb);
                    }
                }
            }

            // bullets removal and adding
            List<BaseBullet> removeB = Bullets.Where(x => x.Destroy).ToList();
            foreach (var item in removeB)
            {
                Bullets.Remove(item);
            }

            foreach (var item in Bullets)
            {
                item.Move(Enemies);
            }
        }

        public void Draw(Graphics gfx, Pen pen)
        {
            foreach (var item in Enemies)
            {
                item.DrawSelf(gfx, pen);
            }

            foreach (var item in Towers)
            {
                item.DrawSelf(gfx, pen);
            }
            
            foreach (var item in Bullets)
            {
                item.DrawSelf(gfx, pen);
            }

            foreach (var item in Map.Roads)
            {
                item.DrawSelf(gfx, pen);
            }

            foreach (var item in Map.Junctions)
            {
                item.DrawSelf(gfx, pen);
            }

            if (SelectedTower != null)
                SelectedTower.DrawSelf(gfx, pen);

        }

    }
}
