using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public GameObject Option, Member, Credits;
    private void Start()
    {
        Option.SetActive(false);
        Member.SetActive(false);
        Credits.SetActive(false);
    }
    public void Quite()
    {
        Application.Quit();
    }
    public void OpenOption()
    {
        QuanLiAmThanh.Instance.PlayButton();
        Option.SetActive(true);
    }
    public void CloseOption()
    {
        QuanLiAmThanh.Instance.PlayButton();
        Option.SetActive(false);
    }
    public void OpenMember()
    {
        QuanLiAmThanh.Instance.PlayButton();
        Member.SetActive(true);
    }
    public void CloseMember()
    {
        QuanLiAmThanh.Instance.PlayButton();
        Member.SetActive(false);
    }
    public void OpenCredits()
    {
        QuanLiAmThanh.Instance.PlayButton();
        Credits.SetActive(true);
    }
    public void CloseCredits()
    {
        Credits.SetActive(false);
        QuanLiAmThanh.Instance.PlayButton();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("ChonLevel");
        QuanLiAmThanh.Instance.PlayButton();
    }
}
