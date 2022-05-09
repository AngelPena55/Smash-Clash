using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    public float cooldown = 12f;
    public int initDamage = 10;
    public int duration = 10;
    public float elapsed;
    public bool collided;
    EnemyCombat enemy;
    BossFIght boss;
    public GameObject impactEffect;
    public GameObject impactObj;
    public AudioSource audioSource;

    public void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().spellRate = cooldown;
        audioSource = GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    void Start()
    {
        // Initiliaze cooldown settings for UI
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTime = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTimer = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().isCooldown = true;

        impactObj = Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
        Destroy(impactObj, 0.42f); // Destroy impact animation after its done
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collided = true;
        enemy = collision.GetComponent<EnemyCombat>();
        boss = collision.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
        }else if (boss.CompareTag("Boss") && boss != null)
        {
            boss.TakeDamage(initDamage);
        }

        audioSource.Play();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collided = false;
        enemy = null;
    }

    private
    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1.5f)
        {
            elapsed = elapsed % 1.5f;
            if (enemy != null)
            {
                enemy.TakeDamage(initDamage);
            }
            impactObj = Instantiate(impactEffect, transform.position, transform.rotation); // Create an impact effect on collision
            Destroy(impactObj, 0.33f); // Destroy impact animation after its done
        }
    }
}
