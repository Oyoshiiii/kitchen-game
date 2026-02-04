using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternativeAction;

    public event EventHandler OnSprintActionStarted;
    public event EventHandler OnSprintActionCanceled;

    public event EventHandler OnDashAction;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_perfomed;
        inputActions.Player.InteractAlternative.performed += InteractAlternative_perfomed;

        inputActions.Player.Sprint.started += Sprint_started;
        inputActions.Player.Sprint.canceled += Sprint_canceled;

        inputActions.Player.Dash.performed += Dash_performed;
    }

    //взаимодействие (E)
    private void Interact_perfomed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    //взаимодействие alternative (F)
    private void InteractAlternative_perfomed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternativeAction?.Invoke(this, EventArgs.Empty);
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

    //дэш (рывок в сторону)
    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// нормализует вектор движения игрока
    /// </summary>
    /// <returns> нормализованный вектор движения </returns>
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
