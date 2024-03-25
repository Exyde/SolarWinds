using UnityEngine;

public static class TransformExtensions{
    public static void Clear(this Transform transform){
        int i;
        for (i = transform.childCount - 1; i >= 0; i--){

            if (Application.IsPlaying(transform.GetChild(i)))
                Object.Destroy((transform.GetChild(i).gameObject));
            else
                Object.DestroyImmediate((transform.GetChild(i).gameObject));
        }
    }
}