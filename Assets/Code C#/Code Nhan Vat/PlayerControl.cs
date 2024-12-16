using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 right, left;
    private ChiSo cs;
    private TrangThai tt;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cs = GetComponent<ChiSo>();
        tt = GetComponent<TrangThai>();
        right = new Vector2(1f, 1f);
        left = new Vector2(-1f, 1f);
    }

    void Update()
    {
        if (tt.CutScene)
        {
            return;
        }
        if (!tt.isAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        tt.ResetCurrentComboAfterTime();
        DiChuyen();
        TanCong();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (tt.KoTheThucHienHanhDong()) return;
            tt.EndAttack();
            tt.EndHurt();
            tt.StartAttack();
            GetComponent<Animator>().Play("TimDuong");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CuaSoTrongGame.Instance.OpenPauseMenu();
        }
    }
    public void FindPath()
    {
        TimDuong.Instance.TimDuongThoatMeCung();
    }
    public void KeThuaCuoc()
    {
        CuaSoTrongGame.Instance.HienThiOverScreen();
    }    
    private void TanCong()
    {
        if (Input.GetMouseButton(0)) tt.AttackNormal();
        if (Input.GetMouseButton(1)) tt.AttackNotNormal("AttackPow");
    }
    private void DiChuyen()
    {
        // luot
        if (Input.GetKeyDown(KeyCode.LeftShift)) tt.Dash();
        // Nhận đầu vào từ bàn phím (WASD hoặc các phím mũi tên)
        if (Input.GetKey(KeyCode.D)) movement.x = 1f;
        else if (Input.GetKey(KeyCode.A)) movement.x = -1f;
        else movement.x = 0f;
        if (Input.GetKey(KeyCode.W)) movement.y = 1f;
        else if (Input.GetKey(KeyCode.S)) movement.y = -1f;
        else movement.y = 0f;
        Flip();
        if (!tt.CanMove()) movement = Vector2.zero;
        if (cs.boostAttack != Vector2.zero)
        {
            movement = Vector2.zero;
        }
        rb.velocity = movement * cs.tocChay + cs.boostAttack - cs.boostBatLui;
        float VanTocSetAnimation = rb.velocity.x == 0f ? rb.velocity.y : rb.velocity.x;
        tt.DiChuyenDiNao(VanTocSetAnimation);
    }
    
    private void Flip()
    {
        if (tt.CanNotFlip()) return;
        if (movement.x > 0f) transform.localScale = right;
        if (movement.x < 0f) transform.localScale = left;
    }
}
