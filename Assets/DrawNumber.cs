using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNumber : MonoBehaviour
{
    private static int Counter;
    private int index;
    private void Awake()
    {
        index = Counter++;
    }

    private void OnGUI()
    {
        var position = Camera.main.WorldToScreenPoint(transform.position);
        position.y = Screen.height - position.y;
        var rect = new Rect(new Vector2(position.x, position.y) - Vector2.up * 30, Vector2.one * 350);
        GUI.Label(rect, index.ToString());
    }
}
