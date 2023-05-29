using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject reportPanel;
    [SerializeField] private Animator animator;
    public static UILogic Instance;
    private static readonly int s_State = Animator.StringToHash("State");

    private void Awake()
    {
        Instance = this;
    }

    public void JumpToSelectPanel()
    {
        animator.SetInteger(s_State, 0);
    }

    public void JumpToReportPanel()
    {
        animator.SetInteger(s_State, 1);
    }
}