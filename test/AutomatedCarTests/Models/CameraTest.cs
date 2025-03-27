using AutomatedCar.Models;
using Avalonia;
using Avalonia.Media;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.Models
{
    public class CameraTest
    {
        private Camera camera;
        private AutomatedCar.Models.AutomatedCar car;
        private List<WorldObject> worldObjects;

        public CameraTest()
        {
            car = new AutomatedCar.Models.AutomatedCar(0, 0, "car.png");
            car.Rotation = 0;
            worldObjects = new List<WorldObject>
                {
                    new WorldObject(-100, 100, "object1.png"),
                    new WorldObject(1000, 1000, "object2.png")
                };
            camera = new Camera(car, worldObjects);
        }

        [Fact]
        public void ChangeCarXCoordinate()
        {
            // Arrange
            var newCarX = 100;
            var expectedX = car.X + (int)(newCarX * Math.Cos(0));
            var expectedY = car.Y + (int)(newCarX * Math.Sin(0));

            //Act
            car.X = newCarX;

            //Assert
            camera.X.Should().Be(expectedX);
            camera.Y.Should().Be(expectedY);
        }

        [Fact]
        public void ChangeCarYCoordinate()
        {
            // Arrange
            var newCarY = 100;
            var expectedX = car.X + (int)(newCarY * Math.Cos(0));
            var expectedY = car.Y + (int)(newCarY * Math.Sin(0));

            //Act
            car.X = newCarY;

            //Assert
            camera.X.Should().Be(expectedX);
            camera.Y.Should().Be(expectedY);
        }

        [Fact]
        public void ChangeCarRotation()
        {
            //Arrange
            var newRotation = -90;
            var expectedRotation = newRotation + 90;

            //Act
            car.Rotation = newRotation;

            //Assert
            camera.Rotation.Should().Be(expectedRotation);
        }

        [Fact]
        public void TestProcessMethod()
        {
            //Arrange
            car.Geometries.Add(new PolylineGeometry(new List<Point> { new Point(0, 0)}, true));
            car.Speed = 10;

            var testObject = new Circle(0, 10, "object1.png", 1);
            testObject.Collideable = true;
            testObject.Geometries = new List<PolylineGeometry>();
            testObject.RawGeometries = new List<PolylineGeometry>();
            testObject.Geometries.Add(new PolylineGeometry(new List<Point> { new Point(10, 0) }, true));
            testObject.RawGeometries.Add(new PolylineGeometry(new List<Point> { new Point(10, 0) }, true));
            worldObjects.Add(testObject);

            //Act
            camera.Process();

            //Assert
            car.VirtualFunctionBus.CameraPackets.Should().NotBeNull();
            car.VirtualFunctionBus.CameraPackets.Should().HaveCount(1);
            car.VirtualFunctionBus.CameraPackets[0].Distance.Should().Be(10);
            car.VirtualFunctionBus.CameraPackets[0].Angle.Should().Be(0);
        }
    }
}
