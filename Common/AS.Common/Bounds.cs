﻿using System;
using UnityEngine;

namespace AS.Common
{
    public class Bounds
    {
        private AS.Common.Vector3 _minimum;
        private AS.Common.Vector3 _maximum;

        public AS.Common.Vector3 _center { get; private set; }
        public AS.Common.Vector3 _extents { get; private set; }

        public Bounds()
        {

        }
        public Bounds(AS.Common.Vector3 c, AS.Common.Vector3 e)
        {
            _center = c;
            _extents = e;

            _minimum = _center - _extents;
            _maximum = _center + _extents;
        }
        public Bounds(AS.Common.Vector3 c, float size)
        {
            _center = c;
            _extents = new AS.Common.Vector3(size, size, size);

            _minimum = _center - _extents;
            _maximum = _center + _extents;
        }

        public bool Contains(AS.Common.Vector3 location)
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
            bounds[0] = new Bounds(new AS.Common.Vector3(x, y, z), _extents / 2f);
            bounds[1] = new Bounds(new AS.Common.Vector3(-x, -y, -z), _extents / 2f);
            return bounds;
        }

        private AS.Common.Vector3 LongestAxis(AS.Common.Vector3 source)
        {
            if (source.x > source.y && source.x > source.z) return new AS.Common.Vector3(1, 0, 0);
            if (source.y > source.x && source.y > source.z) return new AS.Common.Vector3(0, 1, 0);
            if (source.z > source.x && source.z > source.y) return new AS.Common.Vector3(0, 0, 1);
            return new AS.Common.Vector3(1, 0, 0);
        }

        public override string ToString()
        {
            return String.Format("{0}_{1}_{2}", _center.x, _center.y, _center.z);
        }

        public static bool GreaterOrEqual(AS.Common.Vector3 c1, AS.Common.Vector3 c2)
        {
            return c1.x >= c2.x && c1.y >= c2.y && c1.z >= c2.z;
        }
        public static bool LessOrEqual(AS.Common.Vector3 c1, AS.Common.Vector3 c2)
        {
            return c1.x <= c2.x && c1.y <= c2.y && c1.z <= c2.z;
        }
    }
}
