using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternativeAction;

    public event EventHandler OnSprintActionStarted;
    public event EventHandler OnSprintActionCanceled;

    public event EventHandler OnDashAction;

    public event EventHandler OnPauseAction;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("больше 1 gameInput");
        }

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_perfomed;
        inputActions.Player.InteractAlternative.performed += InteractAlternative_perfomed;

        inputActions.Player.Sprint.started += Sprint_started;
        inputActions.Player.Sprint.canceled += Sprint_canceled;

        inputActions.Player.Dash.performed += Dash_performed;

        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_perfomed;
        inputActions.Player.InteractAlternative.performed -= InteractAlternative_perfomed;

        inputActions.Player.Sprint.started -= Sprint_started;
        inputActions.Player.Sprint.canceled -= Sprint_canceled;

        inputActions.Player.Dash.performed -= Dash_performed;

        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
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
