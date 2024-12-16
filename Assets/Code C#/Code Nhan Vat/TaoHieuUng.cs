using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoHieuUng : MonoBehaviour
{
    public Color GhostColor;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        if (spriteRenderer == null)
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void GhostEffect()
    {
        PoolVfx.Instance.CreateGhostEffect(transform.position, spriteRenderer.sprite, GhostColor, transform.localScale);
    }
    public void TaoVaCham()
    {
        PoolVfx.Instance.CreateHitEffect(transform.position, transform.localScale);
    }
}
