using System.Text;
using UnityEngine;

public static class JsonBinaryUtility
{
    public static byte[] ToByteArray(object data)
    {
        return Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
    }

    public static T FromByteArray<T>(byte[] data)
    {
        return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(data));
    }
}
