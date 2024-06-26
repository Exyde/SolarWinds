    using UnityEngine;

public static class Vector3Extensions{
    public static Vector3 Random(this Vector3 vector){
        vector.x = UnityEngine.Random.Range(0f, 1f);
        vector.y = UnityEngine.Random.Range(0f, 1f);
        vector.z = UnityEngine.Random.Range(0f, 1f);
        return vector;
    }
    
    public static Vector3 RandomPosInCircle (this Vector3 vec, float radius)
    {
        var y = vec.y;
        var halfRadius = radius / 2f;
        return vec + new Vector3(
            UnityEngine.Random.Range(-halfRadius, halfRadius),
            y,
            UnityEngine.Random.Range(-halfRadius, halfRadius)
        );
    }

    public static Vector3 RandomPosInSphere(this Vector3 vec, Vector3 size)
    {
        return vec + new Vector3(
            UnityEngine.Random.Range(-size.x /2, size.x),
            UnityEngine.Random.Range(-size.y /2, size.y),
            UnityEngine.Random.Range(-size.z /2, size.z)
        );
    }

    public static Vector3 RandomPosInSphere(this Vector3 vec, float size)
    {
        return RandomPosInSphere(vec, new Vector3(size, size, size));
    }
}