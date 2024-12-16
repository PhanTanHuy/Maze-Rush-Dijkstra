using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrangThai : MonoBehaviour
{
    private ControlAnimator controlAnimator;
    private ChiSo cs;
    private int currentCombo;
    [HideInInspector] public bool isAttack, isHurt;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public float timeResetCurrentCombo;
    public bool CutScene;
    private void Start()
    {
        controlAnimator = GetComponent<ControlAnimator>();
        cs = GetComponent<ChiSo>();
        currentCombo = 0;
        isAttack = isHurt = false;
        isAlive = true;
    }
    public bool AttackNormal()
    {
        if (KoTheThucHienHanhDong()) return false;
        if (isAttack || isHurt) return false;
        StartAttack();
        controlAnimator.PlayAttack("Attack" + currentCombo);
        currentCombo++;
        timeResetCurrentCombo = 0.75f;
        if (currentCombo >= cs.chuoiCombo) currentCombo = 0;
        return true;
    }
    public bool AttackNotNormal(string name)
    {
        if (KoTheThucHienHanhDong()) return false;
        if (isAttack || isHurt) return false;
        StartAttack();
        controlAnimator.PlayAttack(name);
        return true;
    }
    public void ResetCurrentComboAfterTime()
    {
        timeResetCurrentCombo -= Time.deltaTime;
        if (timeResetCurrentCombo < 0f) currentCombo = 0;
    }
    public bool CanNotFlip()
    {
        return isAttack|| isHurt;
    }
    public bool CanMove()
    {
        return !isAttack && !isHurt;
    }
    public void EndAttack()
    {
        isAttack = false;
        cs.SetBoostAttack(0f);
    }
    public void StartAttack()
    {
        isAttack = true;
    }
    public void Dash()
    {
        if (KoTheThucHienHanhDong()) return;
        if (isAttack || isHurt) return;
        EndAttack();
        controlAnimator.PlayDash();
        cs.SetBoostAttack(10f);
    }
    public void Hurt(float dame, Vector2 huongTanCong, float lbl)
    {
        RungCameraSingleton.Instance.Shake(0.15f, 3f, 1f);
        cs.BatLui(huongTanCong, lbl, 0.25f);
        PoolVfx.Instance.CreateHitEffect(transform.position, huongTanCong);
        QuanLiAmThanh.Instance.PlayHit();
        isHurt = true;
        isAttack = false;
        cs.SetBoostAttack(0f);
        cs.TruMau(ref dame);
        if (cs.HetMau()) Death();
        if (KoTheThucHienHanhDong()) return;
        controlAnimator.PlayHurt();
    }
    public void EndHurt()
    {
        isHurt = false;
        EndAttack();
    }
    public void Death()
    {
        if (isAlive)
        {
            isAlive = false;
            controlAnimator.PlayDeath();
        }
    }
    public void DiChuyenDiNao(float vl)
    {
        if (KoTheThucHienHanhDong()) return;
        if (isHurt || isAttack) vl = 0;
        controlAnimator.Move(vl);
    }
    public bool KoTheThucHienHanhDong()
    {
        if (!isAlive) return true;
        return false;
    }
}
