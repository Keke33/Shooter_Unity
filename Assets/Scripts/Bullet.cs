using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other) {

        NewItem otherItem = other.GetComponent<NewItem>();
        if (otherItem != null) {
            otherItem.SetDamage(damage);
        }

        Destroy(gameObject);
        GetComponent<Explosion>()?.Explode();
    }
}
