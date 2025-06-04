using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;

    [SerializeField] InventoryManager inventory;
    [SerializeField] Tiles tiles;
    [SerializeField] Items items;
    [SerializeField] GameObject item;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(rigid.velocity.x) < speed)
        {
            rigid.AddForce(new Vector2(x * 10f, 0f));
        }
        if (x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            GetItem(collision.gameObject.GetComponent<ItemController>());
        }
    }

    public void GetItem(ItemController item)
    {
        bool canAddItem = inventory.AddItem(item.Data);
        if (canAddItem)
        {
            Destroy(item.gameObject);
        }
    }

    public void UseItem()
    {
        if (inventory.CurrenItem is BlockItem)
        {
            Place();
        }
        else if (inventory.CurrenItem is Tool)
        {
            Break();
        }
    }
    public void Break()
    {
        Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickedTilePos = tilemap.WorldToCell(clickedPos);
        TileBase clickedTile = tilemap.GetTile(clickedTilePos);
        GameObject i = null;
        if (clickedTile != null)
        {
            i = Instantiate(item);
            i.transform.position = clickedTilePos;
        }
        if (clickedTile == tiles.stone)
        {
            i.GetComponent<ItemController>().Set(items.stone);
        }
        else if (clickedTile == tiles.wood)
        {
            i.GetComponent<ItemController>().Set(items.wood);
        }
        else if (clickedTile == tiles.grass || clickedTile == tiles.dirt)
        {
            i.GetComponent<ItemController>().Set(items.dirt);
        }

        tilemap.SetTile(clickedTilePos, null);
    }

    public void Place()
    {
        Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickedTilePos = tilemap.WorldToCell(clickedPos);
        if (tilemap.GetTile(clickedTilePos) != null)
        {
            return;
        }
        TileBase toPlace = null;
        if (inventory.CurrenItem == items.dirt)
        {
            toPlace = tiles.dirt;
        }
        else if (inventory.CurrenItem == items.wood)
        {
            toPlace = tiles.wood;
        }
        else if (inventory.CurrenItem == items.stone)
        {
            toPlace = tiles.stone;
        }
        inventory.Consume();
        tilemap.SetTile(clickedTilePos, toPlace);
    }

}
