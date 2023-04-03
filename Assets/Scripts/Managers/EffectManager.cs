using UnityEngine;
using Core.GameEvents;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour, IGameEventManager
{
    [SerializeField] ParticleSystem PS_OnPictureFlash;
    [SerializeField] ParticleSystem PH_PS_NegativeFeedback;

    private void OnEnable() {

    }

    private void OnDisable() {


    }


    #region Interfaces Handlers
    public void HandleTriggerEvents(EventName eventName, string senderName)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCollisionEvents(EventName eventName, string senderName)
    {
        throw new System.NotImplementedException();
    }

    public void HandleRaycastEvents(EventName eventName, string senderName)
    {
        throw new System.NotImplementedException();
    }

    public void HandleSpecialCases(EventName eventName, string senderName)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Particle Systems
    private void PlayNegativeFeedBackFX(ParticleSystem fx, Vector3 pos = default){
        if (fx == null) return;
        Vector3 offset = new Vector3(0, 0, -2);
        Camera cam = Camera.main;
        Instantiate(fx, cam.transform.position + cam.transform.forward * 2, Quaternion.identity);
    }
    #endregion
}
