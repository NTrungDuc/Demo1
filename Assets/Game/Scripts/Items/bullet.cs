using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public enum Type { Player, EnemyA, EnemyC};
    public Type type;
    public int damage;
    public int speed;
    [SerializeField] Rigidbody rb;
    public GameObject player;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(Constant.TAG_WALL))
        {
            gameObject.SetActive(false);
        }
        if (type == Type.Player)
        {
            if (collision.CompareTag(Constant.TAG_BOT))
            {
                gameObject.SetActive(false);
                collision.GetComponent<BotController>().takeDamage(damage);
            }
        }
        if (type == Type.EnemyA)
        {
            if (collision.CompareTag(Constant.TAG_PLAYER))
            {
                //collision.GetComponent<PlayerMovement>().takeDamage(damage);
                InvokeRepeating("ApplyDamage", 1f, 3f);
            }
        }
        if (type == Type.EnemyC)
        {
            if (collision.CompareTag(Constant.TAG_PLAYER))
            {
                gameObject.SetActive(false);
                collision.GetComponent<PlayerMovement>().takeDamage(damage);
            }
        }
    }
    public void ApplyDamage()
    {
        player.GetComponent<PlayerMovement>().takeDamage(damage);
    }
    private void OnEnable()
    {
        if (type != Type.EnemyA)
        {
            rb.velocity = transform.forward * speed;
        }
    }
}
