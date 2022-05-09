using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightning : MonoBehaviour
{
    public Rigidbody2D rb;
    public int initDamage = 40; // Initial Damage from collision

    public float lightningRadius = 5.7f;
    public EnemyLightningBolt lightningBolt;

    public GameObject impactEffect;
    public GameObject impactObj;

    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child
        Invoke("Deploy", 0.7f);
        Destroy(gameObject, 0.74f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name); // Logs what the object collides with in the console

        // Determine if the fireball connected with an player, and if so determine interaction
        PlayerCombat player = hitInfo.GetComponent<PlayerCombat>();
        
        if (player != null)
        {
            player.PlayertakeDamage(initDamage);
            impactObj = Instantiate(impactEffect, player.transform.position, transform.rotation); // Create an impact effect on collision
            Destroy(impactObj, 0.54f); // Delete animation after it has ended
        }
    }

    void Deploy()
    {
        // Determine if the collision connected with an enemy, and if so determine interaction
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, lightningRadius);
        foreach (Collider2D collider2D in colliderArray)
        {
            if (collider2D.TryGetComponent<PlayerCombat>(out PlayerCombat player))
            {
                EnemyLightningBolt bolt = Instantiate(lightningBolt, transform.position, Quaternion.identity);
                bolt.target = player.transform;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
