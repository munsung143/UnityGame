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

        }
        else
        {
            timer += Time.deltaTime;
        }
            image.color = new Color(0, 0, 0, 4);
    }
}
