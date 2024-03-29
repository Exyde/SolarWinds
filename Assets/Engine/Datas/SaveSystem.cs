using UnityEngine;
using System.IO;
using UnityEditor;

public static class SaveSystem
{
    public static void SaveTexToPng(Texture2D tex, string objectName){

        byte[] bytes = tex.EncodeToPNG();

        var dirPath = Constants.DIR_PICTURES_SAVE_PATH; //using constant name hard coded so that designer don't break it up

        if (!Directory.Exists(dirPath)){
            Directory.CreateDirectory(dirPath);
        }
        
        File.WriteAllBytes(dirPath + objectName.RemoveIllegalCharacters("").Replace(" ", "") + "_"  + ".png", bytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
    }

    internal static void EraseSavedPictures()
    {
        var datas = Directory.GetFiles(Constants.DIR_PICTURES_SAVE_PATH);

        foreach (var t in datas)
        {
            File.Delete(t);
        }

        Directory.Delete(Constants.DIR_PICTURES_SAVE_PATH);
        Directory.CreateDirectory(Constants.DIR_PICTURES_SAVE_PATH);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    
    internal static void EraseSaveDatas(){
        Directory.Delete(Constants.DIR_SAVE_PATH);
        Directory.CreateDirectory(Constants.DIR_SAVE_PATH);
        Directory.CreateDirectory(Constants.DIR_PICTURES_SAVE_PATH);
    }

}
