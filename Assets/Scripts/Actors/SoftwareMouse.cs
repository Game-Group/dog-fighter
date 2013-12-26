using UnityEngine;
using System.Collections;

public class SoftwareMouse : MonoBehaviour 
{
    public float Sensitivity;

    public Vector2 ScreenPosition { get { return screenPosition; } }

    public Texture2D Cursor;
    public bool ShowCursor;

    private float xAxis;
    private float yAxis;

    private Vector2 screenPosition;

    void Start () 
    {
        Screen.lockCursor = true;
        xAxis = Screen.width / 2;
        yAxis = Screen.height / 2;

        CalculateNewPosition();
	}

    void Update()
    {
        CalculateNewPosition();
    }

    void OnGUI()
    {
        if (ShowCursor)
            GUI.Label(
                new Rect(screenPosition.x - Cursor.width / 2,
                         screenPosition.y - Cursor.height / 2,
                         Cursor.width, 
                         Cursor.height), 
                new GUIContent(Cursor));
    }

    private void CalculateNewPosition()
    {
        xAxis += Input.GetAxis("Mouse X") * Time.deltaTime * Sensitivity;
        yAxis -= Input.GetAxis("Mouse Y") * Time.deltaTime * Sensitivity;

        xAxis = Mathf.Clamp(xAxis, 0, Screen.width);
        yAxis = Mathf.Clamp(yAxis, 0, Screen.height);

        screenPosition = new Vector2(xAxis, yAxis);
    }
}
