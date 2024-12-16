using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CuaSoTrongGame : MonoBehaviour
{
    public static CuaSoTrongGame Instance;
    public GameObject pauseMenu, overScreen, winMenu;
    private void Awake()
    {
        Instance = this;
        pauseMenu.SetActive(false);
        overScreen.SetActive(false);
        winMenu.SetActive(false);
    }
    public void OpenPauseMenu()
    {
        DemThoiGian.Instance.StopCountdown();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ClosePauseMenu()
    {
        QuanLiAmThanh.Instance.PlayButton();
        DemThoiGian.Instance.ContinueCountDown();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void VeNha()
    {
        QuanLiAmThanh.Instance.PlayButton();
        ClosePauseMenu();
        QuanLiScene.Instance.VeNha();
    }
    public void HienThiCuaSoWin()
    {
        DemThoiGian.Instance.StopCountdown();
        PlayMusic.Instance.musicSource.Stop();
        winMenu.SetActive(true);
    }
    public void HienThiOverScreen()
    {
        DemThoiGian.Instance.StopCountdown();
        overScreen.SetActive(true);
    }
}
