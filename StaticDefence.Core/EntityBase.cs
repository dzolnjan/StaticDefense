using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StaticDefence.Core
{
    [Serializable]
    public abstract class EntityBase
    {
        public PointF Center { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double Angle { get; set; }

        public abstract void DrawSelf(Graphics gfx, Pen pen);

        public static List<BaseEnemy> FilterTargets(List<BaseEnemy> enemies, TargetTypes TargetType)
        {
            if (TargetType == TargetTypes.Ground)
                enemies = enemies.Where(x => x.GetType() == typeof(GroundEnemy)).ToList();
            if (TargetType == TargetTypes.Flying)
                enemies = enemies.Where(x => x.GetType() == typeof(FlyingEnemy)).ToList();

            return enemies;

        }
    }
}
