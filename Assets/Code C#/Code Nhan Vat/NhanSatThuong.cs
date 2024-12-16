using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhanSatThuong : MonoBehaviour
{
    private TrangThai tt;
    private void Start()
    {
        tt = GetComponent<TrangThai>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BoxAttack"))
        {
            if (TuMinhDanhMinh(collision)) return;
            // Xử lí bị tấn công
            ChiSo cs = collision.GetComponentInParent<ChiSo>();
            Vector2 huongTanCong = (cs.transform.position - transform.position).normalized;
            tt.Hurt(cs.dame, huongTanCong, cs.lucBatLui);
        }
        if (collision.CompareTag("ObjectBoxAttack"))
        {
            ChiSoObject cs = collision.GetComponentInParent<ChiSoObject>();
            Vector2 huongTanCong = (cs.transform.position - transform.position).normalized;
            tt.Hurt(cs.dame, huongTanCong, cs.lucBatLui);
        }
    }
    private bool TuMinhDanhMinh(Collider2D collision)
    {
        return collision.transform.parent == transform;
    }
}
