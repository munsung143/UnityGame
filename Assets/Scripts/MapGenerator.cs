using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] StructGenerator structGenerator;
    [SerializeField] Player player;

    [SerializeField] int mapWidth;
    public int MapWidth { get { return mapWidth; } }
    [SerializeField] int mapHeight;
    public int MapHeight { get { return mapHeight; } }

    [SerializeField] int seed;
    private int surfaceSeed;
    private int stoneSeed;
    private int caveSeed;
    private int rockSeed;

    [SerializeField] float surfaceNoiseFreq;
    [SerializeField] float caveNoiseFreq;
    [SerializeField] float stoneNoiseFreq;
    [SerializeField] float rockNoiseFreq;

    [SerializeField] Texture2D surfaceNoise;
    [SerializeField] Texture2D stoneNoise;
    [SerializeField] Texture2D rockNoise;
    [SerializeField] Texture2D caveNoise;

    [SerializeField] Tilemap tilemap;
    [SerializeField] Tiles tiles;
    public Tilemap Tilemap { get { return tilemap; } }
    [SerializeField] TileBase grass => tiles.grass;
    [SerializeField] TileBase dirt => tiles.dirt;
    [SerializeField] TileBase stone => tiles.stone;

    private Color[] pixelColors;
    private int defaultSurfaceHeight;
    private int defaultStoneHeight;

    private void Start()
    {
        Init();
        GenDefaultArea(mapWidth, defaultSurfaceHeight, dirt);
        GenNoiseLine(surfaceNoiseFreq, ref surfaceNoise);
        GenSurface();
        GenDefaultArea(mapWidth, defaultStoneHeight, stone);
        GenNoiseLine(stoneNoiseFreq, ref stoneNoise);
        GenStone();
        GenNoiseMap(rockNoiseFreq, ref rockNoise);
        GenRock();
        GenCaveNoise();
        GenCave();
        structGenerator.GenTrees();
        SetPlayerPos();
    }

    public void Init()
    {
        mapWidth = 1000;
        mapHeight = 1000;
        seed = Random.Range(0, 5000);
        surfaceSeed = seed * 2;
        caveSeed = seed * 3;
        stoneSeed = seed * 5;
        rockSeed = seed * 7;
        defaultSurfaceHeight = mapHeight * 2 / 3;
        defaultStoneHeight = mapHeight / 2;
        surfaceNoiseFreq = 0.03f;
        stoneNoiseFreq = 0.1f;
        caveNoiseFreq = 0.025f;
        rockNoiseFreq = 0.04f;

        surfaceNoise = new Texture2D(mapWidth, 1);
        stoneNoise = new Texture2D(mapWidth, 1);
        caveNoise = new Texture2D(mapWidth, defaultSurfaceHeight);
        rockNoise = new Texture2D(mapWidth, defaultSurfaceHeight);

    }

    public void GenNoiseLine(float freq, ref Texture2D tex)
    {
        pixelColors = new Color[tex.width];
        for (int x = 0; x < tex.width; x++)
        {
            float xCoord = (seed + x) * freq;
            float val = Mathf.PerlinNoise(xCoord, 1);
            pixelColors[x] = new Color(val, val, val);
        }
        tex.SetPixels(pixelColors);
        tex.Apply();
    }
    public void GenNoiseMap(float freq, ref Texture2D tex)
    {
        pixelColors = new Color[tex.width * tex.height];

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                float xCoord = (seed + x) * freq;
                float yCoord = (seed + y) * freq;
                float val = Mathf.PerlinNoise(xCoord, yCoord);
                pixelColors[y * tex.width + x] = new Color(val, val, val);
            }
        }
        tex.SetPixels(pixelColors);
        tex.Apply();
    }
    public void GenCaveNoise()
    {
        pixelColors = new Color[caveNoise.width * caveNoise.height];
        for (int y = 0; y < caveNoise.height * 4; y++)
        {
            for (int x = 0; x < caveNoise.width; x++)
            {
                if (y % 4  == 0)
                {
                    float xCoord = (caveSeed + x) * caveNoiseFreq;
                    float yCoord = (caveSeed + y) * caveNoiseFreq;
                    float val = Mathf.PerlinNoise(xCoord, yCoord);
                    pixelColors[y/4 * caveNoise.width + x] = new Color(val, val, val);
                }
            }
        }
        caveNoise.SetPixels(pixelColors);
        caveNoise.Apply();
    }
    public void GenDefaultArea(int width, int height, TileBase tile)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
    public void GenSurface()
    {
        for (int x = 0; x < surfaceNoise.width; x++)
        {
            Color c = surfaceNoise.GetPixel(x, 1);
            int height = (int)(c.r * 20);
            int h;
            for (h  = 0; h < height; h++)
            {
                tilemap.SetTile(new Vector3Int(x, defaultSurfaceHeight + h, 0), dirt);
            }
            tilemap.SetTile(new Vector3Int(x, defaultSurfaceHeight + h, 0), grass);
        }
    }
    public void GenCave()
    {
        float bound = 0.5f;
        for (int y = 0; y < caveNoise.height; y++)
        {
            for (int x = 0; x < caveNoise.width; x++)
            {
                Color c = caveNoise.GetPixel(x, y);
                if (c.r < bound)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), null);
                }
            }
            if (y % (caveNoise.height / 5) == 0)
            {
                bound = bound - 0.05f;
            }
        }
    }
    public void GenRock()
    {
        for (int y = 0; y < rockNoise.height; y++)
        {
            for (int x = 0; x < rockNoise.width; x++)
            {
                Color c = rockNoise.GetPixel(x, y);
                if (c.r > 0.6f)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), stone);
                }
            }
        }
    }

    public void SetPlayerPos()
    {
        Vector2 pos = new Vector2(500, 700);
        int temp = 700;
        while (Tilemap.GetTile(new Vector3Int(500, temp, 0)) != tiles.grass)
        {
            temp--;
        }
        pos.y = temp + 3;

        player.transform.position = pos;
    }
    public void GenStone()
    {
        for (int x = 0; x < stoneNoise.width; x++)
        {
            Color c = surfaceNoise.GetPixel(x, 1);
            int height = (int)(c.r * 35);
            int h;
            for (h = 0; h < height; h++)
            {
                tilemap.SetTile(new Vector3Int(x, defaultStoneHeight + h, 0), stone);
            }
        }
    }
}
