using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public SampleSO sampleSO;
    public Tilemap tilemap;
    public TileBase tile;

    private void Awake()
    {
        Debug.Log(sampleSO.a);
        tilemap.SetTile(new Vector3Int(3,4,5), tile);

    }
}
