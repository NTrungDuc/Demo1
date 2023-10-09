using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] Rigidbody rb;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.TAG_WALL))
        {
            gameObject.SetActive(false);
        }
        if (collision.CompareTag(Constant.TAG_BOT))
        {
            gameObject.SetActive(false);
            collision.GetComponent<BotController>().takeDamage(damage);
        }
    }
    private void OnEnable()
    {
        rb.velocity = transform.forward * 50;
    }
}
