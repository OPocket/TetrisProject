using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public const int MAP_ROW = 10;
    public const int MAP_COLUM = 23;

    private Transform[,] mapTs = new Transform[MAP_ROW, MAP_COLUM];
    // 每消除一行所得分数
    private const int PER_SCORE = 10;
    // 当前得分
    private int score=0;
    // 最高得分
    private int highScore=0;
    // 游戏次数
    private int gameTimes=0;
    // 是否进行更新
    public bool isScoreUpdate = false;

    public int Score { get => score;}
    public int HighScore { get => highScore;}
    public int GameTimes { get => gameTimes;}

    private void Awake()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        if (PlayerPrefs.HasKey("GameTimes"))
        {
            gameTimes = PlayerPrefs.GetInt("GameTimes");
        }
    }

    // 判断图形中的每个小方块的位置是否可用
    public bool IsVisablePosition(Transform shape)
    {
        foreach (Transform child in shape)
        {
            // 跳过判断
            if (!child.CompareTag("Block")) continue;
            Vector2 vec = VectExtension.VectorMap(child.position);
            // 判断是否超出边界
            if (vec.x < 0 || vec.x >= MAP_ROW || vec.y < 0 || vec.y >= MAP_COLUM)
            {
                return false;
            }
            // 是否重叠
            else if (mapTs[(int)vec.x, (int)vec.y] != null)
            {
                return false;
            }          
        }
        return true;
    }
    // 下落后填充地图格子
    public void FillMap(Transform ts)
    {
        // shape中所有的子格子都要设置
        foreach (Transform child in ts)
        {
             if (!child.CompareTag("Block")) continue;
            Vector2 vec = VectExtension.VectorMap(child.position);
            mapTs[(int)vec.x, (int)vec.y] = child;
        }
    }
    // 判断消除
    public void ClearLine(AudioManager audioMgr)
    {
        for (int i = 0; i < MAP_COLUM; i++)
        {
            if (IsFull(i))
            {
                // 清除格子
                audioMgr.PlayLineClear();
                ClearLineBox(i);
                // 更新得分
                ShowScore();
                // 上部下移
                LineFallDown(i);
                // 下落后，上行下落变成当前行，所以需要重新检测当前行
                i--;
            }
        }      
    }
    // 判断所在行是否填满，即是否可清除
    private bool IsFull(int colum)
    {
        for (int i = 0; i < MAP_ROW; i++)
        {
            if (!mapTs[i, colum])
            {
                return false;
            }
        }
        return true;
    }
    // 清除对应行上面的格子
    private void ClearLineBox(int row)
    {
        for (int i = 0; i < MAP_ROW; i++)
        {
            if (mapTs[i, row])
            {
                Destroy(mapTs[i, row].gameObject);
                mapTs[i, row] = null;
            }        
        }
    }
    // 当前空行的上面一行进行下移
    private void LineFallDown(int row)
    {
        for(int i = row; i<MAP_COLUM-1; i++)
        {
            for (int j = 0; j < MAP_ROW; j++)
            {
                mapTs[j, i] = mapTs[j, i + 1];
                if (mapTs[j, i])
                {
                    mapTs[j, i].position += new Vector3(0, -1, 0);
                }
            }
        }     
    }
    // 更新并显示分数
    private void ShowScore()
    {
        score += PER_SCORE;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        // 告知manager更新分数
        isScoreUpdate = true;
    }
    public void SetScoreUpdateComplete()
    {
        isScoreUpdate = false;
    }
    // 检测游戏是否结束
    public bool IsGameEnd()
    {
        for (int i = 0; i < MAP_ROW; i++)
        {
            if (mapTs[i, MAP_COLUM-3])
            {    
                return true;
            }
        }
        return false;
    }

    // 重新开始
    public void Restart()
    {
        for (int i = 0; i < MAP_COLUM - 1; i++)
        {
            for (int j = 0; j < MAP_ROW; j++)
            {
                if (mapTs[j, i])
                {
                    Destroy(mapTs[j, i].gameObject);
                    mapTs[j, i] = null;
                }
            }
        }
        score = 0;       
    }
    // 删除缓存记录
    public void DeleteData()
    {
        score = 0;
        highScore = 0;
        PlayerPrefs.SetInt("HighScore", highScore);
        gameTimes = 0;
        PlayerPrefs.SetInt("GameTimes", gameTimes);
    }
    // 一旦开始游戏就增加一次游戏次数
    public void UpdateGameTimes()
    {
        gameTimes += 1;
        PlayerPrefs.SetInt("GameTimes", gameTimes);
    }
    // 本地保存是否静音设置
    public void SetMuteData(bool isMute)
    {
        int i;
        if (isMute)
        {
            i = 1;
        }
        else
        {
            i = 0;
        }
        PlayerPrefs.SetInt("IsMute", i);
    }
    public bool GetMuteSet()
    {
        if (PlayerPrefs.HasKey("IsMute"))
        {
            return PlayerPrefs.GetInt("IsMute") == 1;
        }
        return false;
    }
}
