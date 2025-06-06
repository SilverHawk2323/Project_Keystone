using UnityEngine;
using UnityEngine.InputSystem;


public class NewControlManager : MonoBehaviour
{
    

    public InputActionAsset inputActions;

    private InputAction _dragAction;
    private InputAction _screenPositionAction;
    private InputAction _pauseAction;
    private GameState _lastGameState;
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
        _pauseAction = InputSystem.actions.FindAction("Pause");
        
    }

    private void Update()
    {
        if (_pauseAction.WasPressedThisFrame())
        {
            PauseGame();
            Debug.Log("Pause Game");
        }
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

    public void PauseGame()
    {
        if (GameManager.gm.pauseMenu.activeInHierarchy)
        {
            GameManager.gm.pauseMenu.SetActive(false);
            GameManager.gm.state = _lastGameState;
        }
        else
        {
            _lastGameState = GameManager.gm.state;
            GameManager.gm.state = GameState.Pause;
        }
            
    }
}
