using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuState : FSMState
{
    private void Awake()
    {
        stateID = StateID.GameMenu;
        // 添加一个状态转换
        AddTransition(Transition.StartBtnClick, StateID.GamePlay);
    }

    public override void DoBeforeEntering()
    {
        ctrl.view.ShowMenu();
        ctrl.CameraMgr.NarrowSize();
    }

    public override void DoBeforeLeaving()
    {
        ctrl.view.HideMenu();
    }

    public void OnStartBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        fsm.PerformTransition(Transition.StartBtnClick);
    }

    public void OnRankBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.view.ShowRankUI(ctrl.Model.Score, ctrl.Model.HighScore, ctrl.Model.GameTimes);
        ctrl.view.SetRankUIActive(true);
    }

    public void OnSettingBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.view.SetSettingUIActive(true, ctrl.Model.GetMuteSet());
    }
    public void OnSettingUIClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.view.SetSettingUIActive(false, ctrl.Model.GetMuteSet());
    }
    public void OnAudioBtnClick()
    {
        ctrl.AudioMgr.PlayCursor();
        ctrl.Model.SetMuteData(!ctrl.Model.GetMuteSet());
        ctrl.view.SetAudioBtn(ctrl.Model.GetMuteSet());
    }
}
