using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticDefence.Core;
using System.Collections.Generic;
using System.Drawing;

namespace StaticDefence.Tests
{
    [TestClass]
    public class BulletTests
    {
        [TestMethod]
        public void Can_Shoot_Target()
        {
            //arrange
            var enemy = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, 0), HitPoints = 50 };
            var bullet = new SimpleBullet(new PointF(0, 0), new PointF(0, 0)) { Damage = 50 };

            //act
            bullet.Move(new List<BaseEnemy>() { enemy });

            //assert
            Assert.IsTrue(enemy.Shooted, "Bullet failed to shoot the target.");
        }

        [TestMethod]
        public void Can_Deal_Damage_To_Target()
        {
            //arrange
            var enemy = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, 0), HitPoints = 100 };
            var bullet = new SimpleBullet(new PointF(0, 0), new PointF(0, 0)) { Damage = 50 };

            //act
            bullet.Move(new List<BaseEnemy>() { enemy });

            //assert
            Assert.AreEqual(50, enemy.HitPoints, "Bullet failed to damage the target for exact damage amount.");
        }

        [TestMethod]
        public void Heavy_Bullet_Deals_Damage_To_Multiple_Targets_In_Blast_Area()
        {
            //arrange
            var enemy1 = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, 10), HitPoints = 100 };
            var enemy2 = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, 20), HitPoints = 100 };
            var enemy3 = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, 30), HitPoints = 100 };
            var bullet = new HeavyBullet(new PointF(0, 0), new PointF(0, 0)) { Damage = 50, Width = 50, Height = 50 };

            //act
            bullet.Move(new List<BaseEnemy>() { enemy1, enemy2, enemy3 });

            //assert
            Assert.AreEqual(150, enemy1.HitPoints + enemy1.HitPoints + enemy1.HitPoints, "Heavy bullet failed to damage all targets in area.");
        }
    }
}
