using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    public static event Action TouchStart;
    public static event Action<Vector2> TouchPosition;
    public static event Action TouchStop;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchStarted;
        touchPressAction.canceled += TouchStopped;
        touchPositionAction.performed += TouchPositioned;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchStarted;
        touchPressAction.canceled -= TouchStopped;
        touchPositionAction.performed -= TouchPositioned;
    }

    private void TouchStopped(InputAction.CallbackContext context)
    {
        TouchStop?.Invoke();
    }


    private void TouchStarted(InputAction.CallbackContext context)
    {
        TouchStart?.Invoke();
    }

    private void TouchPositioned(InputAction.CallbackContext context)
    {
        TouchPosition?.Invoke(context.ReadValue<Vector2>());
    }
}
