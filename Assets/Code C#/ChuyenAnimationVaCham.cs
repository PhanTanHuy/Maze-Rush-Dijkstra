using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuyenAnimationVaCham : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<Animator>().Play("no");
    }
}
