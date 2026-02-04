using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D clickCursor;

    public Vector2 hotSpot = Vector2.zero;

    private bool isClicking = false;

    void Start()
    {
        SetNormalCursor();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetClickCursor();
            isClicking = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetNormalCursor();
            isClicking = false;
        }
    }

    void SetNormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    void SetClickCursor()
    {
        Cursor.SetCursor(clickCursor, hotSpot, CursorMode.Auto);
    }
}
