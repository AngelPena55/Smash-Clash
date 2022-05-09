using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public GameObject impactEffect;
    public GameObject impactObj;
    public int damage = 10;
    private Vector3 myScreenPos;
    private Vector3 enemyPos;
    public Rigidbody2D rb;
    public int speed = 20;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Determine if the collision connected with an enemy, and if so determine interaction
        EnemyCombat enemy = collision.GetComponent<EnemyCombat>();
        BossFIght boss = collision.GetComponent<BossFIght>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            impactObj = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impactObj, 0.54f);
            Destroy(gameObject);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(damage);
            impactObj = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(impactObj, 0.54f);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            // Will keep moving the Lightning Bolt Toward the enemy (parent) it is assigned to
            myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            enemyPos = Camera.main.WorldToScreenPoint(target.transform.position);
            Vector3 direction = (enemyPos - myScreenPos).normalized;
            rb.velocity = new Vector2(direction.x, direction.y) * speed;

            // Rotates the Lightning Bolt while its tracking the enemy
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } catch
        {
            Debug.Log("LightningBolt error. Bolt likely targeting an already dead enemy.");
            Destroy(gameObject);
        }
    }
}
