using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubPlayer : MonoBehaviour
{
    public static HubPlayer hub;
    public GameObject heartGoc;
    private List<GameObject> Hearts;
    // Start is called before the first frame update
    private void Awake()
    {
        hub = this;
    }
    void Start()
    {
        Hearts = new List<GameObject>();
        ChiSo cs = PlayerControl.Instance.GetComponent<ChiSo>();
        Hearts.Add(heartGoc);
        for (int i = 1; i < cs.mauToiDa; i++)
        {
            GameObject newHeart = Instantiate(heartGoc);
            newHeart.transform.SetParent(heartGoc.transform.parent);
            float plusDistanceX = 80f * i;
            newHeart.transform.position = heartGoc.transform.position;
            newHeart.transform.position += new Vector3(plusDistanceX, 0f, 0f);
            newHeart.transform.localScale = Vector3.one;
            Hearts.Add(newHeart);
        }
    }
    public void CapNhatHubHeart()
    {
        ChiSo cs = PlayerControl.Instance.GetComponent<ChiSo>();
        if (cs.mauHienTai <= 2) GetComponent<Animator>().Play("MauYeu");
        else
        {

        }
        for (float i = cs.mauToiDa; i >= 1; i--)
        {
            if (i > cs.mauHienTai) Hearts[(int)i - 1].GetComponent<Image>().color = Color.black;
            else break;
        }
    }

}
