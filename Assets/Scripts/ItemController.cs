using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] Item data;
    public Item Data { get { return data; } }
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Set(Item data)
    {
        this.data = data;
        spriteRenderer.sprite = data.Sprite;
    }
}
