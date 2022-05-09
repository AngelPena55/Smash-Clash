using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float cooldown = 3f; // How many seconds the cooldown should be
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 40; // Initial Damage from Fireball collision
    public int tickDamage = 2; // Tick damage from Fireball collision
    public int duration = 3;

    public GameObject impactEffect;
    public GameObject impactObj;
    Vector3 myScreenPos;
    public GameObject fireballfab;

    public void Awake()
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
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child

        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the fireball connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
            enemy.TakeDotDamage(tickDamage, duration);
            Destroy(gameObject);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
            boss.TakeDotDamage(tickDamage, duration);
            Destroy(gameObject);
        }
        impactObj = Instantiate(impactEffect, transform.position, Quaternion.identity); // Create an impact effect on collision
        Destroy(impactObj, 1f); // Delete animation after it has ended

    }

    // Update is called once per frame
    void Update()
    {

    }


}
