using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    [HideInInspector] public bool isInZoneAttack;
    private void Start()
    {
        isInZoneAttack = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            isInZoneAttack = true;
            Debug.Log("VaoVungAttack");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Playerrr"))
        {
            isInZoneAttack = false;
            Debug.Log("ThoatVungAttack");
        }
    }
}
