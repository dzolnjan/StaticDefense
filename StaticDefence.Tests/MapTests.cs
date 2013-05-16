using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticDefence.Core;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StaticDefence.Tests
{
    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void Map_End_Road_Position_Is_Equal_To_Sum_Of_Added_Roads()
        {
            //arrange
            Map map = new Map() { RoadThickness = 20, Start = new PointF(0, 0)};

            //act
            map.AddRoad(100, Directions.Right);
            map.AddRoad(50, Directions.Down);

            //assert
            var endX = 100 + 20 / 2;
            var endY = 50 + 20;
            Assert.AreEqual(new PointF(endX, endY), map.End, "Road end position is not valid.");
        }

        [TestMethod]
        public void Road_With_Only_Two_Directions_Has_One_Junction()
        {
            //arrange
            Map map = new Map();

            //act
            map.AddRoad(100, Directions.Right);
            map.AddRoad(50, Directions.Down);

            //assert
            Assert.AreEqual(1, map.Junctions.Count, "Map road has invalid number od junctions.");
        }

        [TestMethod]
        public void New_Enemy_Added_To_Map_Gets_Positioned_On_Road_Start()
        {
            //arrange
            var start = new PointF(10, 20);
            Map map = new Map() { Start = start };
            map.AddRoad(100, Directions.Right);
            BaseEnemy enemy = new GroundEnemy(1.3F, 1, 1, null);

            //act
            map.PositionEnemyForStart(enemy);

            //assert
            Assert.AreEqual(new PointF(10.1f, 20), enemy.Center, "Failed to position new ground enemy to map road start.");
        }
    }
}
