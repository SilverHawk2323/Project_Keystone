using UnityEngine;
using UnityEngine.InputSystem;


public class NewTouchManager : MonoBehaviour
{
    private TouchControls touchControls;

    public InputActionAsset inputActions;

    private InputAction _dragAction;
    private InputAction _screenPositionAction;

    private Vector2 _screenPosition;

    private static Camera _camera;
    new public static Camera camera => _camera;

    private void OnEnable()
    {
        touchControls.Enable();
        inputActions.FindActionMap("Touch").Enable();

    }

    private void OnDisable()
    {
        touchControls.Disable();
        inputActions.FindActionMap("Touch").Disable();
    }

    private void Awake()
    {
        touchControls = new TouchControls();
        _camera = Camera.main;
        _dragAction = InputSystem.actions.FindAction("TouchPress");
        _screenPositionAction = InputSystem.actions.FindAction("TouchPosition");
    }

    private void Update()
    {
        /*if (touchControls.Touch.TouchPress.triggered)
        {
            HandleTouch();
        }*/
        _screenPosition = _screenPositionAction.ReadValue<Vector2>();
        HandleTouch();
        //touchControls.Touch.TouchPress.started += ctx => HandleTouch();
    }

    public void HandleTouch()
    {
        //if (touchControls.Touch.TouchPress.triggered) return;
        Ray touchRay = _camera.ScreenPointToRay(_screenPosition);
        //Vector2 touchPosition = touchControls.Touch.TouchPosition.ReadValue<Vector2>();

        if (Physics.Raycast(touchRay, out RaycastHit hit))
        {
            if(hit.transform.TryGetComponent<I_Touchable>(out I_Touchable currentTouchable))
            {
                currentTouchable.OnTouchBegin(_screenPosition);
                print(_screenPosition);
            }
        }
    }
}
