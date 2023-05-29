using System;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class API
{
    public static string SelectFile()
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.InitialDirectory =
            PlayerPrefs.GetString("SelectFilePath", "file://" + UnityEngine.Application.dataPath); //默认打开路径 
        ofd.Filter = "LinxAssetsCheck文件(*.lac)|*.lac;";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            PlayerPrefs.SetString("SelectFilePath", ofd.FileName);
            PlayerPrefs.Save();
            return ofd.FileName;
            Debug.Log(ofd.FileName);
        }

        return "";
    }

    public static void FlutterPrompt(string tip)
    {
        var go = Resources.Load<GameObject>("Prefabs/FlutterPrompt");
        var flutterPrompt = Object.Instantiate(go);
        flutterPrompt.transform.Find("Frame/Text").GetComponent<Text>().text = tip;
        Object.Destroy(flutterPrompt, 3.0f);
    }

    static public string GetSize(long byteSize)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        long prevOrderRemainder = 0;
        while (byteSize >= 1024 && order < sizes.Length - 1)
        {
            order++;
            prevOrderRemainder = byteSize % 1024;
            byteSize = byteSize / 1024;
        }

        double byteSizeFloat = (double)byteSize + (double)prevOrderRemainder / 1024;

        string result = String.Format("{0:0.##}{1}", byteSizeFloat, sizes[order]);
        return result;
    }
}