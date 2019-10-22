using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectExtension
{
    // 避免旋转引起的误差（坐标不是整数问题）
    // 三维坐标转换为地图上的整数的二维坐标
    public static Vector2 VectorMap(Vector3 vec)
    {
        Vector2 vec2 = new Vector2();
        vec2.x = Mathf.RoundToInt(vec.x);
        vec2.y = Mathf.RoundToInt(vec.y);
        return vec2;
    }
}
