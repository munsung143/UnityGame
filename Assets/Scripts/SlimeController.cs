using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Transform player;
    LayerMask layer;
    [SerializeField] private bool onGround;
    private float timer;

    private void Update()
    {
        if (onGround)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                Jump();
                timer = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onGround = true;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
    }

    public void Jump()
    {
        Vector2 dir = new Vector2(0, 2);
        if ( transform.position.x < player.position.x)
        {
            dir.x = 1;
        }
        else
        {
            dir.x = -1;
        }
        dir.Normalize();
        rigid.AddForce(dir*6, ForceMode2D.Impulse);
    }
}
