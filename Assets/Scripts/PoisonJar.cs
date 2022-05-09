using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonJar : MonoBehaviour
{
    public float cooldown = 3f; // How many seconds the cooldown should last
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 1; // Initial Damage from Fireball collision
    public int tickDamage = 5; // Tick damage from Fireball collision
    public int duration = 10;
    public GameObject impactEffect;
    public GameObject impactObj;
    Vector3 myScreenPos;


    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().spellRate = cooldown;

    }
    // Start is called before the first frame update
    void Start()
    {
        // Initiliaze cooldown settings for UI
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTime = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTimer = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().isCooldown = true;

        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 direction = (Input.mousePosition - myScreenPos).normalized;
        //rb.AddForce(transform.forward * speed, ForceMode2D.Force);
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
        Destroy(gameObject, 1.5f);

    }


    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child

        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the poison jar connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
            enemy.TakeDotDamage(tickDamage, duration);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
            boss.TakeDotDamage(tickDamage, duration);
        }
 
        impactObj = Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(impactObj, 0.58f); // Delete animation after it has ended
        Destroy(gameObject); // Delete Poison Jar once its collided with an enemy

    }

    // Update is called once per frame
    void Update()
    {

    }
}
