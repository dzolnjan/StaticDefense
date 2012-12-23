using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{


    public interface ITower
    {
        BaseBullet Fire(BaseEnemy enemy);
        BaseEnemy FindTarget(List<BaseEnemy> enemies);
    }

    [Serializable]
    public abstract class BaseTower : EntityBase, ITower
    {
        public int Range { get; set; }
        public decimal Price { get; set; }
        public bool CanBuy { get; set; }
        public bool Active { get; set; }
        public bool Placed { get; set; }
        public bool Dummy { get; set; }
        public bool InvalidPosisiton { get; set; }
        public int FireDelayMilis { get; set; }
        protected DateTime LastFiredMilis { get; set; }
        public TargetTypes TargetType { get; set; }



        public BaseTower()
        {
            LastFiredMilis = DateTime.Now;
            Angle = Calc.DegreeToRadian(0);
            Placed = false;
            Active = false;
        }

        public abstract BaseBullet CreateBullet(PointF start, PointF target);

        public bool CanFire()
        {
            return Active && Placed && !Dummy && Calc.TimePassed(FireDelayMilis, LastFiredMilis);
        }

        public void EnableFire(bool enable)
        {
            Active = Placed = enable;
            Dummy = !enable;
        }

        public virtual BaseBullet Fire(BaseEnemy enemy)
        {
            if (enemy != null)
            {
                //double milis = (DateTime.Now - LastFiredMilis).TotalMilliseconds;
                //if (milis >= FireDelayMilis)
                //{
                    LastFiredMilis = DateTime.Now;
                    BaseBullet bullet = CreateBullet(Center, enemy.Center);
                    //SimpleBullet bullet = new SimpleBullet(Center, enemy.Center);
                    Angle = bullet.Angle;
                    return bullet;

                //}
            }

            return null;
        }

        public bool IsOverlapingRoads(Map map)
        {
            foreach (var item in map.Roads)
            {
                if (item.IsInside(new PointF(Center.X - Width, Center.Y)))
                    return true;
                if (item.IsInside(new PointF(Center.X + Width, Center.Y)))
                    return true;
                if (item.IsInside(new PointF(Center.X, Center.Y - Height)))
                    return true;
                if (item.IsInside(new PointF(Center.X, Center.Y + Height)))
                    return true;
            }

            foreach (var item in map.Junctions)
            {
                if (item.IsInside(new PointF(Center.X - Width, Center.Y)))
                    return true;
                if (item.IsInside(new PointF(Center.X + Width, Center.Y)))
                    return true;
                if (item.IsInside(new PointF(Center.X, Center.Y - Height)))
                    return true;
                if (item.IsInside(new PointF(Center.X, Center.Y + Height)))
                    return true;
            }

            return false;
        }


        public static bool IsOverlapingRoads(BaseTower tower, Map map)
        {
            foreach (var item in map.Roads)
            {
                if (item.IsInside(new PointF(tower.Center.X - tower.Width / 2, tower.Center.Y)))
                    return true;
                if (item.IsInside(new PointF(tower.Center.X, tower.Center.Y - tower.Height / 2)))
                    return true;
            }

            return false;
        }

        public virtual List<BaseBullet> TryFire(List<BaseEnemy> enemies)
        {
            List<BaseBullet> bullets = new List<BaseBullet>();
            if (CanFire())
            {
                //BaseEnemy enemy = FindTarget(enemies);
                //if (enemy != null)
                //{
                //    bullets.Add(Fire(enemy));
                //    //return bullet;
                //}
                BaseBullet bullet = TryFireOne(enemies);
                if (bullet != null)
                    bullets.Add(bullet);
            }

            return bullets;

        }
        public virtual BaseBullet TryFireOne(List<BaseEnemy> enemies)
        {
            BaseBullet bullet = null;
            BaseEnemy enemy = FindTarget(enemies);
            if (enemy != null)
            {
                bullet = Fire(enemy);
            }
            return bullet;
        }

        public BaseEnemy FindTarget(List<BaseEnemy> enemies)
        {
            if (!Active)
                return null;

            List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
            foreach (var item in enemies)
            {
                double dist = Calc.Distance(Center, item.Center);

                if (Range >= dist)
                    enemiesInRange.Add(item);
            }

            enemiesInRange = EntityBase.FilterTargets(enemiesInRange, TargetType);

            if (enemiesInRange.Count > 0)
                return enemiesInRange.First();

            return null;
        }


        public List<BaseEnemy> FindTargets(List<BaseEnemy> enemies)
        {
            if (!Active)
                return null;

            List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
            foreach (var item in enemies)
            {
                double dist = Calc.Distance(Center, item.Center);

                if (Range >= dist)
                    enemiesInRange.Add(item);
            }

            enemiesInRange = EntityBase.FilterTargets(enemiesInRange, TargetType);

            return enemiesInRange;
        }


        public bool CanBuyIt(decimal money)
        {
            return money - Price >= 0;
        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {


            if (!CanBuy && Dummy)
            {
                PointF a = new PointF(Center.X - (Width / 2), Center.Y - Height / 2);
                PointF b = new PointF(Center.X + Width / 2, Center.Y - Height / 2);
                PointF c = new PointF(Center.X + Width / 2, Center.Y + Height / 2);
                PointF d = new PointF(Center.X - Width / 2, Center.Y + Height / 2);

                gfx.FillPolygon(Brushes.Yellow, new PointF[5] { a, b, c, d, a });


            }

            if (InvalidPosisiton)
            {
                PointF a = new PointF(Center.X - (Width / 2), Center.Y - Height / 2);
                PointF b = new PointF(Center.X + Width / 2, Center.Y - Height / 2);
                PointF c = new PointF(Center.X + Width / 2, Center.Y + Height / 2);
                PointF d = new PointF(Center.X - Width / 2, Center.Y + Height / 2);

                gfx.FillPolygon(Brushes.Red, new PointF[5] { a, b, c, d, a });
            }

            if (Dummy)
            {
                gfx.DrawString(Price.ToString(), new Font("Arial", 5), Brushes.Black, Center.X - (Width / 2), Center.Y + Height / 2 + 7);
                //gfx.DrawString(Price.ToString(), new Font("Arial", 5), Brushes.Black, Center.X - (Width / 2), Center.Y - Height / 2 - 7);

            }
        }


    }
}
