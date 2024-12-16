using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicSlider : MonoBehaviour
{
    public Slider sliderS, sliderM;
    public void ChangeSFXVolume()
    {
        QuanLiAmThanh.Instance.volumeSFX = sliderS.value;
    }
    public void ChangeMusicVolume() 
    { 
        QuanLiAmThanh.Instance.volumeMusic = sliderM.value;
        PlayMusic.Instance.musicSource.volume = sliderM.value;
    }
}
