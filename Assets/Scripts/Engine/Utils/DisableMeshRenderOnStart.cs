using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class DisableMeshRenderOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(this);
    }
}
