using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "BlockItem", menuName = "ScriptableObjects/BlockItem", order = 4)]
public class BlockItem : Item
{
    [SerializeField] TileBase tile;
}
