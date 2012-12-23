using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{
    public class SonicBullet : BaseBullet
    {
        public const int DamageDefault = 2;

        protected List<BaseEnemy> HitedTargets;
        List<PointF> pointsPassed;
        public float Range { get; set; }

        public SonicBullet(PointF start, PointF target)
            : base(start, target)
        {
            Width = 20;
            Height = 20;
            Speed = 20;
            Damage = DamageDefault;
            MoveDelayMilis = 10;

            pointsPassed = new List<PointF>();
            HitedTargets = new List<BaseEnemy>();
        }

        public override void Move(List<BaseEnemy> enemies)
        {
            if (Destroy)
                return;

            if ((DateTime.Now - _lastMoveTime).TotalMilliseconds < MoveDelayMilis)
                return;

            _lastMoveTime = DateTime.Now;

            _passedDistance = _passedDistance + Speed;
            if (_passedDistance > Range)
            {
                Destroy = true;
            }
            else
            {
                Center = Calc.GetPoint(Center, Angle, Speed);
                List<BaseEnemy> found = FindEnemiesAtPosition(enemies);
                foreach (var item in HitedTargets)
                {
                    found.Remove(item);
                }
                HitTargets(found);
                HitedTargets.AddRange(found);
                pointsPassed.Add(Center);

            }
        }

        public override void HitTargets(List<BaseEnemy> enemies)
        {
            foreach (var item in enemies)
            {
                item.Damage(Damage);
            }
        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            int x = pointsPassed.Count;
            foreach (var item in pointsPassed)
            {
                PointF a = new PointF(item.X + Width / 2, item.Y - Width / 2 / x);
                PointF b = new PointF(item.X + Width / 2, item.Y + Width / 2 / x);
                x--;
                gfx.DrawLines(pen, new PointF[2] { Calc.RotatePoint(item, a, Angle), Calc.RotatePoint(item, b, Angle) });
            }

        }
    }
}
