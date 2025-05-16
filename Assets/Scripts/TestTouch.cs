using UnityEngine;

public class TestTouch : MonoBehaviour, I_Touchable
{

    public Camera mainCamera;

    private void Awake()
    {
        if (mainCamera == null && Camera.main != null)
        {
            mainCamera = Camera.main;
        }
    }

    public void OnTouchBegin(Vector2 touchPosition, float distance)
    {
        print("Started Touch");
    }

    public void OnTouchStay(Vector2 touchPosition)
    {
        Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane);
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenCoordinates);

        newPosition.z = 0;

        transform.position = newPosition;
    }

    public void OnTouchEnd(Vector2 touchPosition)
    {
        print("Ended touch at " + touchPosition);
    }
}
