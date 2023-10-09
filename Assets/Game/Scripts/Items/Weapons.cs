using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Weapons : MonoBehaviour
{
    public enum Type { Melle,Range};
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public ObjectPool bulletPool;

    public void Use()
    {
        if (type == Type.Melle)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        if (type == Type.Range)
        {
            activeBullet();
        }
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled=true;
        trailEffect.enabled=true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled=false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled=false;
    }
    public void activeBullet()
    {
        GameObject bullet = bulletPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = bulletPos.transform.position;
            bullet.transform.rotation = bulletPos.transform.rotation;
            bullet.SetActive(true);
        }
    }

}
