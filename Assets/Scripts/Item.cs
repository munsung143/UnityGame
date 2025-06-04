using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 3)]
public class Item : ScriptableObject
{
    [SerializeField] Sprite _sprite;
    public Sprite Sprite {  get { return _sprite; } }
    [SerializeField] string _description;
    public string Description { get { return _description; } }
    [SerializeField] int _maxAmount;
    public int MaxAmount { get { return _maxAmount; } }
}
