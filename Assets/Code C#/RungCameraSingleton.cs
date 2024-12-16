using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RungCameraSingleton : MonoBehaviour
{
    public static RungCameraSingleton Instance;
    private ShakeCam sk;
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
    // Start is called before the first frame update
    void Start()
    {
        sk = GetComponent<ShakeCam>();
    }
    public void Shake(float time, float doManh, float tanSo)
    {
        sk.Shake(time, doManh, tanSo);
    }
}
