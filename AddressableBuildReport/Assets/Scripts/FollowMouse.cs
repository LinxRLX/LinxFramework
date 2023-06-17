using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;

        var rect = GetComponent<RectTransform>();
        var localPos = rect.localPosition;
        var width = rect.sizeDelta.x;

        // 防止出屏幕
        var screenWidth = Screen.width;
        // 左边界
        var leftDiff = (localPos.x - width / 2) + screenWidth * 0.5f;
        // 右边界
        var rightDiff = (localPos.x + width / 2) - screenWidth * 0.5f;

        if (leftDiff < 0)
        {
            localPos.x -= leftDiff;
        }

        if (rightDiff > 0)
        {
            localPos.x -= rightDiff;
        }

        transform.localPosition = localPos;
    }
}