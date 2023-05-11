using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExydeToolbox{

    public class FastSceneReload : MonoBehaviour{

        [SerializeField] KeyCode _reloadKey = KeyCode.N;

        void Update(){
           if (Input.GetKeyDown (_reloadKey)) Reload();
        }

        void Reload(){
            SceneManager.LoadScene ( SceneManager.GetActiveScene().buildIndex);
        }
    }
}
    
