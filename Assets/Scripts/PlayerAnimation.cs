using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private const string ISWALKING = "IsWalking";
    private const string ISSPRINTING = "IsSprinting";
    private const string WASDASHING = "WasDashed";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetBool(ISWALKING, false);
        animator.SetBool(ISSPRINTING, false);
    }

    private void Start()
    {
        player.OnPlayerDashed += Player_OnPlayerDashed;
    }

    private void Player_OnPlayerDashed(object sender, System.EventArgs e)
    {
        animator.SetTrigger(WASDASHING);
    }

    private void Update()
    {
        animator.SetBool(ISWALKING, player.IsWalking);
        animator.SetBool(ISSPRINTING, player.IsSprinting);
    }
}
