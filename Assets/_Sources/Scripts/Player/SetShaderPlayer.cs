using UnityEngine;
using UnityEngine.Serialization;

public class SetShaderPlayer : MonoBehaviour
{
    [FormerlySerializedAs("mat")] [SerializeField] private Material _material;

    private Vector3 _pos;
    private static readonly int Grow = Shader.PropertyToID("_Grow");

    private void Update()
    {
        _pos = transform.position;
        var t = Mathf.Sin(_pos.x + _pos.z ) /2f + 0.5f;
        t = Mathf.Clamp(t, 0, 1);
        _material.SetFloat(Grow, t);
    }
}
