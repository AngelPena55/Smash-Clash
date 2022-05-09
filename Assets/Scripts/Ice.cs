using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 40; // Initial Damage from Fireball collision
    public int tickDamage = 2; // Tick damage from Fireball collision
    public int duration = 3;
    public GameObject impactEffect;
    Vector3 myScreenPos;

    // Start is called before the first frame update
    void Start()
    {
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 direction = (Input.mousePosition - myScreenPos).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the fireball connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
            enemy.TakeDotDamage(tickDamage, duration);
        }
        if (boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
            boss.TakeDotDamage(tickDamage, duration);
        }

        // Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
