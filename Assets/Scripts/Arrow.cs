using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int initDamage = 10; // Initial Damage from Fireball collision
    public int duration = 3;

    Vector3 myScreenPos;

    public void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 direction = (Input.mousePosition - myScreenPos).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        transform.Find("Impact Audio Manager").GetComponent<ImpactAudioHandler>().Play(); // Play audio on impact audio manager child

        Debug.Log(hitInfo.name); // Logs what the fireball collides with in the console

        // Determine if the fireball connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            enemy.TakeDamage(initDamage);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            boss.TakeDamage(initDamage);
        }
        Destroy(gameObject);
    }

}
