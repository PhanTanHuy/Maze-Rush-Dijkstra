using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoatKhoiMeCung : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            if (!QuanLiQuaiVat.Instance.DaHetQuaiVat()) return;
            CuaSoTrongGame.Instance.HienThiCuaSoWin();
            PlayerControl.Instance.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerControl.Instance.GetComponent<PlayerControl>().enabled = false;
        }
    }
}
