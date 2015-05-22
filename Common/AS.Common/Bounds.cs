using System.Numerics;

namespace AS.Common
{
    public class Bounds
    {
        private Vector3 _minimum;
        private Vector3 _maximum;

        public Vector3 _center { get; private set; }
        public Vector3 _extents { get; private set; }

        public Bounds()
        {

        }
        public Bounds(Vector3 c, Vector3 e)
        {
            _center = c;
            _extents = e;

            _minimum = _center - _extents;
            _maximum = _center + _extents;
        }
        public Bounds(Vector3 c, float size)
        {
            _center = c;
            _extents = new Vector3(size, size, size);

            _minimum = _center - _extents;
            _maximum = _center + _extents;
        }

        public bool Contains(Vector3 location)
        {
            if (GreaterOrEqual(location, _minimum) &&
                LessOrEqual(location, _maximum))
            {
                return true;
            }

            return false;
        }

        public Bounds[] Split()
        {
            Bounds[] bounds = new Bounds[2];
            var x = _center.X - (_extents.X * LongestAxis(_extents).X * 0.5f);
            var y = _center.Y - (_extents.Y * LongestAxis(_extents).Y * 0.5f);
            var z = _center.Z - (_extents.Z * LongestAxis(_extents).Z * 0.5f);
            bounds[0] = new Bounds(new Vector3(x, y, z), _extents / 2f);
            bounds[1] = new Bounds(new Vector3(-x, -y, -z), _extents / 2f);
            return bounds;
        }

        private Vector3 LongestAxis(Vector3 source)
        {
            if (source.X > source.Y && source.X > source.Z) return Vector3.UnitX;
            if (source.Y > source.X && source.Y > source.Z) return Vector3.UnitY;
            if (source.Z > source.X && source.Z > source.Y) return Vector3.UnitZ;
            return Vector3.UnitX;
        }

        public override string ToString()
        {
            return $"{_center.X}_{_center.Y}_{_center.Z}";
        }

        public static bool GreaterOrEqual(Vector3 c1, Vector3 c2)
        {
            return c1.X >= c2.X && c1.Y >= c2.Y && c1.Z >= c2.Z;
        }
        public static bool LessOrEqual(Vector3 c1, Vector3 c2)
        {
            return c1.X <= c2.X && c1.Y <= c2.Y && c1.Z <= c2.Z;
        }
    }
}
