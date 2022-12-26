using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItem : MonoBehaviour
{
    public float distanceToCollect;

    [HideInInspector] public bool collected = false;
    [HideInInspector] public bool canCollect = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviour playerBehaviour))
        {
            OnCollected(Vector3.zero, collision.transform);
        }


    }

    public abstract void OnCollected(Vector3 targetPoint, Transform parent = null);
}
