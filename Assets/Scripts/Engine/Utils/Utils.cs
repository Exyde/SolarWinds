using UnityEngine;

//String extensions methods to help display string in a particuliar style easily.
public static class StringExtension {
    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str,string clr) => string.Format("<color={0}>{1}</color>",clr, str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>",size,str);

    public static string RemoveIllegalCharacters(this string str, string sep = ""){
        return string.Join(sep, str.Split(System.IO.Path.GetInvalidFileNameChars()));
    }
}

public static class TransformExtensions{
    public static void Clear(this Transform transform){
        int i;
        for (i = transform.childCount - 1; i >= 0; i--){

        if (Application.IsPlaying(transform.GetChild(i)))
            GameObject.Destroy((transform.GetChild(i).gameObject));
        else
            GameObject.DestroyImmediate((transform.GetChild(i).gameObject));
        }
    }
}

public static class Vector3Extensions{
    public static Vector3 Random(this Vector3 vector){
        vector.x = UnityEngine.Random.Range(0f, 1f);
        vector.y = UnityEngine.Random.Range(0f, 1f);
        vector.z = UnityEngine.Random.Range(0f, 1f);
        return vector;
    }
}