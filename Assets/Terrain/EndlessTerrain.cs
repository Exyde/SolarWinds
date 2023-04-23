using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    //Position in World Value = 240, 240...
    //Coordinate in the grid system : Position/Chunksize => 1,1 for instance

    public const float MAX_VIEW_DISTANCE = 450f;
    [SerializeField] Transform _viewer;
    [SerializeField] public static Vector2 _viewerPosition;
    int _chunkSize;
    int _chunksVisibleCountInViewDistance;

    //Coordonate to Terrain Chunk Dictionnary
    Dictionary<Vector2, TerrainChunk> _terrainChunksDict = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> _terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start() {
        _chunkSize = MapGenerator.MAP_CHUNK_SIZE - 1;
        _chunksVisibleCountInViewDistance = Mathf.RoundToInt( MAX_VIEW_DISTANCE / _chunkSize);   
    }

    void Update(){
        _viewerPosition = new Vector2(_viewer.position.x, _viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks(){

        //Set back to invisible each update :[
        for (int i = 0; i < _terrainChunksVisibleLastUpdate.Count; i++){
            _terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }

        _terrainChunksVisibleLastUpdate.Clear();

        Vector2 currentChunkCoord = GetCurrentChunkCoord();

        for (int yOffset = -_chunksVisibleCountInViewDistance; yOffset <= _chunksVisibleCountInViewDistance; yOffset++){
            for (int xOffset = -_chunksVisibleCountInViewDistance; xOffset <= _chunksVisibleCountInViewDistance; xOffset++){
            
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoord.x + xOffset, currentChunkCoord.y + yOffset);

                //Update chunk mesh visibility or create a new one. 
                if (_terrainChunksDict.ContainsKey(viewedChunkCoord)){  
                    _terrainChunksDict[viewedChunkCoord].UpdateChunkSelfVisibility();
                    if (_terrainChunksDict[viewedChunkCoord].IsVisible()){
                        _terrainChunksVisibleLastUpdate.Add(_terrainChunksDict[viewedChunkCoord]);
                    }
                } else{
                    _terrainChunksDict.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, _chunkSize, this.transform));
                }
            }
        }
    }

    Vector2 GetCurrentChunkCoord(){
        return new Vector2(Mathf.RoundToInt( _viewerPosition.x / _chunkSize), Mathf.RoundToInt(_viewerPosition.y / _chunkSize));
    }

    public class TerrainChunk{

        Vector2 _position;
        GameObject _meshObject;
        Bounds _bounds;
        public TerrainChunk(Vector2 coord, int size, Transform parent){
            _position = coord * size;
            _bounds = new Bounds(_position, Vector2.one * size);
            Vector3 positionV3 =  new Vector3(_position.x, 0, _position.y);

            _meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _meshObject.transform.position = positionV3;
            _meshObject.transform.localScale = Vector3.one * size / 10f; //Size is 10 by default.
            _meshObject.transform.parent = parent;
            _meshObject.name = $"Debug Plane : {coord} ";

            SetVisible(false);
        }

        public void UpdateChunkSelfVisibility(){
            float smallestDstToViewer = Mathf.Sqrt(_bounds.SqrDistance(_viewerPosition)); //@EXPENSIVE
            bool visible = smallestDstToViewer <= MAX_VIEW_DISTANCE;

            SetVisible(visible);
        }

        public void SetVisible(bool state) => _meshObject.SetActive(state);
        public bool IsVisible() => _meshObject.activeSelf;
    }
}
