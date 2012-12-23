using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{
    public class FlyingEnemy : BaseEnemy
    {
        public PointF Middle { get; set; }
        public bool MiddleReached { get; set; }

        public FlyingEnemy(float speed, int hitPoints, double moveDelayMilis, Map map)
            : base(speed, hitPoints, moveDelayMilis, map)
        {
            //Start = start;
            //End = end;

            //MoveDelayMilis = 100;
            Middle = map.FlyMiddles[new Random().Next(map.FlyMiddles.Count)];
            Center = map.Start;
            End = map.End;
            MiddleReached = false;

            //MoveCount = 0;
        }

        public override bool Move(Map map)
        {
            if (!base.Move(map)) return false;


            base.Move(map);

            if (MiddleReached)
            {
                Angle = Calc.GetAngle(Center, map.End);
                Center = Calc.GetPoint(Center, Angle, Speed);
            }
            else
            {
                MiddleReached = Calc.Distance(Center, Middle) <= Speed;

                Angle = Calc.GetAngle(Center, Middle);
                Center = Calc.GetPoint(Center, Angle, Speed);
            }


            return true;

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
                gfx.FillEllipse(Brushes.Red, a1.X, a1.Y, Width, Height);
            }
            else
            {
                gfx.DrawEllipse(pen, a1.X, a1.Y, Width, Height);
            }
            gfx.DrawString(HitPoints.ToString(), new Font("Arial", 5), Brushes.Black, Center.X - (Width / 2), Center.Y - Height / 2 - 10);
        }
    }
}
