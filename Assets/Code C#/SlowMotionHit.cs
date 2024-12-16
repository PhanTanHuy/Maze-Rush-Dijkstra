using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionHit : MonoBehaviour
{
    public static SlowMotionHit Instance;
    private Coroutine resetTimeCor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        resetTimeCor = null;
    }
    public void SlowKO()
    {
        Time.timeScale = 0.15f;
        if (resetTimeCor != null)
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.3f));
    }
    public void SlowHit()
    {
        Time.timeScale = 0.01f;
        if (resetTimeCor != null )
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.001f));

    }
    IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;
    }
}
