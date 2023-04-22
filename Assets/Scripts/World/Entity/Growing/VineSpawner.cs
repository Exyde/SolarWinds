using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class VineSpawner : MonoBehaviour
{
    [SerializeField] Material _vineMaterial;
    [SerializeField] GameObject _vinePrefab;
    [SerializeField] int _spawnAmount;

    [SerializeField][Range(0.1f, 60f)] float _growTime = 1f;
    [SerializeField] bool _steppedGrowth;
    [SerializeField] float _growthStep = 0.1f;

    [Header("Recursive")]
    [SerializeField] Transform _lastParent = null;
    [SerializeField] Vine _currentVine;
    [SerializeField] int _recursions;
    void Start()
    {
        //SpawnVines();
        SpawnVine();
        StartCoroutine(Grow(_currentVine));
    }

    IEnumerator Grow(Vine vine){
        float currentTime = 0f;

        //Todo : Can be on World tick ?
        while (currentTime < _growTime){

            float t = currentTime / _growTime;

            vine._material.SetFloat("_Grow", t);
            currentTime += Time.deltaTime;
            if (_steppedGrowth) yield return new WaitForSeconds(_growthStep);
        }

        if (_recursions > 0){
            _recursions--;
            SpawnVine();
            StartCoroutine(Grow(_currentVine));
        }
        yield return null;
    }

    [ContextMenu("Grow")]
    public void GrowContextMenu(){
        StartCoroutine("Grow");
    }

    [ContextMenu("Spawn vines")]
    public void SpawnVinesContextMenu(){
        SpawnVines();
    }

    private void SpawnVines(){
        transform.Clear();
            for (int i =0; i < _spawnAmount; i++){
            GameObject go = Instantiate(_vinePrefab, transform.position, Random.rotation);
            go.transform.parent = this.transform;
        }
    }

    private void SpawnVine(){
        Vector3 pos = _currentVine == null ? this.transform.position : FindEndPosition(_currentVine);
        GameObject go = Instantiate(_vinePrefab, pos, Random.rotation);
        go.transform.parent = _lastParent == null ? this.transform : _lastParent;
        _lastParent = go.transform;
        _currentVine = go.GetComponent<Vine>();
    }

    Vector3 FindEndPosition(Vine vine){
        Mesh mesh = vine.GetComponent<MeshFilter>().mesh;
        Vector3 offset = new Vector3(-2.75f, 0, 0);
        return vine.transform.position + vine.transform.rotation * offset;
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
