using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticDefence.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StaticDefence.Tests
{
    [TestClass]
    public class TowerTests
    {
        [TestMethod]
        public void Can_Not_Find_Target_If_Out_Of_Range()
        {
            //arrange
            var range = 100;
            var tower = new SimpleTower() { Center = new PointF(0, 0), Range = range, Active = true };
            var enemy = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range + 1) };

            //act
            var target = tower.FindTarget(new List<BaseEnemy>() { enemy });

            //assert
            Assert.IsNull(target, "Tower found a target that is out of its range.");
        }

        [TestMethod]
        public void Can_Find_Target_In_Range()
        {
            //arrange
            var range = 100;
            var tower = new SimpleTower() { Center = new PointF(0, 0), Range = range, Active = true };
            var enemy = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range) };

            //act
            var target = tower.FindTarget(new List<BaseEnemy>() { enemy });

            //assert
            Assert.IsNotNull(target, "Tower failed to find target that is within range.");
        }

        [TestMethod]
        public void Can_Not_Fire_Faster_Than_FileDelay()
        {
            //arrange
            var range = 100;
            var tower = new SimpleTower() { FireDelayMilis = 1000, Placed = true, Active = true };
            var enemy = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range + 1) };

            //act
            var bullet1 = tower.Fire(enemy);
            var canFire = tower.CanFire();

            //assert
            Assert.IsFalse(canFire, "Tower fires faster that its fire rate.");
        }

        [TestMethod]
        public void Can_Only_Fire_On_Valid_TargetType()
        {
            //arrange
            var range = 100;
            var tower = new SimpleTower() { FireDelayMilis = 1000, Range = 100, TargetType = TargetTypes.Ground, Active = true };
            var ground = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range) };
            var flying = new FlyingEnemy(1.3F, 100, 100, null) { Center = new PointF(100, range) };

            //act
            var targets = tower.FindTargets(new List<BaseEnemy>() { ground, flying });

            //assert
            Assert.AreEqual(ground, targets.First(), "Tower found flying target but can only fire on ground targets.");
        }

        [TestMethod]
        public void Telsa_Tower_Can_Fire_On_Multiple_Targets_In_Range_At_Once()
        {
            //arrange
            var range = 100;
            var tower = new TeslaTower() { FireDelayMilis = 1000, Range = 100, Placed = true, Active = true };
            var ground = new GroundEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range) };
            var flying = new FlyingEnemy(1.3F, 100, 100, null) { Center = new PointF(0, range) };

            //act
            var bullets = tower.TryFire(new List<BaseEnemy>() { ground, flying });

            //assert
            Assert.AreEqual(2, bullets.Count, "Telsa tower failed to fire on two valid targets in range at once.");
        }
    }
}
