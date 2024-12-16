using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolVfx : MonoBehaviour
{
    public static PoolVfx Instance;
    public GameObject ghostEffect;
    public ParticleSystem HitEffect;
    private Queue<GameObject> GhostEffectQueue;
    private Queue<GameObject> HitEffectQueue;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //
        AddQueue(ref GhostEffectQueue, 20, ghostEffect);
        AddQueue(ref HitEffectQueue, 10, HitEffect.gameObject);

    }
    private void AddQueue(ref Queue<GameObject> q, int soLuong, GameObject gobj)
    {
        q = new Queue<GameObject>();
        for (int i = 0; i < soLuong; i++)
        {
            GameObject g = Instantiate(gobj);
            g.SetActive(false);
            q.Enqueue(g);
        }
    }
    public void CreateHitEffect(Vector2 pos, Vector2 huong)
    {
        ParticleSystem hitef = HitEffectQueue.Dequeue().GetComponent<ParticleSystem>();
        hitef.gameObject.SetActive(true);
        hitef.transform.position = pos;
        if (huong.x < 0)  // Nếu hướng đi sang trái (bên trái hình tròn)
        {
            hitef.transform.rotation = Quaternion.Euler(0, 15f, 0f);
        }
        else  // Nếu hướng đi sang phải (bên phải hình tròn)
        {
            hitef.transform.rotation = Quaternion.Euler(0, -15f, 0f);
        }
        hitef.Play();
        StartCoroutine(ReturnToPoolAfterDelay(HitEffectQueue, hitef.gameObject, 0.75f));
    }
    public void CreateGhostEffect(Vector2 pos, Sprite sprite, Color cl, Vector3 scal)
    {
        GameObject gobj = GhostEffectQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        gobj.transform.localScale = scal;
        gobj.GetComponent<SpriteRenderer>().sprite = sprite;
        gobj.GetComponent<SpriteRenderer>().color = cl;
        StartCoroutine(ReturnToPoolAfterDelay(GhostEffectQueue, gobj, 0.75f));
    }
    
    private IEnumerator ReturnToPoolAfterDelay(Queue<GameObject> q, GameObject gobj, float delay)
    {
        yield return new WaitForSeconds(delay);

        gobj.SetActive(false);
        q.Enqueue(gobj); 
    }
}
