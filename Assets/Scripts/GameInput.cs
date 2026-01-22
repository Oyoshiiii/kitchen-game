using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;

    public event EventHandler OnSprintActionStarted;
    public event EventHandler OnSprintActionCanceled;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_perfomed;

        inputActions.Player.Sprint.started += Sprint_started;
        inputActions.Player.Sprint.canceled += Sprint_canceled;
    }

    //взаимодействие
    private void Interact_perfomed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    //бег
    private void Sprint_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintActionStarted?.Invoke(this, EventArgs.Empty);
    }
    private void Sprint_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintActionCanceled?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
   {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
   }
}
