using System;

namespace StaticDefence.Core
{
    public class Level
    {
        public int HitPoints { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SpawnDelayMilis { get; set; }
        public int Count { get; set; }
        public bool Ground { get; set; }
        public decimal Money { get; set; }
        public float Speed { get; set; }
        public int Points { get; set; }
        

        public bool Active { get; set; }

        public DateTime LastTimeSpawn { get; set; }

        public Level()
        {
            LastTimeSpawn = DateTime.Now;
        }

        public BaseEnemy SpawnOne(Map map)
        {
            LastTimeSpawn = DateTime.Now;
            Count--;

            BaseEnemy enemy = null;
            if (Ground)
                enemy = new GroundEnemy(Speed, HitPoints, 100, map) { Height = Height, Width = Width, Money = Money, Points = Points };
            else
                enemy = new FlyingEnemy(Speed, HitPoints, 100, map) { Height = Height, Width = Width, Money = Money, Points = Points };

            enemy.PositionEnemyForStart(map);

            return enemy;
        }

        public bool CanSpawn()
        {
            if (Count == 0) return false;
            return Calc.TimePassed(SpawnDelayMilis, LastTimeSpawn);
        }
    }
}
