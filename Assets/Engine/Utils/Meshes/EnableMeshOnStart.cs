using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class EnableMeshOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = true;
        Destroy(this);
    }
}
