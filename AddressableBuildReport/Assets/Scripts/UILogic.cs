using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject reportPanel;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fullTextUI;
    [SerializeField] private Text textFull;
    [SerializeField] private string textShowFullTarget;

    public static UILogic Instance;
    private static readonly int s_State = Animator.StringToHash("State");

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(textShowFullTarget))
        {
            textFull.text = textShowFullTarget;
            fullTextUI.SetActive(true);
        }
        else
        {
            textFull.text = "";
            fullTextUI.SetActive(false);
            fullTextUI.transform.position = Vector3.down * 3000;
        }
    }

    public void JumpToSelectPanel()
    {
        animator.SetInteger(s_State, 0);
    }

    public void JumpToReportPanel()
    {
        animator.SetInteger(s_State, 1);
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }

    public void ShowFullText(string fullText)
    {
        textShowFullTarget = fullText;
    }

    public void HideFullText()
    {
        textShowFullTarget = null;
    }
}