using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetshaderPlayer : MonoBehaviour
{
    [SerializeField] Material mat;

    void Update()
    {
        float t = Mathf.Sin(transform.position.x + transform.position.z ) /2f + 0.5f;
        t = Mathf.Clamp(t, 0, 1);
        mat.SetFloat("_Grow", t);
    }
}
