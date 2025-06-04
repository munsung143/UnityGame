using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "Tiles", menuName = "ScriptableObjects/Tiles", order = 5)]

public class Tiles : ScriptableObject
{
    [SerializeField] public TileBase grass;
    [SerializeField] public TileBase dirt;
    [SerializeField] public TileBase stone;
    [SerializeField] public TileBase wood;
    [SerializeField] public TileBase leaves;
}
