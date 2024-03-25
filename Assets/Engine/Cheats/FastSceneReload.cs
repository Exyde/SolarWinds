using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExydeToolbox
{
    public class FastSceneReload : MonoBehaviour
    {
        [SerializeField] KeyCode _reloadKey = KeyCode.N;
        private void Update()
        {
           if (Input.GetKeyDown (_reloadKey)) Reload();
        }

        private static void Reload() => SceneManager.LoadScene ( SceneManager.GetActiveScene().buildIndex);
    }
}
    
