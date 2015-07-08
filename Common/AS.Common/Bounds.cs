using UnityEngine;

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
            var x = _center.x - (_extents.x * LongestAxis(_extents).x * 0.5f);
            var y = _center.y - (_extents.y * LongestAxis(_extents).y * 0.5f);
            var z = _center.z - (_extents.z * LongestAxis(_extents).z * 0.5f);
            bounds[0] = new Bounds(new Vector3(x, y, z), _extents / 2f);
            bounds[1] = new Bounds(new Vector3(-x, -y, -z), _extents / 2f);
            return bounds;
        }

        private Vector3 LongestAxis(Vector3 source)
        {
            if (source.x > source.y && source.x > source.z) return new Vector3(1, 0, 0);
            if (source.y > source.x && source.y > source.z) return new Vector3(0, 1, 0);
            if (source.z > source.x && source.z > source.y) return new Vector3(0, 0, 1);
            return new Vector3(1, 0, 0);
        }

        public override string ToString()
        {
            return $"{_center.x}_{_center.y}_{_center.z}";
        }

        public static bool GreaterOrEqual(Vector3 c1, Vector3 c2)
        {
            return c1.x >= c2.x && c1.y >= c2.y && c1.z >= c2.z;
        }
        public static bool LessOrEqual(Vector3 c1, Vector3 c2)
        {
            return c1.x <= c2.x && c1.y <= c2.y && c1.z <= c2.z;
        }
    }
}
