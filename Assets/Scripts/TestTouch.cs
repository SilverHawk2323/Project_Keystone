using UnityEngine;

public class TestTouch : MonoBehaviour
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
}
