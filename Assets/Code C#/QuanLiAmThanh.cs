using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuanLiAmThanh : MonoBehaviour
{
    public static QuanLiAmThanh Instance;
    public AudioClip[] hits;
    public AudioClip button;
    public AudioSource audioSourceSFX;
    [HideInInspector] public float volumeSFX, volumeMusic;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            volumeMusic = volumeSFX = 1f;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayHit()
    {
        audioSourceSFX.PlayOneShot(hits[Random.Range(0, hits.Length)]);
    }
    public void PlayButton()
    {
        audioSourceSFX.PlayOneShot(button);
    }
    public void PlaySfx(AudioClip clip)
    {
        audioSourceSFX.PlayOneShot(clip);
    }
}
