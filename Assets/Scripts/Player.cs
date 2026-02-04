using System;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("На уровне больше 1 игрока! Синглтон сломался");
        }

        Instance = this;
    }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnPlayerDashed;

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private Transform KitchenObjectPlace;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float sprintSpeedCoefficient = 1.5f;

    [SerializeField]
    private float dashSpeedCoefficient = 2f;

    [SerializeField]
    private LayerMask counterMask;

    private Vector3 lastInteractionDir;

    private BaseCounter selectedCounter;

    private KitchenObject kitchenObject;

    public bool IsWalking { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool WasDashed { get; private set; }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternativeAction += GameInput_OnInteractAlternativeAction;

        gameInput.OnSprintActionStarted += GameInput_OnSprintActionStarted;
        gameInput.OnSprintActionCanceled += GameInput_OnSprintActionCanceled;

        gameInput.OnDashAction += GameInput_OnDashAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    /// <summary>
    /// обработчик взаимодействий игрока с другими объектами
    /// </summary>
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if(moveDir !=  Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }

        float interactDist = 2f;

        if (Physics.Raycast(transform.position, lastInteractionDir, 
            out RaycastHit raycastHit, interactDist, counterMask))
        {
           if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
           {
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
           }
           else
           {
                SetSelectedCounter(null);
           }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    /// <summary>
    /// обработчик движения игрока
    /// </summary>
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDist = speed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDir, moveDist);

        IsWalking = moveDir != Vector3.zero;

        if (!canMove)
        {
            //пробуем двинуться по X
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirX, moveDist);

            if (canMove)
            {
                //можем двигаться только по X
                moveDir = moveDirX;
            }
            else
            {
                //по X нельзя => пробуем по Z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirZ, moveDist);

                if (canMove)
                {
                    //можно двигаться только по Z
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            float speedModifier = IsSprinting ? sprintSpeedCoefficient : 1;

            //проверка на бег, тк был баг с тем, что во время бега издалека
            //игрок проходил сквозь объекты
            //(типо игрок уже бежал какое-то время до объекта с расстояния)
            if (IsSprinting)
            {
                float sprintDist = speed * Time.deltaTime * speedModifier;

                bool canSprint = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                    playerRadius, moveDir, sprintDist);

                if (!canSprint)
                {
                    speedModifier = 1;
                }
            }

            if (WasDashed)
            {
                OnPlayerDashed?.Invoke(this, EventArgs.Empty);

                //проверка на то, идет ли игрок, чтобы не было нулевого moveDir
                Vector3 moving = IsWalking ? moveDir : transform.forward;

                //аналогичная проверка на рывок, чтобы не проходить сквозь объекты
                float dashDist = speed * Time.deltaTime * dashSpeedCoefficient * speedModifier;

                bool canDash = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                    playerRadius, moving, dashDist);

                if (canDash)
                {
                    transform.position += speed * moving * Time.deltaTime * dashSpeedCoefficient * speedModifier;
                }
                else
                {
                    transform.position += speed * moveDir * Time.deltaTime * speedModifier;
                }

                WasDashed = false;
            }
            else
            {
                transform.position += speed * moveDir * Time.deltaTime * speedModifier;
            }
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternativeAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternative(this);
        }
    }

    private void GameInput_OnSprintActionStarted(object sender, EventArgs e)
    {
        IsSprinting = true;
    }
    private void GameInput_OnSprintActionCanceled(object sender, EventArgs e)
    {
        IsSprinting = false;
    }

    private void GameInput_OnDashAction(object sender, EventArgs e)
    {
        WasDashed = true;
    }

    /// <summary>
    /// принимает текущий counter, с которым было взаимодействие и переводит его в состояние выбранного игроком
    /// </summary>
    /// <param name="clearCounter"> counter, на который смотрит игрок </param>
    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        this.selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }

    /// <summary>
    /// получение местоположения кухонного объекта "в руках" игрока
    /// </summary>
    /// <returns> местоположение кухонного объекта </returns>
    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectPlace;
    }

    /// <summary>
    /// принимает кухонный объект, который взял игрок "в руки", и задает его значение как текущее 
    /// </summary>
    /// <param name="kitchenObject"> кухонный объект </param>
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    /// <summary>
    /// получение кухонного объекта "в руках" игрока
    /// </summary>
    /// <returns> кухонный объект </returns>
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    /// <summary>
    /// удаление кухонного объекта "из рук" игрока
    /// </summary>
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    /// <summary>
    /// проверяет, есть ли у игрока "в руках" кухонный объект
    /// </summary>
    /// <returns> есть ли кухонный объект </returns>
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}