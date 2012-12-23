using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
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
                //BaseBullet bullet = TryFireOne(enemies);
                //if (bullet != null)
                //    bullets.Add(bullet);
                //List<BaseEnemy> enemiesTemp = enemies;
                List<BaseEnemy> enemiesTemp = FindTargets(enemies);
                foreach (var item in enemiesTemp.Take(NumberOfTargets))
                {
                    BaseBullet bullet = Fire(item);
                        if (bullet != null)
                            bullets.Add(bullet);
                }
                //for (int i = 0; i < NumberOfTargets; i++)
                //{
                //    if (enemy != null)
                //    {
                //        BaseBullet bullet = Fire(enemy);
                //        if (bullet != null)
                //            bullets.Add(bullet);
                //        //firedAt.Add(enemy);
                //        //enemiesTemp.Remove(enemy);
                //    }

                //    enemy = FindTarget(enemiesTemp);
                //}
            }

            return bullets;
        }

        public override void DrawSelf(Graphics gfx, Pen pen)
        {
            base.DrawSelf(gfx, pen);

            //PointF a = new PointF(Center.X + Width / 2, Center.Y);
            //PointF b = new PointF(Center.X - Width / 2, Center.Y + Height / 2);
            //PointF c = new PointF(Center.X - Width / 2, Center.Y - Height / 2);

            //PointF a1 = Calc.RotatePoint(Center, a, Angle);
            //PointF b1 = Calc.RotatePoint(Center, b, Angle);
            //PointF c1 = Calc.RotatePoint(Center, c, Angle);

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
