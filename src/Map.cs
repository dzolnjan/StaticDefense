using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StaticDefense
{
    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Map
    {

        public List<Road> Roads { get; set; }
        public List<Juntion> Junctions { get; set; }
        public List<PointF> FlyMiddles { get; set; }
        public PointF Start { get; set; }
        public PointF End { get; set; }
        public float Thickness { get; set; }

        public Directions EndDirection { get; set; }


        public Map()
        {
            Roads = new List<Road>();
            Junctions = new List<Juntion>();
            FlyMiddles = new List<PointF>();
        }

        public void PositionEnemyForStart(BaseEnemy enemy)
        {
            Road first = Roads.First();
            if (first.Direction == Directions.Right)
                enemy.Center = new PointF(Start.X + 0.1F, Start.Y + Thickness / 2); 
            else if (first.Direction == Directions.Left)
                enemy.Center = new PointF(Start.X + 0.1F, Start.Y + Thickness / 2);
            else if (first.Direction == Directions.Down)
                enemy.Center = new PointF(Start.X + Thickness / 2, Start.Y + 0.1F);
            else if (first.Direction == Directions.Up)
                enemy.Center = new PointF(Start.X + Thickness / 2, Start.Y + 0.1F); 


        }

        public void AddRoad(float distance, Directions direction)
        {
            if (Roads.Count == 0)
            {
                if (direction == Directions.Right)
                    Roads.Add(new Road(Directions.Right) { Left = Start.X, Top = Start.Y, Distance = distance, Thickness = Thickness });
                else if (direction == Directions.Left)
                    Roads.Add(new Road(Directions.Left) { Left = Start.X - distance, Top = Start.Y, Distance = distance, Thickness = Thickness });
                else if (direction == Directions.Down)
                    Roads.Add(new Road(Directions.Down) { Left = Start.X, Top = Start.Y, Distance = distance, Thickness = Thickness });
                else if (direction == Directions.Up)
                    Roads.Add(new Road(Directions.Up) { Left = Start.X, Top = Start.Y - distance, Distance = distance, Thickness = Thickness });
            }
            else
            {
                Road last = Roads.Last();
                AddJunction(direction);
                if (direction == Directions.Right)
                {
                    if (last.Direction == Directions.Down)
                        Roads.Add(new Road(direction) { Left = last.Left + Thickness, Top = last.Top + last.Distance, Distance = distance, Thickness = Thickness });
                    if (last.Direction == Directions.Up)
                        Roads.Add(new Road(direction) { Left = last.Left + Thickness, Top = last.Top - Thickness, Distance = distance, Thickness = Thickness });
                }
                if (direction == Directions.Left)
                {
                    if (last.Direction == Directions.Down)
                        Roads.Add(new Road(direction) { Left = last.Left - last.Distance, Top = last.Top + last.Distance, Distance = distance, Thickness = Thickness });
                    if (last.Direction == Directions.Up)
                        Roads.Add(new Road(direction) { Left = last.Left - last.Distance, Top = last.Top - Thickness, Distance = distance, Thickness = Thickness });
                }
                if (direction == Directions.Down)
                {
                    if (last.Direction == Directions.Right)
                        Roads.Add(new Road(direction) { Left = last.Left + last.Distance, Top = last.Top + Thickness, Distance = distance, Thickness = Thickness });
                    if (last.Direction == Directions.Left)
                        Roads.Add(new Road(direction) { Left = last.Left - Thickness, Top = last.Top + Thickness, Distance = distance, Thickness = Thickness });
                }
                if (direction == Directions.Up)
                {
                    if (last.Direction == Directions.Right)
                        Roads.Add(new Road(direction) { Left = last.Left + last.Distance, Top = last.Top - distance, Distance = distance, Thickness = Thickness });
                    if (last.Direction == Directions.Left)
                        Roads.Add(new Road(direction) { Left = last.Left - Thickness, Top = last.Top - distance, Distance = distance, Thickness = Thickness });
                }
            }

            EndDirection = direction;
            Road last1 = Roads.Last();
            if (last1.Direction == Directions.Right)
                End = new PointF(last1.Left + last1.Distance, last1.Top + Thickness/2);
            else if (last1.Direction == Directions.Left)
                End = new PointF(last1.Left, last1.Top + Thickness / 2);
            else if (last1.Direction == Directions.Down)
                End = new PointF(last1.Left + Thickness / 2, last1.Top + last1.Distance);
            else if (last1.Direction == Directions.Up)
                End = new PointF(last1.Left + Thickness / 2, last1.Top);


        }

        private void AddJunction(Directions direction)
        {
            Road last = Roads.Last();
            if (direction == Directions.Right)
            {
                if (last.Direction == Directions.Up)
                    Junctions.Add(new Juntion(320) { Left = last.Left, Top = last.Top - Thickness, Width = Thickness });
                else if (last.Direction == Directions.Down)
                    Junctions.Add(new Juntion(40) { Left = last.Left, Top = last.Top + last.Distance, Width = Thickness });
                //else
                //    throw "Cant add junction to " + direction.ToString() + "
            }
            else if (direction == Directions.Left)
            {
                if (last.Direction == Directions.Up)
                    Junctions.Add(new Juntion(220) { Left = last.Left, Top = last.Top - Thickness, Width = Thickness });
                else if (last.Direction == Directions.Down)
                    Junctions.Add(new Juntion(140) { Left = last.Left, Top = last.Top + last.Distance, Width = Thickness });
                //else
                //    throw "Cant add junction to " + direction.ToString() + "
            }
            else if (direction == Directions.Up)
            {
                if (last.Direction == Directions.Right)
                    Junctions.Add(new Juntion(310) { Left = last.Left + last.Distance, Top = last.Top, Width = Thickness });
                else if (last.Direction == Directions.Left)
                    Junctions.Add(new Juntion(140) { Left = last.Left + last.Distance, Top = last.Top, Width = Thickness });
                //else
                //    throw "Cant add junction to " + direction.ToString() + "
            }
            else if (direction == Directions.Down)
            {
                if (last.Direction == Directions.Right)
                    Junctions.Add(new Juntion(50) { Left = last.Left + last.Distance, Top = last.Top, Width = Thickness });
                else if (last.Direction == Directions.Left)
                    Junctions.Add(new Juntion(130) { Left = last.Left - Thickness, Top = last.Top, Width = Thickness });
                //else
                //    throw "Cant add junction to " + direction.ToString() + "
            }

        }

        public bool EndReached(PointF center)
        {
            if (EndDirection == Directions.Right)
                return center.X > End.X;
            else if (EndDirection == Directions.Left)
                return center.X < End.X;
            else if (EndDirection == Directions.Up)
                return center.Y < End.Y;
            else if (EndDirection == Directions.Down)
                return center.Y > End.Y;

            return true;
        }


        public Road FindRoad(PointF center)
        {
            Road road = null;
            foreach (var item in Roads)
            {
                if (item.IsInside(center))
                {
                    road = item;
                    break;
                }
            }
            return road;
        }

        public Vector MoveOnRoad(PointF center, float distance, double currentAngle, int angleShiftDegree)
        {
            Road road = null;
            foreach (var item in Roads)
            {
                if (item.IsInside(center))
                {
                    road = item;
                    break;
                }
            }
            if (road != null)
            {
                return road.Move(center, distance, currentAngle, angleShiftDegree);
            }
            else
            {
                Juntion juntion = null;
                foreach (var item in Junctions)
                {
                    if (center.X > item.Left && center.X < item.Left + item.Width && center.Y > item.Top && center.Y < item.Top + item.Width)
                    {
                        juntion = item;
                        break;
                    }
                }
                if (juntion != null)
                {
                    return juntion.Move(center, distance);
                }
            }

            return new Vector() { Point = center, Angle = currentAngle };
        }

        public Vector MoveOnAir(PointF center, float distance, double currentAngle, PointF currentMiddle, bool changeMiddle)
        {
            PointF middle = changeMiddle ? FlyMiddles[new Random().Next(FlyMiddles.Count)] : currentMiddle;

            double angle = Calc.GetAngle(center, middle);

            if (Calc.Distance(center, middle) <= distance)
                angle = Calc.GetAngle(center, End);

            Vector vector = new Vector() { Angle = angle, Point = Calc.GetPoint(center, angle, distance), Middle = middle };
            return vector;
        }
    }

    public class Road
    {
        public float Top { get; set; }
        public float Left { get; set; }
        public float Thickness { get; set; }
        public float Distance { get; set; }

        Random rnd = new Random();

        public Directions Direction { get; set; }
        //public int Angle { get; set; }
        protected double _angle { get; set; }

        public Road(Directions direction)
        {
            Direction = direction;

            _angle = GetDefaultAngle();

        }

        public double GetDefaultAngle()
        {
            double angle = 0;
            if (Direction == Directions.Up)
                angle = Calc.DegreeToRadian(270);
            else if (Direction == Directions.Down)
                angle = Calc.DegreeToRadian(90);
            else if (Direction == Directions.Left)
                angle = Calc.DegreeToRadian(180);
            else
                angle = Calc.DegreeToRadian(0);

            return angle;

        }

        public Vector Move(PointF center, float distance, double currentAngle, int angleShiftDegree)
        {
            if (Direction == Directions.Up && center.Y < Top - distance - 0.1)
                currentAngle = Calc.DegreeToRadian(270);
            else if (Direction == Directions.Down && center.Y < Top + distance + 0.1)
                currentAngle = Calc.DegreeToRadian(90);
            else if (Direction == Directions.Left && center.X < Top - distance - 0.1)
                currentAngle = Calc.DegreeToRadian(180);
            else if (Direction == Directions.Right && center.X < Left + distance + 0.1)
                currentAngle = Calc.DegreeToRadian(0);


            double angleShift = Calc.DegreeToRadian(rnd.Next(angleShiftDegree));
            angleShift = rnd.Next(angleShiftDegree) % 2 == 0 ? angleShift : -angleShift;

            double moveAngle = currentAngle + angleShift;
            PointF temp = Calc.GetPoint(center, moveAngle, distance);

            if (!IsInside(temp))
            {
                moveAngle = moveAngle - angleShift;
                temp = Calc.GetPoint(center, moveAngle, distance);
            }

            if (!IsInside(temp))
            {
                moveAngle =_angle;
                temp = Calc.GetPoint(center, moveAngle, distance);
            }

            return new Vector() { Angle = moveAngle, Point = temp };

        }

        public bool IsInside(PointF temp)
        {
            if ((Direction == Directions.Up || Direction == Directions.Down) && temp.X > Left && temp.X < Left + Thickness && temp.Y > Top && temp.Y < Top + Distance)
                return true;
            else if ((Direction == Directions.Left || Direction == Directions.Right) && temp.X > Left && temp.X < Left + Distance && temp.Y > Top && temp.Y < Top + Thickness)
                return true;

            return false;
        }



        public void DrawSelf(Graphics gfx, Pen pen)
        {
            if (Direction == Directions.Left || Direction == Directions.Right)
            {
                gfx.DrawLine(pen, Left, Top, Left + Distance, Top);
                gfx.DrawLine(pen, Left, Top + Thickness, Left + Distance, Top + Thickness);
            }
            else
            {
                gfx.DrawLine(pen, Left, Top, Left, Top + Distance);
                gfx.DrawLine(pen, Left + Thickness, Top, Left + Thickness, Top + Distance);
            }
        }


    }

    public class Juntion
    {
        public float Top { get; set; }
        public float Left { get; set; }
        public float Width { get; set; }

        Random rnd = new Random();

        public Directions InDirection { get; set; }
        public Directions OutDirection { get; set; }
        public int Angle { get; set; }
        protected double _angle { get; set; }

        public Juntion(int angle)
        {
            //InDirection = inDir;
            //OutDirection = outDir;
            Angle = angle;
            _angle = Calc.DegreeToRadian(angle);
        }

        public Vector Move(PointF center, float distance)
        {
            PointF temp = Calc.GetPoint(center, _angle, distance);

            return new Vector() { Angle = _angle, Point = temp };
        }


        public bool IsInside(PointF temp)
        {
            if (temp.X > Left && temp.X < Left + Width && temp.Y > Top && temp.Y < Top + Width)
                return true;

            return false;
        }


        public void DrawSelf(Graphics gfx, Pen pen)
        {
            gfx.DrawRectangle(new Pen(Brushes.BlueViolet), Left, Top, Width, Width);
        }


    }
}
