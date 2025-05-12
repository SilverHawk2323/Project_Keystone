using UnityEngine;

public class TestTouch : MonoBehaviour, I_Touchable
{

    public TouchManager _TouchManager;

    public Camera mainCamera;

    private void Awake()
    {
        if (mainCamera == null && Camera.main != null)
        {
            mainCamera = Camera.main;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _TouchManager = TouchManager.instance;
    }

    private void OnEnable()
    {
        //we're subscribing to this event
        _TouchManager.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        _TouchManager.OnEndTouch -= Move;
    }


    public void Move(Vector2 screenPosition, float time)
    {
        Vector3 screenCoordinates = new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane);
        Vector3 worldCoordinates = mainCamera.ScreenToWorldPoint(screenCoordinates);
        worldCoordinates.z = 0;
        transform.position = worldCoordinates;
    }

    public void OnTouchBegin(Vector2 touchPosition)
    {
        Vector3 screenCoordinates = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane);
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(screenCoordinates);

        newPosition.z = 0;

        transform.position = newPosition;
    }

    public void OnTouchStay(Vector2 touchPosition)
    {
        
    }

    public void OnTouchEnd(Vector2 touchPosition)
    {
        
    }
}
