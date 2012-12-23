using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{

    public interface IEnemy
    {
        bool Move(Map map);
        void SetSlow(int slowPercent, int slowDuration);
    }

    public abstract class BaseEnemy : EntityBase, IEnemy
    {
        public Map Map { get; set; }

        public int Points { get; set; }
        public decimal Money { get; set; }
        public bool EndReached { get; set; }

        public PointF Start { get; set; }
        public PointF End { get; set; }

        public bool Destroy { get; set; }
        public int HitPoints { get; set; }
        public float Speed { get; set; }
        protected float _designatedSpeed { get; set; }

        protected bool Shooted;
        public double MoveDelayMilis { get; set; }
        protected DateTime _lastMoveTime { get; set; }
        public int MoveCount { get; set; }

        public int SlowPercent { get; set; }
        public int SlowDuration { get; set; }
        protected bool _slowed;

        public BaseEnemy(float speed, int hitPoints, double moveDelayMilis, Map map)
        {
            Speed = speed;
            HitPoints = hitPoints;
            MoveDelayMilis = moveDelayMilis;
            MoveDelayMilis = 0;
            _lastMoveTime = DateTime.Now;
            //MoveDelayMilis = 20;

            Shooted = false;
            MoveCount = 0;

            SlowDuration = 0;
            SlowPercent = 0;
            _slowed = false;

            _designatedSpeed = Speed;
            Destroy = false;

            Center = map.Start;

            EndReached = false;

        }


        public void PositionEnemyForStart(Map map)
        {
            Road first = map.Roads.First();
            if (first.Direction == Directions.Right)
                Center = new PointF(map.Start.X + 0.1F, map.Start.Y + map.Thickness / 2);
            else if (first.Direction == Directions.Left)
                Center = new PointF(map.Start.X + 0.1F, map.Start.Y + map.Thickness / 2);
            else if (first.Direction == Directions.Down)
                Center = new PointF(map.Start.X + map.Thickness / 2, Start.Y + 0.1F);
            else if (first.Direction == Directions.Up)
                Center = new PointF(map.Start.X + map.Thickness / 2, Start.Y + 0.1F);


        }

        public override int GetHashCode()
        {
            return Center.X.GetHashCode() + Center.Y.GetHashCode() + Angle.GetHashCode();
        }

        public virtual bool Move(Map map)
        {

            if (HitPoints < 1)
                return false;

            if (EndReached)
                return false;

            if ((DateTime.Now - _lastMoveTime).TotalMilliseconds < MoveDelayMilis)
                return false;

            _lastMoveTime = DateTime.Now;

            Shooted = false;

            MoveCount++;

            if (!_slowed && SlowPercent > 0)
            {
                Speed = Speed - Speed * SlowPercent / 100;
                _slowed = true;
            }
            SlowDuration = SlowDuration > 0 ? SlowDuration - 1 : 0;
            SlowPercent = SlowDuration > 0 ? SlowPercent : 0;
            if (SlowPercent == 0)
            {
                _slowed = false;
                Speed = _designatedSpeed;
            }

            EndReached = map.EndReached(Center);

            return true;
        }

        public virtual void SetSlow(int slowPercent, int slowDuration)
        {
            if (slowPercent > SlowPercent)
            {
                SlowPercent = slowPercent;
                SlowDuration += slowDuration;
                //_slowed = true;
            }
        }

        public virtual void Damage(int damage)
        {
            Shooted = true;
            HitPoints = HitPoints - damage;
        }
    }
}
