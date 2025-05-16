using UnityEngine;
using UnityEngine.InputSystem;


public class NewControlManager : MonoBehaviour
{
    

    public InputActionAsset inputActions;

    private InputAction _dragAction;
    private InputAction _screenPositionAction;

    private Vector2 _screenPosition;
    I_Touchable currentTouchable;
    private static Camera _camera;
    new public static Camera camera => _camera;

    private void OnEnable()
    {
        
        inputActions.FindActionMap("Touch").Enable();

    }

    private void OnDisable()
    {
        
        inputActions.FindActionMap("Touch").Disable();
    }

    private void Awake()
    {
        
        _camera = Camera.main;
        _dragAction = InputSystem.actions.FindAction("TouchPress");
        _screenPositionAction = InputSystem.actions.FindAction("TouchPosition");
        
    }

    private void Update()
    {
        
        _screenPosition = _screenPositionAction.ReadValue<Vector2>();
        if (currentTouchable != null)
        {
            if (!_dragAction.inProgress)
            {
                ReleasedTouch();
            }
            //else if (_dragAction.WasPressedThisFrame())
            //{
            //    currentTouchable.OnTouchBegin(_screenPosition);
            //}
            else// if (_dragAction.inProgress)
            {
                currentTouchable.OnTouchStay(_screenPosition);
            }
        }
        else if (_dragAction.inProgress)//currentTouchable == null)
        {
            HandleTouch();
        }
    }

    private void ReleasedTouch()
    {
        currentTouchable.OnTouchEnd(_screenPosition);
        currentTouchable = null;
    }

    public void HandleTouch()
    {
        Ray touchRay = _camera.ScreenPointToRay(_screenPosition);

        if (Physics.Raycast(touchRay, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent<I_Touchable>(out currentTouchable))
            {
                currentTouchable.OnTouchBegin(_screenPosition, hit.distance);
                //print(_screenPosition);
            }
        }
    }
}
