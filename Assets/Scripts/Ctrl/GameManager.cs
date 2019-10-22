using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Ctrl ctrl;

    // 是否开始游戏
    private bool isStart = false;
    // 是否通过游戏
    private bool isEnd = false;
    // 是否暂停游戏
    private bool isPause = false;

    public Shape[] shapes;
    public Color[] colors;
    // 当前下落的形状
    private Shape curShape = null;

    private void Awake()
    {
        ctrl = GetComponent<Ctrl>();
    }

    private void Start()
    {
        UpdateScore();
    }

    private void BornShape()
    {
        // 随机形状
        int i = Random.Range(0, shapes.Length);
        int j = Random.Range(0, colors.Length);
        curShape = GameObject.Instantiate(shapes[i], shapes[i].transform.position, Quaternion.identity);
        curShape.SetColor(colors[j], ctrl, this);
    }

    private void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
        if (isEnd || isPause) return;
        if (isStart && curShape == null)
        {
            BornShape();
        }
    }

    public void StartGame()
    {
        if (!isPause)
        {
            // 如果不是暂停游戏的情况，就增加一次游戏次数
            ctrl.Model.UpdateGameTimes();
        }
        isStart = true;
        isPause = false;
        if (curShape)
        {
            curShape.StopFall(false);
        }
        isEnd = false;
    }
    // 暂停游戏
    public void PauseGame()
    {
        isPause = true;
        if (curShape)
        {
            curShape.StopFall(true);
        }
    }

    // 填充对应的地图格子
    public void FallComplete()
    {
        ctrl.Model.FillMap(curShape.transform);
        ctrl.Model.ClearLine(ctrl.AudioMgr);
        if (ctrl.Model.isScoreUpdate)
        {
            UpdateScore();
        }
        ctrl.Model.SetScoreUpdateComplete();
        // 判断游戏是否结束
        if (ctrl.Model.IsGameEnd())
        {
            isEnd = true;
            ctrl.view.ShowGameOverUI(ctrl.Model.Score);
            // 播放游戏结束的音效
            ctrl.AudioMgr.PlayGameOver();
        }
        curShape = null;
    }

    public void UpdateScore()
    {
        ctrl.view.UpdateScore(ctrl.Model.Score, ctrl.Model.HighScore);
    }

}
