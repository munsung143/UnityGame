using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap tagmap;
    [SerializeField] TileBase tag;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;
    [SerializeField] int hp;
    [SerializeField] InventoryManager inventory;
    [SerializeField] Tiles tiles;
    [SerializeField] Items items;
    [SerializeField] GameObject item;

    private bool onGround;
    private Vector3Int beforeTagPos;

    private void Update()
    {
        if (CheckRange())
        {
            ViewTag();
        }
        else
        {
            tagmap.SetTile(beforeTagPos, null);
        }
        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                Jump();
            }
        }
    }

    public void ViewTag()
    {
        tagmap.SetTile(beforeTagPos, null);
        Vector3Int tagPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        tagmap.SetTile(tagPos, tag);
        beforeTagPos = tagPos;
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


    private void OnCollisionStay2D(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y >= 0.99f)
            {
                onGround = true;
                return;
            }
        }
        onGround = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false ;
    }

    public void Jump()
    {
        rigid.AddForce(Vector2.up * 7, ForceMode2D.Impulse);
    }

    public void GetItem(ItemController item)
    {
        bool canAddItem = inventory.AddItem(item.Data);
        //Debug.Log(1);
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
    public bool CheckRange()
    {
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (p.magnitude < 5)
        {
            return true;
        }
        return false;
    }
    public void Break()
    {
        if (!CheckRange())
        {
            return;
        }
        Vector3 clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickedTilePos = tilemap.WorldToCell(clickedPos);
        TileBase clickedTile = tilemap.GetTile(clickedTilePos);
        GameObject i = null;
        if (clickedTile != null)
        {
            i = Instantiate(item);
            i.transform.position = clickedTilePos + new Vector3(0.5f,0.5f,0);
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
        if (!CheckRange())
        {
            return;
        }
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
