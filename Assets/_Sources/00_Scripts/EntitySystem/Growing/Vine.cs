using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    public Material _material;

    void Awake()
    {
        //GameObject go = Instantiate(prefab, FindEndPosition(), Quaternion.identity);
        //go.transform.parent = this.transform;
        _material = GetComponent<MeshRenderer>().material;
    }  
}
