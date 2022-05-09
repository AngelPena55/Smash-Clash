using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 40; // Initial Damage from Fireball collision
    public int tickDamage = 2; // Tick damage from Fireball collision
    public int duration = 3;
    public GameObject impactEffect;
    public GameObject impactObj;
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
        try
        {
            transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child
        } catch
        {
            Debug.Log("Impact Audio Manager error. Likely NullReferenceException on object: " + gameObject);
        }
        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the fireball connected with an enemy, and if so determine interaction
        PlayerCombat player = hitInfo.GetComponent<PlayerCombat>();
        if (player != null)
        {
            player.PlayertakeDamage(initDamage);
            player.TakeDotDamage(tickDamage, duration);
            Destroy(gameObject);
        }
        impactObj = Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(impactObj, 1f);
    }

}
