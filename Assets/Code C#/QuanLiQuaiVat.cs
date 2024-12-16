using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuanLiQuaiVat : MonoBehaviour
{
    public static QuanLiQuaiVat Instance;
    public TextMeshProUGUI textSoQuaiVatConLai;
    public GameObject objectChuaQuaiVat;
    private int soLuongQuaiVatDaGiet, soLuongQuaiVatToiDa;
    private void Awake()
    {
        Instance = this;
        soLuongQuaiVatDaGiet = 0;
        soLuongQuaiVatToiDa = objectChuaQuaiVat.transform.childCount;
        textSoQuaiVatConLai.text = soLuongQuaiVatDaGiet.ToString() + " / " + soLuongQuaiVatToiDa.ToString();
    }
    public bool DaHetQuaiVat()
    {
        return soLuongQuaiVatDaGiet == soLuongQuaiVatToiDa;
    }
    public void CapNhatSoLuongQuaiVat()
    {
        if (soLuongQuaiVatDaGiet == soLuongQuaiVatToiDa) return;
        soLuongQuaiVatDaGiet++;
        textSoQuaiVatConLai.text = soLuongQuaiVatDaGiet.ToString() + " / " + soLuongQuaiVatToiDa.ToString();
    }
}
