using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMouseCursor : MonoBehaviour
{
    public Texture2D mouseCursor;

    private Vector2 hotSpot;

    CursorMode cursorMode = CursorMode.Auto;

    private void Start()
    {
        hotSpot = new Vector2(mouseCursor.width / 2, mouseCursor.height / 2);
        Cursor.SetCursor(mouseCursor, hotSpot, cursorMode);
    }
}