using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFileLogic : MonoBehaviour
{
    public static SelectFileLogic Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectFileA()
    {
        ReportModel.Instance.ReportDataA = ReportData.SelectDataFromFile();
    }

    public void SelectFileB()
    {
        ReportModel.Instance.ReportDataB = ReportData.SelectDataFromFile();
    }
}