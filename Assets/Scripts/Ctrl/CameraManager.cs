using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }

    // 放大
    public void NagnifySize()
    {
        cam.DOOrthoSize(14.0f, 0.5f);
    }
    // 缩小
    public void NarrowSize()
    {
        cam.DOOrthoSize(17.0f, 0.5f);
    }
}
