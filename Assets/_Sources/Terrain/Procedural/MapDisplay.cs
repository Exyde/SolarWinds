using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [Header("Texture Mode")]
    [SerializeField] Renderer _textureRenderer;

    [Header("Mesh Mode")]
    [SerializeField] MeshFilter _meshFilter;
    [SerializeField] MeshRenderer _meshRenderer;

    public void DrawTexture(Texture2D texture){
        _textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
        _textureRenderer.sharedMaterial.SetTexture("_NoiseTexture", texture);
       // _textureRenderer.sharedMaterial.mainTexture = texture; //shared so applied in editor ! vs material which is the instance :o
    }

    public void DrawMesh(MeshData data, Texture2D texture){
        _meshFilter.sharedMesh = data.CreateMesh();
        _meshRenderer.sharedMaterial.mainTexture = texture;
        _meshRenderer.sharedMaterial.SetTexture("_NoiseTexture", texture);
    }

} 
