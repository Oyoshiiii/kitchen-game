using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float rotationSpeed = 10f;

    public bool IsWalking { get; private set; }

    private void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);

        //GetKey - удержание клавиши
        //GetKeyDown - отпускание клавиши
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        IsWalking = moveDir != Vector3.zero;

        transform.position += speed * moveDir * Time.deltaTime;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime); //интерпол€ци€, присуща€ 3д объектам; Lerp - интерпол€ци€ между двум€ точками
    }
}
