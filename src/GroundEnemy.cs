using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{


    public class GroundEnemy : BaseEnemy
    {
        public int angleShift { get; set; }
        public GroundEnemy(float speed, int hitPoints, double moveDelayMilis, Map map)
            : base(speed, hitPoints, moveDelayMilis, map)
        {
            //MoveDelayMilis = 100;
            angleShift = 0;

            Road r = map.FindRoad(Center);
            Angle = r != null ? r.GetDefaultAngle() : 0;

            Center = map.Start;
            End = map.End;

        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            PointF a = new PointF(Center.X - (Width / 2), Center.Y - Height / 2);
            PointF b = new PointF(Center.X + Width / 2, Center.Y - Height / 2);
            PointF c = new PointF(Center.X + Width / 2, Center.Y + Height / 2);
            PointF d = new PointF(Center.X - Width / 2, Center.Y + Height / 2);

            PointF a1 = Calc.RotatePoint(Center, a, Angle);
            PointF b1 = Calc.RotatePoint(Center, b, Angle);
            PointF c1 = Calc.RotatePoint(Center, c, Angle);
            PointF d1 = Calc.RotatePoint(Center, d, Angle);

            if (Shooted)
            {
                gfx.FillPolygon(Brushes.Red, new PointF[5] { a1, b1, c1, d1, a1 });
            }
            else
            {
                gfx.DrawLines(pen, new PointF[5] { a1, b1, c1, d1, a1 });
            }

            gfx.DrawString(HitPoints.ToString(), new Font("Arial", 5), Brushes.Black, Center.X - (Width / 2), Center.Y - Height / 2 - 10);
        }


        public override bool Move(Map map)
        {
            if (!base.Move(map)) return false;

            if (MoveCount > 5)
            {
                Vector vector = map.MoveOnRoad(Center, Speed, Angle, 15);
                Angle = vector.Angle;
                Center = vector.Point;
                MoveCount = 0;
            }

            Vector vector1 = map.MoveOnRoad(Center, Speed, Angle, 0);
            Angle = vector1.Angle;
            Center = vector1.Point;

            return true;

        }


    }
}
