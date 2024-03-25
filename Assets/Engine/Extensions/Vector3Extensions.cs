using UnityEngine;

public static class Vector3Extensions{
    public static Vector3 Random(this Vector3 vector){
        vector.x = UnityEngine.Random.Range(0f, 1f);
        vector.y = UnityEngine.Random.Range(0f, 1f);
        vector.z = UnityEngine.Random.Range(0f, 1f);
        return vector;
    }
}