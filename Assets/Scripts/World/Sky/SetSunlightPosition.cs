using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSunlightPosition : MonoBehaviour, ITickable
{
    [SerializeField] bool _update;
    [SerializeField] Transform _target;
    [SerializeField] bool _lookAtTarget;
    void Start()
    {
        //WorldClock._instance.RegisterTickable(this);
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }

    void Update()
    {
        if (!_update) return;
        Shader.SetGlobalVector("_SunDirection", transform.forward);

        if (_lookAtTarget) transform.LookAt(_target);
    }

    public void Tick(){
        Debug.Log("Update pos");
        transform.Rotate(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180), Space.Self);
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
