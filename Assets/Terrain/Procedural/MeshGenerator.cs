using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int LOD){
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //Centering the mesh
        float topLeftX = (width -1) / -2f;
        float topLeftZ = (height -1) / 2f;

        //We use this iterator to skip vertices according to LOD value ! 
        int meshSimplificationIncrement = (LOD == 0) ? 1 : LOD * 2;
        int verticesPerLine = (width - 1)/meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y+= meshSimplificationIncrement){
            for (int x = 0; x < width; x+= meshSimplificationIncrement){

                meshData._vertices[vertexIndex] = new Vector3(topLeftX + x, _heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData._uvs[vertexIndex] = new Vector2(x /(float)width, y  /(float)height); //Percentage of the mesh on both axis between 0-1

                //Not on the right edge or bottom edge
                if (x < width - 1 && y < height - 1){
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine +1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        
        return meshData;
    }
}
public class MeshData{
    public Vector3[] _vertices;
    public int[] _triangles;
    public Vector2[] _uvs;

    int _triangleIndex;

    public MeshData(int width, int height){
        _vertices = new Vector3[width * height];
        _triangles = new int[(width - 1) * (height - 1) * 6];
        _uvs = new Vector2[width * height];
    }

    public void AddTriangle(int a, int b, int c){
        _triangles[_triangleIndex] = a;
        _triangles[_triangleIndex + 1] = b;
        _triangles[_triangleIndex + 2] = c;

        _triangleIndex += 3;
    }

    public Mesh CreateMesh(){
        Mesh mesh = new Mesh(); 
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.uv = _uvs;
        //Todo : have fun and add more uvs

        mesh.RecalculateNormals();
        return mesh;
    }
}
