using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private const string ISWALKING = "IsWalking";
    private const string ISSPRINTING = "IsSprinting";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(ISWALKING, false);
        animator.SetBool(ISSPRINTING, false);
    }

    private void Update()
    {
        animator.SetBool(ISWALKING, player.IsWalking);
        animator.SetBool(ISSPRINTING, player.IsSprinting);
    }
}
