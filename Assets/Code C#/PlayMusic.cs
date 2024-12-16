using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioClip music;
    public AudioSource musicSource;
    public static PlayMusic Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        musicSource.loop = true;
        musicSource.clip = music;
        musicSource.volume = QuanLiAmThanh.Instance.volumeMusic;
        musicSource.Play();
    }

   
}
