using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Iceball : MonoBehaviour
{
    public float cooldown = 3f; // How many seconds the cooldown should be
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 20; // Initial Damage from Fireball collision
    public int freezeDuration = 3;
    public int slowDuration = 3;
    public float slow = 0.3f; // The fraction of the enemy's speed you want it to be slowed to

    public GameObject impactEffect;
    public GameObject impactObj;
    Vector3 myScreenPos;

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

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child
        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the iceball connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
            enemy.Freeze(slow, freezeDuration, slowDuration);
            Destroy(gameObject);
        } else if(boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
            boss.Freeze(slow, freezeDuration, slowDuration);
            Destroy(gameObject);
        }

        impactObj = Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(impactObj, 0.41f); // Destroy impact animation after its done
    }

    // Update is called once per frame
    void Update()
    {

    }
}
