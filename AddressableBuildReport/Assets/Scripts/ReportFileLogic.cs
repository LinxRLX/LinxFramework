using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReportFileLogic : MonoBehaviour
{
    public static ReportFileLogic Instance;

    private void Awake()
    {
        Instance = this;
    }
}