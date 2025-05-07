using AutomatedCar;
using AutomatedCar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Helpers
{
    public class SpeedTests
    {
        private const double Tolerance = 1e-6;

        private double GetPixelsPerTick(SpeedHelper speed)
        {
            Type speedType = typeof(SpeedHelper);
            PropertyInfo pixelsPerTickProperty = speedType.GetProperty("PixelsPerTick", BindingFlags.NonPublic | BindingFlags.Instance);
            return (double)pixelsPerTickProperty.GetValue(speed);
        }

        private void SetPixelsPerTick(SpeedHelper speed, double pixelsPerTick)
        {
            Type speedType = typeof(SpeedHelper);
            PropertyInfo pixelsPerTickProperty = speedType.GetProperty("PixelsPerTick", BindingFlags.NonPublic | BindingFlags.Instance);
            pixelsPerTickProperty.SetValue(speed, pixelsPerTick);
        }

        [Fact]
        public void FromPixelsPerTick_ShouldCreateCorrectSpeed()
        {
            // Arrange
            double pixelsPerTick = 5.0;

            // Act
            SpeedHelper speed = SpeedHelper.FromPixelsPerTick(pixelsPerTick);

            // Assert
            double expected = pixelsPerTick;
            Assert.InRange(GetPixelsPerTick(speed), expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void FromMetersPerSecond_ShouldCreateCorrectSpeed()
        {
            // Arrange
            double metersPerSecond = 1.0;

            // Act
            SpeedHelper speed = SpeedHelper.FromMetersPerSecond(metersPerSecond);

            // Assert
            double expected = SpeedHelper.MeterToPixels / GameBase.TicksPerSecond;
            Assert.InRange(GetPixelsPerTick(speed), expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void FromKmPerHour_ShouldCreateCorrectSpeed()
        {
            // Arrange
            double kmPerHour = 3.6; // 1 m/s

            // Act
            SpeedHelper speed = SpeedHelper.FromKmPerHour(kmPerHour);

            // Assert
            double expected = SpeedHelper.MeterToPixels / GameBase.TicksPerSecond;
            Assert.InRange(GetPixelsPerTick(speed), expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void InPixelsPerTick_ShouldReturnCorrectValue()
        {
            // Arrange
            double pixelsPerTick = 5.0;
            SpeedHelper speed = SpeedHelper.FromPixelsPerTick(pixelsPerTick);
            SetPixelsPerTick(speed, pixelsPerTick);

            // Act
            double result = speed.InPixelsPerTick();

            // Assert
            double expected = pixelsPerTick;
            Assert.InRange(result, expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void InMetersPerSecond_ShouldReturnCorrectValue()
        {
            // Arrange
            double pixelsPerTick = 1.0;
            SpeedHelper speed = SpeedHelper.FromPixelsPerTick(pixelsPerTick);
            SetPixelsPerTick(speed, pixelsPerTick);

            // Act
            double result = speed.InMetersPerSecond();

            // Assert
            double expected = GameBase.TicksPerSecond / SpeedHelper.MeterToPixels;
            Assert.InRange(result, expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void InKmPerHour_ShouldReturnCorrectValue()
        {
            // Arrange
            double pixelsPerTick = SpeedHelper.MeterToPixels / GameBase.TicksPerSecond;
            SpeedHelper speed = SpeedHelper.FromPixelsPerTick(pixelsPerTick);
            SetPixelsPerTick(speed, pixelsPerTick);

            // Act
            double result = speed.InKmPerHour();

            // Assert
            double expected = 3.6; // 1 m/s
            Assert.InRange(result, expected - Tolerance, expected + Tolerance);
        }

        [Fact]
        public void FromPixelsPerTick_InPixelsPerTick_ShouldReturnSameValue()
        {
            // Arrange
            double ticksPerSecond = 500.0;

            // Act
            SpeedHelper speed = SpeedHelper.FromPixelsPerTick(ticksPerSecond);
            double result = speed.InPixelsPerTick();

            // Assert
            Assert.InRange(ticksPerSecond, result - Tolerance, result + Tolerance);
        }

        [Fact]
        public void FromKmPerHour_InKmPerHour_ShouldReturnSameValue()
        {
            // Arrange
            double kmPerHour = 50.0;

            // Act
            SpeedHelper speed = SpeedHelper.FromKmPerHour(kmPerHour);
            double result = speed.InKmPerHour();

            // Assert
            Assert.InRange(kmPerHour, result - Tolerance, result + Tolerance);
        }

        [Fact]
        public void FromMetersPerSecond_InMetersPerSecond_ShouldReturnSameValue()
        {
            // Arrange
            double metersPerSecond = 10.0;

            // Act
            SpeedHelper speed = SpeedHelper.FromMetersPerSecond(metersPerSecond);
            double result = speed.InMetersPerSecond();

            // Assert
            Assert.InRange(metersPerSecond, result - Tolerance, result + Tolerance);
        }
    }
}
