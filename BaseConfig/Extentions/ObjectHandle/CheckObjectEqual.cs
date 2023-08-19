using Newtonsoft.Json;

namespace BaseConfig.Extentions.ObjectHandle
{
    public static class CheckObjectEqual
    {
        public static bool ObjectAreEqual<T>(T a, T b)
        {
            var objectA = JsonConvert.SerializeObject(a);
            var objectB = JsonConvert.SerializeObject(b);
            return objectA == objectB;
        }
    }
}
