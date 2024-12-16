using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSo : MonoBehaviour
{
    public int chuoiCombo;
    public float dame;
    public float tocChay, lucBatLui;
    public float mauToiDa;
    [HideInInspector] public Vector2 boostAttack, boostBatLui;
    [HideInInspector] public float mauHienTai;
    private float doLonBatLui;
    private Rigidbody2D rb;
    private void Start()
    {
        mauHienTai = mauToiDa;
        rb = GetComponent<Rigidbody2D>();
    }
    public void TruMau(ref float vl)
    {
        mauHienTai -= vl;
        if (gameObject == PlayerControl.Instance.gameObject) HubPlayer.hub.CapNhatHubHeart();
    }
    public void SetLucBatLui(float value)
    {
        lucBatLui = value;
    }
    public bool HetMau()
    {
        return mauHienTai <= 0f;
    }
    public void BatLui(Vector2 huongBiBatLui, float _lucBatLui, float time)
    {
        doLonBatLui = _lucBatLui;
        boostBatLui = doLonBatLui * huongBiBatLui;
        StartCoroutine(NgungBatLui(time));
    }
    IEnumerator NgungBatLui(float time)
    {
        float elapsedTime = 0f; // Thời gian đã trôi qua
        float initialBatLui = doLonBatLui; // Lưu độ lớn ban đầu của lực bật lùi

        while (elapsedTime < time)
        {
            yield return null; // Chờ đến frame tiếp theo
            elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua

            // Tính toán giá trị mới của doLonBatLui (giảm đều theo thời gian)
            doLonBatLui = Mathf.Lerp(initialBatLui, 0f, elapsedTime / time);

            // Cập nhật boostBatLui theo giá trị mới của doLonBatLui
            boostBatLui = boostBatLui.normalized * doLonBatLui;
        }

        // Đảm bảo doLonBatLui và boostBatLui về chính xác 0 sau khi kết thúc
        doLonBatLui = 0f;
        boostBatLui = Vector2.zero;
    }
    public void SetBoostAttack(float value)
    {
        if (rb.velocity == Vector2.zero) boostAttack.x = value * transform.localScale.x;
        else boostAttack = value * GetComponent<Rigidbody2D>().velocity.normalized;
    }
}
