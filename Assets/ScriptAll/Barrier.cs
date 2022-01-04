using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetBool("IsopenCross", true);

        }

    }
    void OnCollisionExit2D(Collision2D other)
    {
        StartCoroutine(WaitClose());
    }
    IEnumerator WaitClose()
    {
        yield return new WaitForSeconds(2.5f);
        animator.SetBool("IsopenCross", false);
    }
}
