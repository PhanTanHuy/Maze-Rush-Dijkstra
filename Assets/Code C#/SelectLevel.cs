using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    private string tenLevel;
    private int currentLevel;
    public GameObject oKhoa, pre, next;
    public TextMeshProUGUI textLevel;
    private void Start()
    {
        oKhoa.SetActive(false);
        currentLevel = 0;
        tenLevel = "Level" + currentLevel.ToString();
        textLevel.text = "Level " + currentLevel.ToString();
        pre.SetActive(false);
    }
    public void NextLevel()
    {
        QuanLiAmThanh.Instance.PlayButton();
        currentLevel++;
        if (currentLevel >= 4) currentLevel = 4;
        oKhoa.SetActive(!QuanLiScene.Instance.DaMoKhoaLevel(currentLevel));
        tenLevel = "Level" + currentLevel.ToString();
        textLevel.text = "Level " + currentLevel.ToString();
        if (currentLevel == 4)
        {
            next.SetActive(false);
            pre.SetActive(true);
        }
        else
        {
            next.SetActive(true);
            pre.SetActive(true);
        }
    }
    public void PreviousLevel()
    {
        QuanLiAmThanh.Instance.PlayButton();
        currentLevel--;
        if (currentLevel <= 0) currentLevel = 0;
        oKhoa.SetActive(!QuanLiScene.Instance.DaMoKhoaLevel(currentLevel));
        tenLevel = "Level" + currentLevel.ToString();
        textLevel.text = "Level " + currentLevel.ToString();
        if (currentLevel == 0)
        {
            next.SetActive(true);
            pre.SetActive(false);
        }
        else
        {
            next.SetActive(true);
            pre.SetActive(true);
        }
    }
    public void EnterMaze()
    {
        if (!QuanLiScene.Instance.DaMoKhoaLevel(currentLevel)) return;
        QuanLiScene.Instance.LoadLevelStartGame(currentLevel);
        QuanLiAmThanh.Instance.PlayButton();
    }

}
