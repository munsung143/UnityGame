using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/Items", order = 6)]
public class Items : ScriptableObject
{
    [SerializeField] public Item dirt;
    [SerializeField] public Item stone;
    [SerializeField] public Item wood;
    [SerializeField] public Item pickaxe;
}
