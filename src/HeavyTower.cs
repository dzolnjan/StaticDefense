using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{
    [Serializable]
    public class HeavyTower : BaseTower
    {
        public int SlowPercent { get; set; }
        public int SlowDuration { get; set; }

        public HeavyTower()
            : base()
        {
            Height = 10;
            Width = 10;
            FireDelayMilis = 500;
            
            
        }

        public override BaseBullet CreateBullet(PointF start, PointF target)
        {
            return new HeavyBullet(start, target) { SlowDuration = SlowDuration, SlowPercent = SlowPercent, TargetType = TargetTypes.Ground };
        }



        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            base.DrawSelf(gfx, pen);

            PointF a = new PointF(Center.X + Width / 2, Center.Y);
            PointF b = new PointF(Center.X - Width / 2, Center.Y + Height / 2);
            PointF c = new PointF(Center.X - Width / 2, Center.Y - Height / 2);

            PointF a1 = Calc.RotatePoint(Center, a, Angle);
            PointF b1 = Calc.RotatePoint(Center, b, Angle);
            PointF c1 = Calc.RotatePoint(Center, c, Angle);

            Brush brush = Brushes.Gray;
            gfx.FillPolygon(brush, new PointF[4] { a1, b1, c1, a1 });
            gfx.DrawPolygon(pen, new PointF[4] { a1, b1, c1, a1 });

            if (!Dummy)
            {

                Pen penn = new Pen(Color.Gray, 1);
                gfx.DrawEllipse(penn, Center.X - Range, Center.Y - Range, Range * 2, Range * 2);
            }
            if (Dummy)
            {
                gfx.DrawString((1000 / FireDelayMilis * TeslaBullet.DamageDefault).ToString(), new Font("Arial", 7), Brushes.Black, Center.X - (Width / 2), Center.Y - 15);
            }
        }
    }
}
