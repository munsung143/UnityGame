using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    [SerializeField] int seed;
    [SerializeField] float noiseScale = 1.0F;
    [SerializeField] Texture2D noiseTexture;
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase tile;
    private Color[] pixelColors;

    private void Awake()
    {
        GenerateNoiseTexture();
        GenerateTileMap();
    }
    public void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(mapWidth, mapHeight);
        pixelColors = new Color[mapWidth * mapHeight];

        for (float y = 0.0F; y < noiseTexture.height; y++)
        {
            for (float x = 0.0F; x < noiseTexture.width; x++)
            {
                float xCoord = seed + x / noiseTexture.width * noiseScale;
                float yCoord = seed + y / noiseTexture.height * noiseScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pixelColors[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTexture.SetPixels(pixelColors);
        noiseTexture.Apply();
    }
    public void GenerateTileMap()
    {
        for (int y = 0; y < noiseTexture.height; y++)
        {
            for (int x = 0; x < noiseTexture.width; x++)
            {
                Color c = noiseTexture.GetPixel(x, y);
                if (c.r < 0.5f)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }
}
