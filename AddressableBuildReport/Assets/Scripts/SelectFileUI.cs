using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectFileUI : MonoBehaviour
{
    [SerializeField] private Button btnSelectA;
    [SerializeField] private Button btnSelectB;
    [SerializeField] private Button btnGoAhead;
    private readonly string m_normalTextPath = "Normal/Text";
    private readonly string m_highlightedTextPath = "Highlighted/Text";
    private Text m_textNormalA;
    private Text m_textNormalB;
    private Text m_textHighlightedA;
    private Text m_textHighlightedB;

    private void Awake()
    {
        btnSelectA.onClick.AddListener(() => { SelectFileLogic.Instance.SelectFileA(); });
        btnSelectB.onClick.AddListener(() => { SelectFileLogic.Instance.SelectFileB(); });
        btnGoAhead.onClick.AddListener(() => { UILogic.Instance.JumpToReportPanel(); });
        m_textNormalA = btnSelectA.transform.Find(m_normalTextPath).GetComponent<Text>();
        m_textNormalB = btnSelectB.transform.Find(m_normalTextPath).GetComponent<Text>();
        m_textHighlightedA = btnSelectA.transform.Find(m_highlightedTextPath).GetComponent<Text>();
        m_textHighlightedB = btnSelectB.transform.Find(m_highlightedTextPath).GetComponent<Text>();
    }

    private void Update()
    {
        // btnSelectA文本
        m_textHighlightedA.text = m_textNormalA.text = ReportModel.Instance.IsSelectA ? "重选当前" : "选择当前";
        // btnSelectB文本
        m_textHighlightedB.text = m_textNormalB.text = ReportModel.Instance.IsSelectB ? "重选旧版本" : "选择旧版本";
        // btnGoAhead状态
        btnGoAhead.gameObject.SetActive(ReportModel.Instance.IsReady);
    }
}