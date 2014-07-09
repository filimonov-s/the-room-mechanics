using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour
{
    public static float MouseSpeedX;
    public static float MouseSpeedY;

    private Vector2 _prevMousePosition;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseSpeedX = 0;
            MouseSpeedY = 0;
            _prevMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            MouseSpeedX = _prevMousePosition.x - Input.mousePosition.x;
            MouseSpeedY = _prevMousePosition.y - Input.mousePosition.y;
            _prevMousePosition = Input.mousePosition;
        }
    }
}