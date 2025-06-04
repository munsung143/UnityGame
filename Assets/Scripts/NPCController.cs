using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;

    private float waiting;
    private float directionChange;
    private float untilWaiting;
    private bool isGotoLeft;

    private bool isWaiting;

    private float untilWaitingCount;
    private float waitingCount;
    private float directionChangeCount;

    private void Start()
    {
        isWaiting = false;
        SetValues();
    }
    private void Update()
    {
        if (!isWaiting)
        {
            if (isGotoLeft)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
            untilWaitingCount += Time.deltaTime;
            directionChangeCount += Time.deltaTime;
            if (untilWaitingCount > untilWaiting)
            {
                untilWaitingCount = 0;
                isWaiting = true;
            }
            if (directionChangeCount > directionChange)
            {
                directionChangeCount = 0;
                isGotoLeft = !isGotoLeft;
            }
        }
        else
        {
            waitingCount += Time.deltaTime;
            if (waitingCount > waiting)
            {
                SetValues();
                waitingCount = 0;
                isWaiting = false;
            }
        }
    }
    public void MoveLeft()
    {
        if (Mathf.Abs(rigid.velocity.x) < speed)
        {
            rigid.AddForce(new Vector2(-10f, 0f));
        }
        spriteRenderer.flipX = false;
    }
    public void MoveRight()
    {
        if (Mathf.Abs(rigid.velocity.x) < speed)
        {
            rigid.AddForce(new Vector2(10f, 0f));
        }
        spriteRenderer.flipX = true;
    }

    public void SetValues()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            isGotoLeft = true;
        }
        else
        {
            isGotoLeft = false;
        }
        untilWaiting = Random.Range(2f, 8f);
        waiting = Random.Range(2f, 6f);
        directionChange = Random.Range(4f, 6f);
    }
}
