using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTextureComponent : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture; 
    [SerializeField] private Vector2 hotspot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
