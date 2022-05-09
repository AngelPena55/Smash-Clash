using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float cooldown = 3f;
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 40; // Initial Damage from collision

    public float lightningRadius = 5.7f;
    public LightningBolt lightningBolt;

    public GameObject impactEffect;
    public GameObject impactObj;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().spellRate = cooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child

        // Initiliaze cooldown settings for UI
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTime = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTimer = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().isCooldown = true;

        //GetComponentInChildren<ImpactAudioHandler>();
        Invoke("Deploy", 0.7f);
        Destroy(gameObject, 0.74f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name); // Logs what the object collides with in the console

        // Determine if the fireball connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
            impactObj = Instantiate(impactEffect, enemy.transform.position, transform.rotation); // Create an impact effect on collision
            Destroy(impactObj, 0.54f); // Delete animation after it has ended
        }
        if(boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
            impactObj = Instantiate(impactEffect, boss.transform.position, transform.rotation); // Create an impact effect on collision
            Destroy(impactObj, 0.54f); // Delete animation after it has ended
        }

    }
    void Deploy()
    {
        // Determine if the collision connected with an enemy, and if so determine interaction
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, lightningRadius);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<EnemyCombat>(out EnemyCombat enemy))
            {
                LightningBolt bolt = Instantiate(lightningBolt, transform.position, Quaternion.identity);
                bolt.target = enemy.transform;
            }
            else if (collider2D.TryGetComponent<BossFIght>(out BossFIght boss) && boss.CompareTag("Boss"))
            {
                LightningBolt bolt = Instantiate(lightningBolt, transform.position, transform.rotation, boss.transform);
                bolt.target = boss.transform;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


}
