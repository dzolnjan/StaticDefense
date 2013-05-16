using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace StaticDefence.Core
{
    [Serializable]
    public class TeslaTower : BaseTower
    {
        public int NumberOfTargets { get; set; }

        public TeslaTower()
            : base()
        {
            Height = 10;
            Width = 10;
            FireDelayMilis = 1000;
            NumberOfTargets = 3;
            TargetType = TargetTypes.All;
        }

        public override BaseBullet CreateBullet(PointF start, PointF target)
        {
            return new TeslaBullet(start, target) { TargetType = TargetTypes.All , Speed = Range};
        }

        public override List<BaseBullet> TryFire(List<BaseEnemy> enemies)
        {
            List<BaseBullet> bullets = new List<BaseBullet>();
            if (CanFire())
            {
                List<BaseEnemy> enemiesTemp = FindTargets(enemies);
                foreach (var item in enemiesTemp.Take(NumberOfTargets).ToList())
                {
                    BaseBullet bullet = Fire(item);
                        if (bullet != null)
                            bullets.Add(bullet);
                }
            }

            return bullets;
        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            base.DrawSelf(gfx, pen);

            gfx.FillEllipse(Brushes.Orange, Center.X - Width / 2, Center.Y - Height / 2, Width, Height);

            if (!Dummy)
            {

                Pen penn = new Pen(Color.Gainsboro, 1);
                gfx.DrawEllipse(penn, Center.X - Range, Center.Y - Range, Range * 2, Range * 2);
            }
            if (Dummy)
            {
                gfx.DrawString((1000/FireDelayMilis * TeslaBullet.DamageDefault * NumberOfTargets).ToString(), new Font("Arial", 7), Brushes.Black, Center.X - (Width / 2), Center.Y - 15);
            }

        }
    }
}
