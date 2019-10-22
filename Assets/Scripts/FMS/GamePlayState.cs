using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayState : FSMState
{
    private void Awake()
    {
        stateID = StateID.GamePlay;
        AddTransition(Transition.PauseBtnClick, StateID.GameMenu);
    }

    public override void DoBeforeEntering()
    {
        ctrl.view.ShowGameUI();
        ctrl.CameraMgr.NagnifySize();
        // 开始游戏
        ctrl.GameMgr.StartGame();
    }

    public override void DoBeforeLeaving()
    {
        ctrl.view.HideGameUI();
        ctrl.view.ShowRestartBtn();
        ctrl.GameMgr.PauseGame();
    }

    public void OnPauseBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        fsm.PerformTransition(Transition.PauseBtnClick);
    }

    public void OnRestartBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.view.HideGameOverUI();
        ctrl.Model.Restart();
        ctrl.GameMgr.UpdateScore();
        ctrl.GameMgr.StartGame();
    }

    // 返回主界面
    public void OnHomeBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnRankUIBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.view.SetRankUIActive(false);
    }
    // 删除记录事件
    public void OnDeleteBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.Model.DeleteData();
        ctrl.view.ShowRankUI(ctrl.Model.Score, ctrl.Model.HighScore, ctrl.Model.GameTimes);
    }
}
