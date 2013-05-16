using System.Drawing;

namespace StaticDefence.Core
{
    public class SimpleBullet : BaseBullet
    {
        public const int DamageDefault = 1;

        public SimpleBullet(PointF start, PointF target) : base(start, target)
        {
            Height = 2;
            Width = 2;
            Damage = DamageDefault;
            Speed = 20;
            MoveDelayMilis = 0;
        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            //PointF a = new PointF(Center.X + Width / 2, Center.Y);
            //PointF b = new PointF(Center.X + Width / 2, Center.Y + Width / 2);
            //PointF c = new PointF(Center.X - Width / 2, Center.Y - Width / 2);

            gfx.FillEllipse(Brushes.Black, Center.X - 1F, Center.Y - 1F, 2, 2);
            //gfx.DrawPolygon(pen, new PointF[3] { Calc.RotatePoint(Center, a, Angle), Calc.RotatePoint(Center, b, Angle), Calc.RotatePoint(Center, c, Angle) });
        }

    }
}
