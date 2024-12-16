using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private float _tgr;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (_tgr <= 0f)
        {
            StopShake();
        }
        _tgr -= Time.deltaTime;
    }
    public void Shake(float thoiGianRung, float doManh, float tanSoRung)
    {
        noise.m_AmplitudeGain = doManh;
        noise.m_FrequencyGain = tanSoRung;
        _tgr = thoiGianRung;
    }
  
    public void StopShake()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0;
    }
}
