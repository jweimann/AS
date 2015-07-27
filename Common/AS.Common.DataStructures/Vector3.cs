using System;

namespace AS.Common
{
    [Serializable]
    public struct Vector3
    {
        private static Random _random = new Random();

        public float x;
        public float y;
        public float z;

        public Vector3(double x, double y, double z)
        {
            this.x = (float)x;
            this.y = (float)y;
            this.z = (float)z;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 position) : this()
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }

        public string ToRoundedString(int places = 2)
        {
            return Math.Round(x, places) + ", " + Math.Round(y, places) + ", " + Math.Round(z, places);
        }

        public static Vector3 operator +(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
        }

        public static Vector3 operator -(Vector3 c1, Vector3 c2)
        {
            return new Vector3(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
        }

        public static Vector3 operator /(Vector3 c1, int c2)
        {
            return new Vector3(c1.x / c2, c1.y / c2, c1.z / c2);
        }

        public static Vector3 operator /(Vector3 c1, float c2)
        {
            return new Vector3(c1.x / c2, c1.y / c2, c1.z / c2);
        }

        public static Vector3 operator *(Vector3 c1, int c2)
        {
            return new Vector3(c1.x * c2, c1.y * c2, c1.z * c2);
        }

        public static Vector3 operator *(Vector3 c1, float c2)
        {
            return new Vector3(c1.x * c2, c1.y * c2, c1.z * c2);
        }

        public static Vector3 operator *(Vector3 c1, double c2)
        {
            return c1 * (float)c2;
        }

        public static Vector3 operator *(double c2, Vector3 c1)
        {
            return c1 * (float)c2;
        }

        public static Vector3 operator -(Vector3 c1)
        {
            return c1 * -1f;
        }

        public static bool operator >(Vector3 c1, Vector3 c2)
        {
            return c1.x > c2.x && c1.y > c2.y && c1.z > c2.z;
        }

        public static bool operator <(Vector3 c1, Vector3 c2)
        {
            return c1.x < c2.x && c1.y < c2.y && c1.z < c2.z;
        }

        public static bool operator ==(Vector3 c1, Vector3 c2)
        {
            return c1.x == c2.x && c1.y == c2.y && c1.z == c2.z;
        }

        public static bool operator !=(Vector3 c1, Vector3 c2)
        {
            return c1.x != c2.x || c1.y != c2.y || c1.z != c2.z;
        }

        public static Vector3 zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }

        public static Vector3 one
        {
            get
            {
                return new Vector3(1, 1, 1);
            }
        }

        public static int GetSize()
        {
            return sizeof(float) + sizeof(float) + sizeof(float);
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
            Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
            return (float)Math.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
            //return (float)Math.Sqrt(Math.Pow((v1.x - v2.x), 2) + Math.Pow((v1.y - v2.y), 2) + Math.Pow((v1.z - v2.z), 2));
        }

        public Vector3 Normalized()
        {
            float length = (float)Math.Sqrt(x * x + y * y + z * z);
            return new Vector3(x / length, y / length, z / length);
        }

        public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x)
        {
            Vector3 P = (double)x * (B - A).Normalized() + A;
            return P;
        }

        public Vector3 WithoutHeight()
        {
            return new Vector3(this.x, 0, this.z);
        }

        public bool IsNan()
        {
            return (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z));
        }

        public static Vector3 Random(float range)
        {
            return new Vector3(GetRandomNumber(-range, range), GetRandomNumber(-range, range), GetRandomNumber(-range, range));
        }
        
        private static float GetRandomNumber(float minimum, float maximum)
        {
            return (float)(_random.NextDouble() * (maximum - minimum) + minimum);
        }
    }
}