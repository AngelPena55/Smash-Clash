using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIceball : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 20; // Initial Damage from Fireball collision
    public int freezeDuration = 3;
    public int slowDuration = 3;
    public float slow = 0.3f; // The fraction of the enemy's speed you want it to be slowed to

    public GameObject impactEffect;
    Vector3 myScreenPos;
    private Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        playerPos = Camera.main.WorldToScreenPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
        Vector3 direction = (playerPos - myScreenPos).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
        Destroy(gameObject, 1.25f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the iceball connected with an enemy, and if so determine interaction
        PlayerCombat enemy = hitInfo.GetComponent<PlayerCombat>();
        if (enemy != null)
        {
            transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child
            enemy.PlayertakeDamage(initDamage);
            Destroy(gameObject);
        }

        // Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision

    }

    // Update is called once per frame
    void Update()
    {

    }
}
