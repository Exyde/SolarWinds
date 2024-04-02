using System;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    private enum CameraMode
    {
        LowRes,
        Normal
    };

    [SerializeField] private CameraMode _cameraMode;
    
    [SerializeField] Camera _playerCamera;
    [SerializeField] private Camera _lowResCamera;
    [SerializeField] RenderTexture _lowResRenderTexture;

    private void Awake() => SetCameraMode();
    private void OnValidate() => SetCameraMode();

    private void SetCameraMode()
    {
        switch (_cameraMode)
        {
            case CameraMode.LowRes:
                _playerCamera.targetTexture = _lowResRenderTexture;
                _lowResCamera.gameObject.SetActive(true);
                break;
            case CameraMode.Normal:
                _playerCamera.targetTexture = null;
                _lowResCamera.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }        switch (_cameraMode)
        {
            case CameraMode.LowRes:
                _playerCamera.targetTexture = _lowResRenderTexture;
                break;
            case CameraMode.Normal:
                _playerCamera.targetTexture = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
