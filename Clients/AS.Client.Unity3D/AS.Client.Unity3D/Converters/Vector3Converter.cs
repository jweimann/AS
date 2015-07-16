namespace AS.Client.Unity3D.Converters
{
    public static class Vector3Converter
    {
        public static UnityEngine.Vector3 ToUnity(this Common.Vector3 vector)
        {
            return new UnityEngine.Vector3(vector.x, vector.y, vector.z);
        }

        public static Common.Vector3 ToCommon(this UnityEngine.Vector3 vector)
        {
            return new Common.Vector3(vector.x, vector.y, vector.z);
        }
    }
}
