using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float sprintSpeed = 15f;

    [SerializeField]
    private LayerMask counterMask;

    private Vector3 lastInteractionDir;

    public bool IsWalking { get; private set; }
    public bool IsSprinting { get; private set; }

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
                clearCounter.Interact();
            }
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
            //пробуем двинуться по X
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
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
            transform.position += speed * moveDir * Time.deltaTime;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }
}