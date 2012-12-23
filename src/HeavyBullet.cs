using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{
    public class HeavyBullet : BaseBullet
    {
        public const int DamageDefault = 2;

        public int SlowPercent { get; set; }
        public int SlowDuration { get; set; }

        public HeavyBullet(PointF start, PointF target)
            : base(start, target)
        {
            Height = 50;
            Width = 50;
            Damage = DamageDefault;
            Speed = 20;
            MoveDelayMilis = 0;
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
            if (Center == Target)
            {
                gfx.FillEllipse(Brushes.Gainsboro, Center.X - (Width / 2), Center.Y - Height / 2, Width, Height);
            }
            else
            {
                //PointF a = new PointF(Center.X + 3, Center.Y);
                //PointF b = new PointF(Center.X + 3, Center.Y + 3);
                //PointF c = new PointF(Center.X - 3, Center.Y - 3);

                gfx.FillEllipse(Brushes.Black, Center.X - 2, Center.Y - 2, 4, 4);
            }
        }

    }
}
