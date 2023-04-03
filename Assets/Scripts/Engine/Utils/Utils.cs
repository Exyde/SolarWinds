using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

}


//String extensions methods to help display string in a particuliar style easily.
public static class StringExtension {
    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str,string clr) => string.Format("<color={0}>{1}</color>",clr, str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>",size,str);

    public static string RemoveIllegalCharactersFromRubensDesignerMagicTool(this string str, string sep = ""){
        return string.Join(sep, str.Split(System.IO.Path.GetInvalidFileNameChars()));
    }
    
}