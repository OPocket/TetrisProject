using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    private RectTransform logoName;
    private RectTransform menuUI;
    private GameObject restartBtn;

    private RectTransform gameUI;
    private Text overScoreText;
    private Text highScoreText;

    private RectTransform gameOverUI;
    private Text scoreText;

    private RectTransform rankUI;
    private Text rankCurScoreText;
    private Text rankHighScoreText;
    private Text rankTimeText;

    private RectTransform settingUI;
    private GameObject muteImage;

    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Screen.SetResolution(480, 800, false);
#endif
        logoName = transform.Find("Canvas/NameLogo") as RectTransform;

        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        restartBtn = transform.Find("Canvas/MenuUI/RestartBtn").gameObject;

        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        scoreText = gameUI.Find("ScoreLable/ScoreText").GetComponent<Text>();
        highScoreText = gameUI.Find("HighScoreLable/HighScoreText").GetComponent<Text>();

        gameOverUI = transform.Find("Canvas/GameOverUI") as RectTransform;
        overScoreText = gameOverUI.Find("BackGround/ScoreLable").GetComponent<Text>();

        rankUI = transform.Find("Canvas/RankUI") as RectTransform;
        rankCurScoreText = rankUI.Find("BackGround/CurLable/Text").GetComponent<Text>();
        rankHighScoreText = rankUI.Find("BackGround/HighLable/Text").GetComponent<Text>();
        rankTimeText = rankUI.Find("BackGround/TimeLable/Text").GetComponent<Text>();

        settingUI = transform.Find("Canvas/SettingUI") as RectTransform;
        muteImage = settingUI.Find("BackGround/AudioBtn/None").gameObject;
    }

    public void ShowMenu()
    {
        logoName.gameObject.SetActive(true);
        logoName.DOAnchorPosY(-140.0f, 0.5f);     
        menuUI.gameObject.SetActive(true);
        menuUI.DOAnchorPosY(108.0f, 0.5f);
    }

    public void HideMenu()
    {

        logoName.DOAnchorPosY(140.0f, 0.5f)
            .OnComplete(delegate { logoName.gameObject.SetActive(false); });

        menuUI.DOAnchorPosY(-108.0f, 0.5f)
            .OnComplete(delegate { menuUI.gameObject.SetActive(false); });
    }

    public void ShowGameUI()
    {
        gameUI.gameObject.SetActive(true);
        gameUI.DOAnchorPosY(-130.0f, 0.5f);
    }

    public void HideGameUI()
    {
        gameUI.DOAnchorPosY(130.0f, 0.5f)
            .OnComplete(delegate { gameUI.gameObject.SetActive(false); });
    }

    public void ShowRestartBtn()
    {
        restartBtn.SetActive(true);
    }

    public void HideGameOverUI()
    {
        gameOverUI.gameObject.SetActive(false);
    }

    // 设置分数显示
    public void UpdateScore(int score, int highScore)
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
    // 游戏结束界面
    public void ShowGameOverUI(int score)
    {
        overScoreText.text = score.ToString();
        gameOverUI.gameObject.SetActive(true);
    }

    // 排名显示界面
    public void ShowRankUI(int score, int highScore, int times)
    {
        rankCurScoreText.text = score.ToString();
        rankHighScoreText.text = highScore.ToString();
        rankTimeText.text = times.ToString();
    }
    public void SetRankUIActive(bool isActive)
    {
        rankUI.gameObject.SetActive(isActive);
    }
    public void SetSettingUIActive(bool isActive, bool isMute)
    {
        muteImage.SetActive(isMute);
        settingUI.gameObject.SetActive(isActive);
    }

    public void SetAudioBtn(bool isMute)
    {
        muteImage.SetActive(isMute);
    }
}
