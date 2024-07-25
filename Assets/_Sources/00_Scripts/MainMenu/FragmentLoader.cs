using System.Collections.Generic;
using _Sources.Scripts.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FragmentLoader : MonoBehaviour
{
    [SerializeField] private List<FragmentThumbnail> _fragmentThumbnails;

    public static void LoadFragment(string fragmentScene)
    {
        SceneManager.LoadScene(fragmentScene); 
    }
    
}
