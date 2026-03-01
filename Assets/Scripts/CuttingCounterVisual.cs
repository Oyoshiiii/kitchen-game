using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField]
    private CuttingCounter cuttingCounter;

    private const string Cut = "Cut";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CuttingCounter.OnProductsCut += CuttingCounter_OnProductsCut;
    }

    private void CuttingCounter_OnProductsCut(object sender, EventArgs e)
    {
        animator.SetTrigger(Cut);
    }
}
