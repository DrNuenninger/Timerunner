using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landmine : MonoBehaviour
{
    public float explosionRadius = 1.5f;
    public GameObject explosionEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
    }

    private void Explode()
    {
        Destroy(Instantiate(explosionEffect, transform.position, transform.rotation), 3);
        Collider2D[] damageableInRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        if (damageableInRadius.Length == 1)
        {
            damageableInRadius[0].gameObject.GetComponent<Controller2D>().isDead = true;
            damageableInRadius[0].gameObject.GetComponent<Controller2D>().isSplashed = true;
            damageableInRadius[0].gameObject.GetComponent<Controller2D>().deathByMine();
        }
        Destroy(gameObject);
    }
}
