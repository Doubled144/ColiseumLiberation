using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isMouseOver : MonoBehaviour
{
    public bool over = false;
    // public Vector2 cursor_point;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // void Update()
    // {
    //     Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    //     cursor_point = Camera.main.ScreenToWorldPoint(mouse);
    // }

    void OnMouseOver()
    {
        over = true;
    }

    void OnMouseExit()
    {
        over = false;
    }
}
