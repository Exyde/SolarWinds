using UnityEngine;

namespace _Sources.Scripts.MainMenu
{
    [CreateAssetMenu(menuName = "Engine/FragmentData", fileName = "New Fragment Data")]
    public class FragmentData : ScriptableObject
    {
        [SerializeField] public string FragmentName;
        [SerializeField, TextArea(10, 25)] public string Description;
        [SerializeField] public bool Available;

        [Space(5)] public string SceneToLoad = "";

    }
}