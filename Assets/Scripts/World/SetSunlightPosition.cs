using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSunlightPosition : MonoBehaviour
{
    void Start()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }

    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
