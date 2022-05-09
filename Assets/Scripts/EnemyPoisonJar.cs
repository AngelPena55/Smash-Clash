using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoisonJar : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 1; // Initial Damage from Fireball collision
    public int tickDamage = 5; // Tick damage from Fireball collision
    public int duration = 10;
    public GameObject impactEffect;

    Vector3 myScreenPos;
    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        myScreenPos = this.transform.position;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 direction = (playerPos - myScreenPos).normalized;
        //rb.AddForce(transform.forward * speed, ForceMode2D.Force);
        rb.velocity = new Vector2(direction.x, direction.y) * speed;



    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child

        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the poison jar connected with an enemy, and if so determine interaction
        PlayerCombat enemy = hitInfo.GetComponent<PlayerCombat>();
        if (enemy != null)
        {
            enemy.PlayertakeDamage(initDamage);
            enemy.TakeDotDamage(tickDamage, duration);
        }

        // Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
