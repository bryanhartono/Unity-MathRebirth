using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResize : MonoBehaviour
{
    void Awake()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float canvasHeight = rt.rect.height;
        float desiredCanvasWidth = canvasHeight * Camera.main.aspect;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, desiredCanvasWidth);
    }
}
