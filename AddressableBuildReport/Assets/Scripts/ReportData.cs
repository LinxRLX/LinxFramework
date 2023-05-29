using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ReportData
{
    [SerializeField] public List<string> AllBundles = new List<string>();
    [SerializeField] public List<long> AllBundlesSize = new List<long>();
    [SerializeField] public List<string> NameList = new List<string>();
    [SerializeField] public List<string> HashList = new List<string>();

    private Dictionary<string, string> m_assetsDic;
    private Dictionary<string, long> m_bundlesDic;

    public long BundleSize => AllBundlesSize.Sum();
    public long BundleCnt => AllBundles.Count;
    public long AssetsCnt => NameList.Count;

    public Dictionary<string, string> GetAssetsDic()
    {
        if (m_assetsDic != null) return m_assetsDic;
        m_assetsDic = new Dictionary<string, string>();

        for (int i = 0; i < NameList.Count; i++)
        {
            m_assetsDic.Add(NameList[i], HashList[i]);
        }

        return m_assetsDic;
    }

    public Dictionary<string, long> GetBundlesDic()
    {
        if (m_bundlesDic != null) return m_bundlesDic;
        m_bundlesDic = new Dictionary<string, long>();

        for (int i = 0; i < AllBundles.Count; i++)
        {
            m_bundlesDic.Add(AllBundles[i], AllBundlesSize[i]);
        }

        return m_bundlesDic;
    }

    public static ReportData SelectDataFromFile()
    {
        var selectPath = API.SelectFile();
        Debug.Log(selectPath);
        if (string.IsNullOrEmpty(selectPath))
        {
            API.FlutterPrompt("文件为空");
            return null;
        }

        // 检查文件
        try
        {
            var text = File.ReadAllText(selectPath);
            var report = JsonUtility.FromJson<ReportData>(text);
            if (report == null ||
                report.HashList == null || report.NameList == null ||
                report.NameList.Count != report.HashList.Count ||
                report.AllBundles == null || report.AllBundlesSize == null ||
                report.AllBundlesSize.Count != report.AllBundles.Count)
            {
                API.FlutterPrompt("文件不合法");
                return null;
            }

            return report;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            API.FlutterPrompt("文件不合法");
        }

        return null;
    }
}