using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CuriousityGame.CuriousityGame;
using Microsoft.Xna.Framework;
using CuriousityGame;

namespace CuriousityGameTest
{
    /// <summary>
    /// This Testcase are to exploit all the protocols explained in the task sheet
    /// 
    /// The initial state of the rover is at point 0,0 facing north
    /// </summary>
    [TestClass]
    public class RoverControllerTest
    {
        private Game game;
        private RoverController controller;
        private MarsRover rover;

        [TestInitialize]
        public void init()
        {
            game = new Game();
            rover = new MarsRover(new Game(), new Point(0, 0), Orientation.NORTH);
            controller = rover.Controller;
            game.Components.Add(rover);
        }

        private void runUpdates()
        {
            // Fast update
            while (!rover.Idle)
                rover.Update(null);

            // Real simulation of update
            //while (!rover.Idle)
            //    game.RunOneFrame();
        }

        [TestMethod]
        public void TestGetRoverStartPosition()
        {
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected Start position 0");
            Assert.AreEqual(0, point.Y, "Expected Start position 0");
        }

        [TestMethod]
        public void TestRoverMoveForward()
        {
            controller.MoveRover("f");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected position 0");
            Assert.AreEqual(1, point.Y, "Expected position 1");
        }

        [TestMethod]
        public void TestRover2MoveForward()
        {
            controller.MoveRover("ff");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected position 0");
            Assert.AreEqual(2, point.Y, "Expected position 2");
        }

        [TestMethod]
        public void TestRoverGivenExample()
        {
            controller.MoveRover("ffrff");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(2, point.X, "Expected position 2");
            Assert.AreEqual(2, point.Y, "Expected position 2");
        }

        [TestMethod]
        public void TestRoverBlocked()
        {
            controller.MoveRover("ffff");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected position 0");
            Assert.AreEqual(3, point.Y, "Expected position 3");
        }

        [TestMethod]
        public void TestMapWrapping()
        {
            controller.MoveRover("lf");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(9, point.X, "Expected position 9");
            Assert.AreEqual(0, point.Y, "Expected position 0");
        }

        [TestMethod]
        public void TestWrapWorldBlockedBackwards()
        {
            controller.MoveRover("ffffrb");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected position 0");
            Assert.AreEqual(3, point.Y, "Expected position 3");
        }

        [TestMethod]
        public void TestWrapWorldBlockedForward()
        {
            controller.MoveRover("fffflf");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(0, point.X, "Expected position 0");
            Assert.AreEqual(3, point.Y, "Expected position 3");
        }

        [TestMethod]
        public void TestBlockOnXAxis()
        {
            controller.MoveRover("rffff");
            runUpdates();
            Point point = controller.GetRoverPosition();
            Assert.AreEqual(3, point.X, "Expected position 3");
            Assert.AreEqual(0, point.Y, "Expected position 0");
        }
    }
}
