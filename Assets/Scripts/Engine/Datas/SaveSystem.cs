using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public static class SaveSystem
{
    public static bool _isEnabled = true;

    public static bool IsEnabled() => _isEnabled;

    public static void SaveTexToPng(Texture2D tex, string objectName){

        byte[] bytes = tex.EncodeToPNG();

        var dirPath = Constants.DIR_PICTURES_SAVE_PATH; //using constant name hard coded so that designer don't break it up

        if (!Directory.Exists(dirPath)){
            Directory.CreateDirectory(dirPath);
        }
        
        int randomTag = UnityEngine.Random.Range(0, 999);

        File.WriteAllBytes(dirPath + objectName.RemoveIllegalCharactersFromRubensDesignerMagicTool("").Replace(" ", "") + "_"  + ".png", bytes);

#if UNITY_EDITOR
//@BUILDCHECK
            AssetDatabase.Refresh();
#endif
    }

    internal static void EraseSavedPictures()
    {
        var datas = Directory.GetFiles(Constants.DIR_PICTURES_SAVE_PATH);

        for (int i = 0; i < datas.Length; i++){
            File.Delete(datas[i]);
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
