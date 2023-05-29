using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    [SerializeField] private Text textAssetsTitle;
    [SerializeField] private Text textBundlesTitle;
    [SerializeField] private Transform groupAdd;
    [SerializeField] private Transform groupChg;
    [SerializeField] private Transform groupDel;
    [SerializeField] private Transform groupBundle;
    [SerializeField] private Button btnBack;

    [SerializeField] private GameObject prefabAssetsListItem;
    [SerializeField] private GameObject prefabBundleListItem;

    private void Awake()
    {
        btnBack.onClick.AddListener(() => { UILogic.Instance.JumpToSelectPanel(); });
    }

    private void OnEnable()
    {
        // 初始化数据
        ReportModel.Instance.ContainInit();
        // 处理资源
        AttachAssetsAdd();
        AttachAssetsChg();
        AttachAssetsDel();
        // 处理Bundles
        AttachBundles();
        // 资源数量
        var assetCnt = ReportModel.Instance.AssetsTotalChgCnt();
        textAssetsTitle.text = $"资源对比({assetCnt}个文件变化)";
        // Bundle总大小
        var bundleCnt = ReportModel.Instance.BundlesDic.Count;
        var bundleSize = ReportModel.Instance.BundlesTotalChgSize();
        textBundlesTitle.text = $"Bundle对比({bundleCnt}个文件,共{API.GetSize(bundleSize)})";
    }

    private void AttachBundles()
    {
        foreach (var bundleInfo in ReportModel.Instance.BundlesDic)
        {
            var go = GameObject.Instantiate(prefabBundleListItem, groupBundle);
            var textName = go.transform.Find("TextName").GetComponent<Text>();
            var textSize = go.transform.Find("TextSize").GetComponent<Text>();
            textName.text = bundleInfo.Key;
            textSize.text = API.GetSize(bundleInfo.Value);
        }
    }

    private void AttachAssetsAdd()
    {
        foreach (var assetName in ReportModel.Instance.AssetsAddList)
        {
            var go = GameObject.Instantiate(prefabAssetsListItem, groupAdd);
            var textAdd = go.GetComponentInChildren<Text>();
            textAdd.text = assetName;
        }
    }

    private void AttachAssetsChg()
    {
        foreach (var assetName in ReportModel.Instance.AssetsChgList)
        {
            var go = GameObject.Instantiate(prefabAssetsListItem, groupChg);
            var textChg = go.GetComponentInChildren<Text>();
            textChg.text = assetName;
        }
    }

    private void AttachAssetsDel()
    {
        foreach (var assetName in ReportModel.Instance.AssetsDelList)
        {
            var go = GameObject.Instantiate(prefabAssetsListItem, groupDel);
            var textDel = go.GetComponentInChildren<Text>();
            textDel.text = assetName;
        }
    }

    private void OnDisable()
    {
        textAssetsTitle.text = "资源对比";
        textBundlesTitle.text = "Bundle对比";
        KillAllChild(groupAdd).KillAllChild(groupChg).KillAllChild(groupDel).KillAllChild(groupBundle);
    }

    private ReportUI KillAllChild(Transform parent)
    {
        var children = parent.GetComponentsInChildren<Transform>();
        if (children.Length == 0) return this;
        foreach (var child in children)
        {
            if (child != parent)
                Destroy(child.gameObject);
        }

        return this;
    }
}