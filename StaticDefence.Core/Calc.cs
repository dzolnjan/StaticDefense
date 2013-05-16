using System;
using System.Drawing;

namespace StaticDefence.Core
{
    public static class Calc
    {
        public static double GetAngle(PointF c, PointF t)
        {
            return Math.Atan2(t.Y - c.Y, t.X - c.X);
        }


        public static PointF GetPoint(PointF center, double angle, double distance)
        {
            // Angles in java are measured clockwise from 3 o'clock.
            PointF p = new PointF();
            p.X = (float)(center.X + distance * Math.Cos(angle));
            p.Y = (float)(center.Y + distance * Math.Sin(angle));
            return p;
        }

        public static PointF GetPoint(PointF center, int angle, double distance)
        {
            double theta = DegreeToRadian(angle);
            return GetPoint(center, theta, distance);
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double Distance(PointF start, PointF target)
        {
            return Math.Sqrt(Math.Pow(target.X - start.X, 2) + Math.Pow(target.Y - start.Y, 2));
        }
        
        public static PointF RotatePoint(PointF center, PointF point, double rotationAngle)
        {
            PointF p = new PointF();
            p.X = (float)(Math.Cos(rotationAngle) * (point.X - center.X) - Math.Sin(rotationAngle) * (point.Y - center.Y) + center.X);
            p.Y = (float)(Math.Sin(rotationAngle) * (point.X - center.X) + Math.Cos(rotationAngle) * (point.Y - center.Y) + center.Y);

            return p;
        }

        public static bool TimePassed(int delay, DateTime last)
        {
            return (DateTime.Now - last).TotalMilliseconds > delay;
        }
    }
}
