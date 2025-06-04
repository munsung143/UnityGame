using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float dayCycleSpeed;

    private float timer;
    private bool isDecrease;

    private void Update()
    {
        if (isDecrease)
        {
            timer -= dayCycleSpeed * Time.deltaTime;
        }
        else
        {
            timer += dayCycleSpeed * Time.deltaTime;
        }
        image.color = new Color(0f, 0f, 0f, timer/1000);
        if (image.color.a >= 0.9)
        {
            isDecrease = true;
        }
        if (image.color.a <= 0)
        {
            isDecrease = false;
        }

    }
}
