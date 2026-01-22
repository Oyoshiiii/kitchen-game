using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Ќа уровне больше 1 игрока! —инглтон сломалс€");
        }

        Instance = this;
    }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float sprintSpeedCoefficient = 1.5f;

    [SerializeField]
    private LayerMask counterMask;

    private Vector3 lastInteractionDir;

    private ClearCounter selectedCounter;

    public bool IsWalking { get; private set; }
    public bool IsSprinting { get; private set; }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnSprintActionStarted += GameInput_OnSprintActionStarted;
        gameInput.OnSprintActionCanceled += GameInput_OnSprintActionCanceled;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

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
           if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
           {
                if(clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
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
            //пробуем двинутьс€ по X
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirX, moveDist);

            if (canMove)
            {
                //можем двигатьс€ только по X
                moveDir = moveDirX;
            }
            else
            {
                //по X нельз€ => пробуем по Z
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
                playerRadius, moveDirZ, moveDist);

                if (canMove)
                {
                    //можно двигатьс€ только по Z
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            float speedModifier = IsSprinting ? sprintSpeedCoefficient : 1;
            transform.position += speed * moveDir * Time.deltaTime * speedModifier;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact();
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

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        this.selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = this.selectedCounter
        });
    }
}