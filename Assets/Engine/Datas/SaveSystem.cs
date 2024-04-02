using UnityEngine;
using System.IO;
using UnityEditor;

public static class SaveSystem
{
    public static void SaveTexToPng(Texture2D tex, string objectName){

        byte[] bytes = tex.EncodeToPNG();

        const string dirPath = Constants.PICTURES_SAVES_PATH;
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
        const string path = Constants.PICTURES_SAVES_PATH;
        var files = Directory.GetFiles(path);

        foreach (var file in files)
        {
            File.Delete(file);
        }

        Directory.Delete(path);
        Directory.CreateDirectory(path);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    
    internal static void EraseSaveDatas(){
        Directory.Delete(Constants.DIR_SAVE_PATH);
        Directory.CreateDirectory(Constants.DIR_SAVE_PATH);
        Directory.CreateDirectory(Constants.PICTURES_SAVES_PATH);
    }

}
