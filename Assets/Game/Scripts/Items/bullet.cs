using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] Rigidbody rb;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        rb.velocity = transform.forward * 50;
    }
}
