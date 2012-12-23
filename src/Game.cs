using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace StaticDefense
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

        public Graphics Gfx { get; set; }
        public Pen Pen { get; set; }

        protected bool Updating { get; set; }

        public Game(int nextWaveCounterSec)
        {
            _designatedNextLevelCounterSeconds = nextWaveCounterSec;
            NextLevelCounterSeconds = nextWaveCounterSec;

            Clock.Interval = 1000;
            Clock.Start();
            Clock.Tick += new EventHandler(Timer_Tick);

            Map = new Map();
            //Roads = new List<Road>();
            //Junctions = new List<Juntion>();

            Enemies = new List<BaseEnemy>();
            Bullets = new List<BaseBullet>();
            Towers = new List<BaseTower>();
            TowersToAdd = new List<BaseTower>();
            Levels = new List<Level>();

            //for (int i = 0; i < 100; i++)
            //{
            //    Towers.Add(new SimpleTower() { Placed = false, Active = false });
            //}
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
                //if (Updating) Thread.Sleep(5);
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
                //Clock.Stop();
                //NextLevelCounterSeconds = 10;
            }

            foreach (var item in Levels)
            {
                if (item.Active && item.CanSpawn())
                {
                    Enemies.Add(item.SpawnOne(Map));
                }
            }
            //Updating = true;

            //while (!AddingEnemy
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

            //for (int i = 0; i < Enemies.Count; i++)
            //{
            //    if (Enemies[i] != null)
            //        Enemies[i].Move(Map);
            //}

            foreach (var item in Enemies)
            {
                item.Move(Map);
            }
            //Updating = false;

            foreach (var item in TowersToAdd)
            {
                item.EnableFire(true);
                Towers.Add(item);
            }
            TowersToAdd.Clear();
            //foreach (var item in Towers)
            //{
            //    if (item.Dummy)
            //    {
            //        item.CanBuy = item.CanBuyIt(Money);
            //    }
            //    else if (item.CanFire())
            //    {

            //            BaseBullet bullet = item.TryFireOne(Enemies);
            //            if (bullet != null) Bullets.Add(bullet);
            //    }

            //}
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
                        //if (bb.Count > 0)
                        //    Debugger.Break();
                        Bullets.AddRange(bb);
                    }
                }
            }

            List<BaseBullet> removeB = Bullets.Where(x => x.Destroy).ToList();
            foreach (var item in removeB)
            {
                Bullets.Remove(item);
            }
            foreach (var item in Bullets)
            {
                item.Move(Enemies);
            }


            //foreach (var item in DummyTowers)
            //{
            //    item.CanBuy = item.CanBuyIt(Money);
            //}

        }

        //public BaseTower FindDisabled()
        //{
        //    Towers.Where(x => x.
        //}

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

            //foreach (var item in DummyTowers)
            //{
            //    item.DrawSelf(gfx, pen);
            //}

        }

    }
}
