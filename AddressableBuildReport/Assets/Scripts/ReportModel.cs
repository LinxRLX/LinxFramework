using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReportModel : MonoBehaviour
{
    public static ReportModel Instance;
    public bool IsSelectA => (ReportDataA != null);
    public bool IsSelectB => (ReportDataB != null);
    public bool IsReady => (ReportDataA != null && ReportDataB != null);
    public ReportData ReportDataA;
    public ReportData ReportDataB;

    public List<string> AssetsAddList { get; private set; }
    public List<string> AssetsChgList { get; private set; }
    public List<string> AssetsDelList { get; private set; }

    public Dictionary<string, long> BundlesDic { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ContainInit()
    {
        // 清空缓存
        AssetsAddList = new List<string>();
        AssetsChgList = new List<string>();
        AssetsDelList = new List<string>();

        BundlesDic = new Dictionary<string, long>();

        if (ReportDataA == null || ReportDataB == null) return;

        // 处理资源 
        InitAssets();
        // 处理bundles
        InitBundles();
    }

    public int AssetsTotalChgCnt()
    {
        return AssetsAddList.Count + AssetsChgList.Count + AssetsDelList.Count;
    }

    public long BundlesTotalChgSize()
    {
        return BundlesDic.Sum(bundleInfo => bundleInfo.Value);
    }

    private void InitBundles()
    {
        var localDic = ReportDataA.GetBundlesDic();
        var containDic = ReportDataB.GetBundlesDic();
        // 遍历出旧包中没有的Bundle
        foreach (var bundleInfo in localDic)
        {
            if (!containDic.ContainsKey(bundleInfo.Key))
            {
                BundlesDic.Add(bundleInfo.Key, bundleInfo.Value);
            }
        }
    }

    private void InitAssets()
    {
        var localDic = ReportDataA.GetAssetsDic();
        var containDic = ReportDataB.GetAssetsDic();
        // 先遍历当前，如果在对比文件中不存在则为增加的文件，如果hash值不同则为改变的文件
        foreach (var assetInfo in localDic)
        {
            containDic.TryGetValue(assetInfo.Key, out var hashInContain);
            if (hashInContain == null)
            {
                AssetsAddList.Add(assetInfo.Key);
            }
            else if (hashInContain != assetInfo.Value)
            {
                AssetsChgList.Add(assetInfo.Key);
            }
        }

        // 遍历对比的，如果在本地文件中不存在则为删除的文件
        foreach (var assetInfo in containDic)
        {
            localDic.TryGetValue(assetInfo.Key, out var hashInLocal);
            if (hashInLocal == null)
            {
                AssetsDelList.Add(assetInfo.Key);
            }
        }
    }
}