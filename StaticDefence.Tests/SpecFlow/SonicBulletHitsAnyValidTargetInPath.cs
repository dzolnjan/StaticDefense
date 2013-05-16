using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticDefence.Core;
using TechTalk.SpecFlow;

namespace StaticDefence.Tests
{
    [Binding]
    public class SonicBulletHitsAnyValidTargetInPath
    {
        private BaseTower tower;
        private BaseBullet bullet;
        private BaseEnemy enemy1, enemy2;
        private int range = 100;

        [Given(@"Sonic Tower fires a bullet to target One")]
        public void GivenSonicTowerFiresABulletToTargetOne()
        {
            tower = new SonicTower() { Center = new PointF(0, 0), FireDelayMilis = 1000, Placed = true, Active = true };
            enemy1 = new GroundEnemy(1.3F, 1, 1, null) { Center = new PointF(0, range), Height = 10, Width = 10};
            bullet = tower.Fire(enemy1);
        }
        
        [Given(@"There is target Two in bullets path to target One")]
        public void GivenThereIsTargetTwoInBulletsPathToTargetOne()
        {
            enemy2 = new GroundEnemy(1.3F, 1, 1, null) { Center = new PointF(0, range / 2), Height = 10, Width = 10 };
        }
        
        [When(@"Bullet reaches target Two or end of flight path range")]
        public void WhenBulletReachesTargetTwoOrEndOfFlightPathRange()
        {
            var enemiesInPlay = new List<BaseEnemy>() { enemy1, enemy2 };
            while (!bullet.Destroy)
            {
                bullet.Move(enemiesInPlay);
            }
        }
        
        [Then(@"Bullet should hit target Two")]
        public void ThenBulletShouldHitTargetTwo()
        {
            Assert.IsTrue(enemy2.Shooted, "Sonic bullet failed to hit target two.");
        }
    }
}
