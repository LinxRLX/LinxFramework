using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanShowFullText : MonoBehaviour
{
    public string fullText;

    public void Show(Text text)
    {
        UILogic.Instance.ShowFullText(fullText);
    }

    public void Hide()
    {
        UILogic.Instance.HideFullText();
    }
}