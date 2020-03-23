using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public GameObject explosionEffect;
    public float speed = 5f;
    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(Instantiate(explosionEffect, transform.position, transform.rotation), 1); 
        Collider2D[] damageableInRadius = Physics2D.OverlapCircleAll(transform.position, 1.5f, LayerMask.GetMask("Player"));
        if (damageableInRadius.Length == 1)
        {
            damageableInRadius[0].gameObject.GetComponent<Controller2D>().isDead = true;
        }
        Destroy(gameObject);
    }
}
