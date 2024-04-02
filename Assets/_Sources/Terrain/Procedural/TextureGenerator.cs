
using UnityEngine;
using System.Collections;

public static class  TextureGenerator{
    public static Texture2D GenerateTextureFromColorMap(Color[] map, int width, int height){
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(map);
        texture.Apply();
        return texture;
    }

    public static Texture2D GenerateTextureFromHeightMap(float[,] heightMap){
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);


        Color[] colorMap = new Color[width * height];

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x,y]);
            }
        }

        return GenerateTextureFromColorMap(colorMap, width, height);
    }
}