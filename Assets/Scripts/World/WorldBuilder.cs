using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    [SerializeField][Range(1, 256)] int _resolution = 32;
    [SerializeField][Range(.1f, 8f)] float _meshBaseScale = .2f;
    [SerializeField] Vector2 _meshVerticalScale;
    [SerializeField] bool _disableCollision = false;

    [SerializeField] PrimitiveType _meshType = PrimitiveType.Capsule;
    [SerializeField] Mesh _mesh;
    [SerializeField] bool _useCustomMesh;
    [SerializeField] Material _material;
    [SerializeField] Material _lightMaterial;


    [Header("Colors")]
    [SerializeField] bool _randomizeColor;

    GameObject go;

    public void GenerateWorld(){
        Debug.Log("Generating World !");
        transform.Clear();


        for (int i =0; i < _resolution; i++){
            for (int j =0; j < _resolution; j++){

                if (_useCustomMesh){
                    go = new GameObject();
                    go.AddComponent<MeshFilter>().mesh = _mesh;
                    MeshRenderer renderer = go.AddComponent<MeshRenderer>();
                    go.AddComponent<BoxCollider>();


                    Material[] mats = new Material[]{
                        _material, _lightMaterial
                    };
                    
                    renderer.materials = mats;

                }

                else{
                    go = GameObject.CreatePrimitive(_meshType);
                }
                go.name = "Entity : " + i + j;
                go.transform.parent = this.transform;

                go.transform.position = new Vector3(Random.Range(- _resolution/2f * _resolution, _resolution/2f * _resolution), 0, Random.Range(- _resolution/2f * _resolution, _resolution/2f * _resolution));
                Vector3 randomScale = new Vector3(_meshBaseScale, Random.Range(_meshVerticalScale.x, _meshVerticalScale.y), _meshBaseScale);
                go.transform.localScale = randomScale;

                SetWorldCollision();
                SetWorldColors();

            }
        }
    }

    public void SetWorldCollision(){
        if (_disableCollision){
            DestroyImmediate(go.GetComponent<Collider>());
        }
    }

    public void SetWorldColors(){
        go.GetComponent<MeshRenderer>().material = _material;

        if (_randomizeColor){

            _material = new Material(Shader.Find("Shader Graphs/G_BiColor"));

            Color colorA = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            Color colorB = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

            _material.SetColor("_Color_A", colorA);
            _material.SetColor("_Color_B", colorB);
        }
    }

    public void ClearWorld(){
        transform.Clear();
    }

}
