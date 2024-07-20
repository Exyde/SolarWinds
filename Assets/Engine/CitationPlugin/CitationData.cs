using UnityEngine;

[CreateAssetMenu(menuName = "Engine/Citation", fileName = "New Citation")]
public class CitationData : ScriptableObject
{
    [SerializeField] private string _author = "Unknown Author";
    [SerializeField, TextArea(10, 25)] private string _citation;

    public string Author => _author;
    public string Citation => _citation;
}
