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

    Vector3 FindEndPosition(){
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3 endPositionLocal = vertices[0];
        for (int i = 1; i < vertices.Length; i++) {
            Vector3 point = vertices[i];
            if (point.x < endPositionLocal.x) {
                endPositionLocal = point;
            }
        }
        Matrix4x4 matrix = transform.localToWorldMatrix;
        Vector3 endPosition = matrix.MultiplyPoint(endPositionLocal);
        return endPosition;
    }
}
