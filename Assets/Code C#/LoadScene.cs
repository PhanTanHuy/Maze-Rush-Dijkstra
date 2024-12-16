using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneHienTai()
    {
        QuanLiAmThanh.Instance.PlayButton();
        QuanLiScene.Instance.LoadLaiSceneHienTai();
    }
    public void LoadNextScene()
    {
        QuanLiAmThanh.Instance.PlayButton();
        QuanLiScene.Instance.LoadSceneTiepTheo();
    }
    public void LoadHome()
    {
        QuanLiScene.Instance.VeNha();
    }
}
