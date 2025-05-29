using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinTest : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public int seed;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    public Texture2D noiseTex;
    private Color[] pix;
    public TileBase tile;
    public Tilemap tilemap;
    public TilePaletteElement palette;
    void Start()
    {
        //rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        CalcNoise();
        Test();
    }
    void CalcNoise()
    {
        // For each pixel in the texture...
        for (float y = 0.0F; y < noiseTex.height; y++)
        {
            for (float x = 0.0F; x < noiseTex.width; x++)
            {
                float xCoord = seed + x / noiseTex.width * scale;
                float yCoord = seed + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
            }
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
    void Test()
    {
    }
}
