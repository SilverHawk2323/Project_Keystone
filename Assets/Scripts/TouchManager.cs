using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class TouchManager : MonoBehaviour
{
    private TouchControls touchControls;

    //delegate means we're letting another script handle the functionality
    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    public static TouchManager instance;

    private bool _touchedObject;

    private void OnEnable()
    {
        touchControls.Enable();

    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        touchControls = new TouchControls();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //ctx is the syntax for subscribing to an event and being able to pass through the information from that event to our function
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void Update()
    {
        if (!_touchedObject)
        {
            return;
        }
        if (touchControls.Touch.TouchPress.inProgress)
        {
            ContinueTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        }
    }


    private void StartTouch(InputAction.CallbackContext callbackContext)
    {
        Vector2 screenPos = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent<TestTouch>(out TestTouch touchHit))
            {
                if (OnStartTouch != null)
                {
                    _touchedObject = true;
                    OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)callbackContext.startTime);
                }
            }
        }
        Debug.Log("Touch started " + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        
    }

    private void ContinueTouch(Vector2 position)
    {
        OnStartTouch(position, Time.time);
    }

    private void EndTouch(InputAction.CallbackContext callbackContext)
    {
        if (!_touchedObject)
        {
            return;
        }
        if (OnEndTouch != null)
        {
            _touchedObject = false;
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)callbackContext.time);
            Debug.Log("Ended Touch");
        }
    }
}
