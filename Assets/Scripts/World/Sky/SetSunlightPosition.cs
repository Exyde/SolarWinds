using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSunlightPosition : MonoBehaviour, ITickable
{
    void Start()
    {
        WorldClock._instance.RegisterTickable(this);
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }

    void Update()
    {
        //Shader.SetGlobalVector("_SunDirection", transform.forward);
    }

    public void Tick(){
        Debug.Log("Update pos");
        transform.Rotate(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180), Space.Self);
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
