using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StructGenerator : MonoBehaviour
{
    [SerializeField] int treeCount;
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] Tiles tiles;
    
    [SerializeField] TileBase wood => tiles.wood;
    [SerializeField] TileBase leaves => tiles.leaves;

    public void GenTrees()
    {
        Vector2Int[] treePoses = GetTreePos();
        foreach (var treePo in treePoses)
        {
            GrowTree(treePo);
        }
    }
    public void GrowTree(Vector2Int pos)
    {
        int woodHeight = Random.Range(5, 15);
        int i;
        for (i = 0; i < woodHeight; i++)
        {
            mapGenerator.Tilemap.SetTile(new Vector3Int(pos.x, pos.y + i, 0), wood);
        }
        mapGenerator.Tilemap.SetTile(new Vector3Int(pos.x, pos.y + i, 0), leaves);
    }
    public Vector2Int[] GetTreePos()
    {
        int treeCount = Random.Range(15, 30);
        Vector2Int[] poses = new Vector2Int[treeCount];
        for (int i = 0; i < poses.Length; i++)
        {
            int temp;
            do
            {
                temp = Random.Range(0, mapGenerator.MapWidth);
            } while (poses.Contains(new Vector2Int(temp, 0)));
            poses[i].x = temp;
        }

        for (int i = 0; i < poses.Length; i++)
        {
            int temp = mapGenerator.MapHeight;
            while (mapGenerator.Tilemap.GetTile(new Vector3Int(poses[i].x, temp, 0)) != tiles.grass)
            {
                temp--;
            }
            poses[i].y = temp + 1;
        }

        return poses;
    }
}
