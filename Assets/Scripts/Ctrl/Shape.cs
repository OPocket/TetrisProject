using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private bool isPause = false;
    private float timer = 0.0f;
    public float stepTime = 0.8f;

    private Ctrl ctrl;
    private GameManager gameMgr;
    // 获取旋转点
    private Transform pivotts;
    // 触屏的尺度
    public float slidingDistance = 80f;
#if UNITY_ANDROID || UNITY_IOS
    private Vector2 startPos = new Vector2();
    private Vector2 endPos = new Vector2();
#endif
    private void Awake()
    {
        pivotts = transform.Find("Pivot");
    }
    private void Update()
    {
        if (isPause) return;
        timer += Time.deltaTime;
        if (timer > stepTime)
        {
            timer = 0.0f;
            DoFall();
        }       
        TouchCtrl();

    }
    public void SetColor(Color color, Ctrl ctrl, GameManager gameMgr)
    {
        foreach (Transform ts in transform)
        {
            if (ts.CompareTag("Block"))
            {
                ts.GetComponent<SpriteRenderer>().color = color;
            }        
        }
        this.ctrl = ctrl;
        this.gameMgr = gameMgr;
    }

    private void DoFall()
    {
        Vector3 vec = transform.position;
        vec.y -= 1;
        transform.position = vec;
        // 判断是否可移动
        if (ctrl.Model.IsVisablePosition(transform) == false)
        {
            vec.y += 1;
            transform.position = vec;
            pivotts = null;
            // 停止下落
            isPause = true;
            // 填充地图中的格子
            // 重新生成下一个方块并再次下落
            gameMgr.FallComplete();
        }
        else
        {
            ctrl.AudioMgr.PlayDrop();
        }
    }
    // 旋转
    private void DoRotate()
    {
        transform.RotateAround(pivotts.position, Vector3.forward, -90.0f);
        if (ctrl.Model.IsVisablePosition(transform) == false)
        {
            ctrl.AudioMgr.PlayBalloon();
            transform.RotateAround(pivotts.position, Vector3.forward, 90.0f);
        }
        else
        {
            ctrl.AudioMgr.PlayDrop();
        }
    }
    // 暂停下落
    public void StopFall(bool isPause)
    {
        this.isPause = isPause;
    }
    // 实现触屏控制
    private void TouchCtrl()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        // 键盘控制
        // 左右移动
        int h = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            h = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            h=1;
        }
        if (h != 0)
        {
            transform.position += new Vector3(h, 0, 0);
            // 判断是否可移动  
            if (ctrl.Model.IsVisablePosition(transform) == false)
            {
                // 碰撞音效
                ctrl.AudioMgr.PlayBalloon();
                transform.position -= new Vector3(h, 0, 0);
            }
            else
            {
                // 移动音效
                ctrl.AudioMgr.PlayDrop();
            }
        }
        // 旋转
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DoRotate();
        }
        // 加速下落
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            stepTime = 0.05f;
        }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount >= 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startPos = Input.touches[0].position;   //记录手指刚触碰的位置
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                endPos = Input.touches[0].position;
                if (Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y))
                {
                    if (Mathf.Abs(endPos.x - startPos.x) > slidingDistance)
                    {
                        int h = 0;
                        if (endPos.x > startPos.x)
                        {
                            // 向右
                            h = 1;
                        }
                        else if (endPos.x < startPos.x)
                        {
                            // 向左
                            h = -1;
                        }

                        if (h != 0)
                        {
                            transform.position += new Vector3(h, 0, 0);
                            // 判断是否可移动  
                            if (ctrl.Model.IsVisablePosition(transform) == false)
                            {
                                // 碰撞音效
                                ctrl.AudioMgr.PlayBalloon();
                                transform.position -= new Vector3(h, 0, 0);
                            }
                            else
                            {
                                // 移动音效
                                ctrl.AudioMgr.PlayDrop();
                            }
                        }
                    }

                }
                else if (Mathf.Abs(endPos.x - startPos.x) < Mathf.Abs(endPos.y - startPos.y))
                {
                    if (Mathf.Abs(endPos.y - startPos.y) > slidingDistance)
                    {
                        if (endPos.y > startPos.y)
                        {
                            // 向上
                            DoRotate();
                        }
                        else if (endPos.y < startPos.y)
                        {
                            // 向下
                            stepTime = 0.05f;
                        }
                    }
                }
                startPos = endPos = Vector2.zero;
            }
        }  
#endif
    }
}
