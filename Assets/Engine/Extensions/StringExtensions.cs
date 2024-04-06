using UnityEngine;

public static class StringExtension {
    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str,string clr) => $"<color={clr}>{str}</color>";
    public static string Color(this string str, Color color) => str.Color($"#{ColorUtility.ToHtmlStringRGBA(color)}");
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => $"<size={size}>{str}</size>";
    public static string RemoveIllegalCharacters(this string str, string sep = ""){
        return string.Join(sep, str.Split(System.IO.Path.GetInvalidFileNameChars()));
    }

    public static string GetRandomString(int length)
    {
        var random = "";

        for (var i = 0; i < length; i++)
        {
            random += GetRandomLetter();
        }

        return random;
    }

    static System.Random _random = new ();
    public static string GetRandomLetter()
    {
        return ((char)_random.Next('a', 'z')).ToString();
    }
}