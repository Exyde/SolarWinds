using System;
using TMPro;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Engine/Citation", fileName = "New Citation")]
public class CitationData : ScriptableObject
{
    [SerializeField] private string _author = "Unknown Author";
    [SerializeField, TextArea] private string _citation;

    public string Author => _author;
    public string Citation => _citation;

    private void OnValidate()
    {
        return;
        //Should rename the asset but it crash hardcore
        
        var suffix = _citation.Length >= 5 ? _citation[..5] : _citation;
        var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
        var newName = $"CD_{_author}_{suffix}";
        AssetDatabase.RenameAsset(assetPath, newName);
        AssetDatabase.SaveAssets();
    }
}
