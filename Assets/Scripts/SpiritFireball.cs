using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFireball : MonoBehaviour
{
    public GameObject impactEffect;
    public GameObject impactObj;

    public int damage = 11; // How much initial damage on collision
    public int dotDamage = 3; // How much damage the DoT effect should do per tick
    public int duration = 3; // How long the DoT Damage should take to complete
    public int speed = 20; // Projectile speed

    private Vector3 myScreenPos;
    private Vector3 enemyPos;
    public Rigidbody2D rb;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Determine if the collision connected with an enemy, and if so determine interaction
        EnemyCombat enemy = collision.GetComponent<EnemyCombat>();
        BossFIght boss = collision.GetComponent<BossFIght>();

        try
        {
            if (collision.gameObject.tag == "Enemy")
            {
                enemy.TakeDamage(damage);
                enemy.TakeDotDamage(dotDamage, duration);
                impactObj = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(impactObj, 0.66f);
                Destroy(gameObject);
            }
            else if (boss != null && boss.CompareTag("Boss"))
            {
                boss.TakeDamage(damage);
                boss.TakeDotDamage(dotDamage, duration);
                impactObj = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(impactObj, 0.66f);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        } catch
        {
            Debug.Log("Spirit Fireball error. Likely targeting enemy thats already dead.");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            // Will keep moving the Spirit Fireball Toward the enemy (parent) it is assigned to
            myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            enemyPos = Camera.main.WorldToScreenPoint(target.transform.position);
            Vector3 direction = (enemyPos - myScreenPos).normalized;
            rb.velocity = new Vector2(direction.x, direction.y) * speed;

            // Rotates the Spirit Fireball while its tracking the enemy
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } catch
        {
            Debug.Log("Spirit Fireball Error. Fireball likely targerting an already dead enemy");
        }
    }
}
