using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StaticDefence.Core
{
    public interface IBullet
    {
        void Move(List<BaseEnemy> enemies);
        List<BaseEnemy> FindEnemiesAtPosition(List<BaseEnemy> enemies);
        void HitTargets(List<BaseEnemy> enemies);
    }

    public abstract class BaseBullet : EntityBase, IBullet
    {
        public const int DamageDefault = 0;

        public PointF Target { get; set; }
        public PointF Start { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public bool Destroy { get; set; }
        protected float _distanceToTarget { get; set; }
        protected float _passedDistance { get; set; }

        public double MoveDelayMilis { get; set; }
        protected DateTime _lastMoveTime { get; set; }

        public TargetTypes TargetType { get; set; }

        public BaseBullet(PointF start, PointF target)
        {
            Start = start;
            Target = target;
            Center = start;
            Destroy = false;
            Angle = Calc.GetAngle(Center, Target);
            
            _distanceToTarget = (float)(Calc.Distance(start, target));
            _lastMoveTime = DateTime.Now;
        }

        public virtual void Move(List<BaseEnemy> enemies)
        {
            if (Destroy)
                return;

            if ((DateTime.Now - _lastMoveTime).TotalMilliseconds < MoveDelayMilis)
                return;

            _lastMoveTime = DateTime.Now;

            _passedDistance = _passedDistance + Speed;
            if (_passedDistance >= _distanceToTarget)
            {
                // find enemy at target position, if any
                // first center bullet to target possition
                Center = Target;

                List<BaseEnemy> found = FindEnemiesAtPosition(enemies);
                HitTargets(found);
                Destroy = true;
            }
            else
                Center = Calc.GetPoint(Center, Angle, Speed);
        }


        public virtual List<BaseEnemy> FindEnemiesAtPosition(List<BaseEnemy> enemies)
        {
            List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
            foreach (var item in enemies)
            {
                if (Math.Abs(this.Center.X - item.Center.X) < this.Width / 2 + item.Width / 2)
                    if (Math.Abs(this.Center.Y - item.Center.Y) < this.Height / 2 + item.Height / 2)
                    {
                        enemiesInRange.Add(item);
                    }
            }

            enemiesInRange = EntityBase.FilterTargets(enemiesInRange, TargetType);

            return enemiesInRange;
        }

        public virtual void HitTargets(List<BaseEnemy> enemies)
        {
            if (enemies.Count > 0)
                enemies.First().Damage(Damage);
            //foreach (var item in enemies)
            //{
            //    item.Damage(Damage);
            //}
        }


    }
}
