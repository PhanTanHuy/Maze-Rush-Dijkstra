using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioClip[] sfxs;
    private int lengthSfxs;
    // Start is called before the first frame update
    void Start()
    {
        lengthSfxs = sfxs.Length;
    }
    public void PlaySfxIndex(int i)
    {
        if (i < lengthSfxs) QuanLiAmThanh.Instance.PlaySfx(sfxs[i]);
    }
   
}
