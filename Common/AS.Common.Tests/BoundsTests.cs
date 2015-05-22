using System;
using System.Numerics;
using Xunit;

namespace AS.Common.Tests
{
    public class BoundsTests
    {
        [Fact]
        public void bounds_defaultSplitX()
        {
            Bounds bounds = new Bounds(Vector3.Zero, Vector3.One);
            var split = bounds.Split();
            Assert.Equal(new Vector3(-0.5f, 0, 0), split[0]._center);
            Assert.Equal(new Vector3(0.5f, 0, 0), split[1]._center);
        }

        [Fact]
        public void bounds_splitX100()
        {
            Bounds bounds = new Bounds(Vector3.Zero, 100f);
            var split = bounds.Split();
            Assert.Equal(new Vector3(-50f, 0, 0), split[0]._center);
            Assert.Equal(new Vector3(50f, 0, 0), split[1]._center);

            Assert.Equal(new Vector3(50f, 50f, 50f), split[0]._extents);
            Assert.Equal(new Vector3(50f, 50f, 50f), split[1]._extents);
        }

        [Fact]
        public void bounds_splitX()
        {
            Bounds bounds = new Bounds(Vector3.Zero, new Vector3(2, 1, 1));
            var split = bounds.Split();
            Assert.Equal(new Vector3(-1f, 0, 0), split[0]._center);
            Assert.Equal(new Vector3(1f, 0, 0), split[1]._center);
        }

        [Fact]
        public void bounds_splitY()
        {
            Bounds bounds = new Bounds(Vector3.Zero, new Vector3(1, 2, 1));
            var split = bounds.Split();
            Assert.Equal(new Vector3(0, -1f, 0), split[0]._center);
            Assert.Equal(new Vector3(0, 1f, 0), split[1]._center);
        }

        [Fact]
        public void bounds_splitZ()
        {
            Bounds bounds = new Bounds(Vector3.Zero, new Vector3(1, 1, 2));
            var split = bounds.Split();
            Assert.Equal(new Vector3(0, 0, -1f), split[0]._center);
            Assert.Equal(new Vector3(0, 0, 1f), split[1]._center);
        }

        [Fact]
        public void bounds_whenContains_returnsTrue()
        {
            Bounds bounds = new Bounds(Vector3.Zero, 100f);
            Assert.True(bounds.Contains(new Vector3(10, 10, 10)));

            bounds = new Bounds(new Vector3(25, 25, 25), 50f);
            Assert.True(bounds.Contains(new Vector3(10, 10, 10)));
        }

        [Fact]
        public void bounds_whenDoesNotContain_returnsFalse()
        {
            //Bounds bounds = new Bounds(Vector3.Zero, 100f);
            //Assert.True(bounds.Contains(new Vector3(10, 10, 10)));

            var bounds = new Bounds(new Vector3(-25, -25, -25), 50f);
            Assert.True(bounds.Contains(new Vector3(10, 10, 10)));
        }
    }
}
