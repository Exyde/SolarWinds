using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset){

        float[,] map = new float[width,height];

        //Setting up offset to sample from this according to the seed
        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + offset.x; //Correct range for best results
            float offsetY= prng.Next(-100000, 100000) + offset.y; //Correct range for best results
            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) scale = 0.0001f;

        //Value for remaping noise to 0-1 range at the end
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        //Remapping to the center for the scale zoom
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){

                //Reseting values
                float noiseHeight = 0;
                float amplitude = 1;
                float frequency = 1;

                for (int i = 0; i < octaves; i++){
                    float sampleX = (x - halfWidth) / scale * frequency + octavesOffset[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency +  octavesOffset[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // [Remaping noise between [-1, 1]]
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence; //Decrease each octaves, since persistence -> [0 ,1]
                    frequency *= lacunarity; //Grow each octaves, since lacunarity > 1
                }

                //Updating boundaries
                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;

                map[x,y] = noiseHeight;
            }
        } 

        //Remapping noise [0, 1]
        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){
                map[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x,y]); 
            }
        }
        return map;
    }
}
