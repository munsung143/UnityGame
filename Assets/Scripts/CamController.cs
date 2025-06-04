using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{

    [SerializeField] Camera cam;
    private float size;
    private void Update()
    {
        Zoom();
    }

    public void Zoom()
    {

        size = Mathf.Clamp(size + 0.5f * Input.mouseScrollDelta.y, 3, 30);
        cam.orthographicSize = size;
    }
}
