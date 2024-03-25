using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    //Todo : Enum with Camera Mode
    
    [SerializeField] Camera _playerCamera;
    [SerializeField] Camera _lowResCamera;
    [SerializeField] RenderTexture _lowResRenderTexture;

    private void Awake() {

        if (_lowResCamera.isActiveAndEnabled){
            _playerCamera.targetTexture = _lowResRenderTexture;
        }
    }
}