using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuanLiScene : MonoBehaviour
{
    public static QuanLiScene Instance;
    private List<int> nhungLevelDaMoKhoa;
    private int indexLevelHienTai;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            nhungLevelDaMoKhoa = new List<int>();
            //
            PlayerPrefs.SetInt("Level0", 1);
            CapNhatNhungLevel();
            //
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void VeNha()
    {
        CapNhatNhungLevel();
        SceneManager.LoadScene("Home");
    }
    public void CapNhatNhungLevel()
    {
        for (int i = 0; i < 5; i++)
        {
            nhungLevelDaMoKhoa.Add(PlayerPrefs.GetInt("Level" + i.ToString(), 0));
        }
    }
    public void LoadLevelStartGame(int indexLevel)
    {
        indexLevelHienTai = indexLevel;
        SceneManager.LoadScene("Level" + indexLevel.ToString());
    }
    public bool DaMoKhoaLevel(int indexLevel)
    {
        return nhungLevelDaMoKhoa[indexLevel] == 1;
    }

    public void UnClockLevel()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
    }
    public void LoadLaiSceneHienTai()
    {
        // Lấy tên hoặc index của scene hiện tại
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Load lại scene hiện tại
        SceneManager.LoadScene(currentSceneName);
    }
    public void LoadSceneTiepTheo()
    {
        indexLevelHienTai++;
        if (indexLevelHienTai >= 5)
        {
            SceneManager.LoadScene("Ending");
            return;
        }
        PlayerPrefs.SetInt("Level" + indexLevelHienTai.ToString(), 1);
        SceneManager.LoadScene("Level" + indexLevelHienTai.ToString());
    }
}
