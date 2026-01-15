using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    public bool IsWalking { get; private set; }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        IsWalking = moveDir != Vector3.zero;

        transform.position += speed * moveDir * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }
}
