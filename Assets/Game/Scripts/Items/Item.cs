using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public enum Type { Ammo,Coin,Grenade,Heart,Weapon,GrenadeGroup};
    public Type type;
    public int value;

    private Vector3 rotationAxis = Vector3.up; 
    private float rotateDuration = 10.0f; 
    private Ease easeType = Ease.Linear; 

    private void Start()
    {
        RotateContinuously();
    }

    private void RotateContinuously()
    {
        transform.DORotate(rotationAxis * 360f, rotateDuration, RotateMode.FastBeyond360)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Restart);
    }
}
