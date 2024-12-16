using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           // Tham chiếu đến Transform của người chơi
    public LayerMask obstacleLayer;    // Layer của tường hoặc vật cản
    public ZoneAttack zat;
    private ChiSo cs;
    public float tocDoDuoi;
    [SerializeField] private float khoangCachTanCong, thoiGianTanCong;
    private float _thoiGianTanCong, _tocDoDuoi;
    // di chuyen random
    private float changeDirectionInterval; // Thời gian thay đổi hướng
    private Rigidbody2D rb; // Rigidbody của enemy
    private Vector2 huongDiChuyen; // Hướng di chuyển hiện tại
    private float timeSinceLastChange; // Thời gian từ lần đổi hướng gần nhất
    private TrangThai tt;
    private Vector2 left, right;
    private float timeDungIm;
    private float thoiGianDiRaXa;
    private void Start()
    {
        player = PlayerControl.Instance.transform;

        // bỏ qua va chạm giua nguoi choi va enemy
        Physics2D.IgnoreCollision(player.transform.GetChild(0).GetComponent<Collider2D>(), transform.GetChild(0).GetComponent<Collider2D>());
        //
        cs = GetComponent<ChiSo>();
        rb = GetComponent<Rigidbody2D>();
        tt = GetComponent<TrangThai>();
        ThayDoiHuongDiChuyen();
        changeDirectionInterval = Random.Range(0.5f, 1.75f);
        right = new Vector2(1f, 1f);
        left = new Vector2(-1f, 1f);
    }
    private void Update()
    {
        if (tt.CutScene) return;
        if (!tt.isAlive)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        tt.ResetCurrentComboAfterTime();
        CheckNguoiChoi();
        DiChuyen();
        timeSinceLastChange += Time.deltaTime;
        _thoiGianTanCong += Time.deltaTime;
        timeDungIm -= Time.deltaTime;
        thoiGianDiRaXa -= Time.deltaTime;
        if (timeDungIm < -5f) timeDungIm = Random.Range(0.25f, 2f);
    }
    private void CheckNguoiChoi()
    {
        if (zat.isInZoneAttack)
        {
            timeDungIm = -1f;
            // Bắn raycast từ enemy đến người chơi
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer + 3f, obstacleLayer);

            Debug.DrawRay(transform.position, directionToPlayer * (distanceToPlayer + 3f), Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Playerrr")) // Không có vật cản giữa enemy và người chơi
            {
                timeSinceLastChange = 0f;
                if (thoiGianDiRaXa < 0f)
                {
                    huongDiChuyen = directionToPlayer;
                    _tocDoDuoi = tocDoDuoi;
                }
                if (distanceToPlayer < khoangCachTanCong && _thoiGianTanCong > thoiGianTanCong)
                {
                    _thoiGianTanCong = 0f;
                    if (tt.AttackNormal()) DiRaXaMotTi();
                }
            }
        }
        else
        {
            if (_tocDoDuoi != 0f) _tocDoDuoi = 0f;
        }
    }
    public void DiRaXaMotTi()
    {
        thoiGianDiRaXa = Random.Range(0.75f, 1.5f);
        _tocDoDuoi = 0f;
        huongDiChuyen = RotateVector(-huongDiChuyen, Random.Range(-50f, 50f));
    }
    private Vector2 RotateVector(Vector2 originalVector, float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad; // Chuyển đổi sang radian
        float cos = Mathf.Cos(angleInRadians);
        float sin = Mathf.Sin(angleInRadians);

        // Tính toán vector sau khi xoay
        float newX = originalVector.x * cos - originalVector.y * sin;
        float newY = originalVector.x * sin + originalVector.y * cos;

        return new Vector2(newX, newY).normalized;
    }

    public void TangSoLuongQuaiVatDaGiet()
    {
        QuanLiQuaiVat.Instance.CapNhatSoLuongQuaiVat();
    }
    private void DiChuyen()
    {
        float di = timeDungIm > 0f || !tt.CanMove() ? 0f : 1f;
        rb.velocity = huongDiChuyen * (cs.tocChay + _tocDoDuoi) * di + cs.boostAttack - cs.boostBatLui;
        Flip();
        float VanTocSetAnimation = rb.velocity.x == 0f ? rb.velocity.y : rb.velocity.x;
        tt.DiChuyenDiNao(VanTocSetAnimation);
        ThayDoiHuongDiChuyen();
    }
    private void Flip()
    {
        if (tt.CanNotFlip() || thoiGianDiRaXa > 0f) return;
        if (huongDiChuyen.x > 0f) transform.localScale = right;
        if (huongDiChuyen.x < 0f) transform.localScale = left;
    }
    private void ThayDoiHuongDiChuyen()
    {
        if (timeSinceLastChange >= changeDirectionInterval)
        {
            // Đổi hướng di chuyển ngẫu nhiên
            timeSinceLastChange = 0f;
            if (thoiGianDiRaXa > 0f)
            {
                changeDirectionInterval = Random.Range(0.1f, 0.25f);
                huongDiChuyen = RotateVector(huongDiChuyen, Random.Range(-40f, 40f));
            }
            else
            {
                changeDirectionInterval = Random.Range(0.5f, 1.5f);
                huongDiChuyen = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            }
        }
        
    }
    
}
