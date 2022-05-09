using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEarthquake : MonoBehaviour
{
    public int initDamage = 10;
    public int duration = 10;
    public float elapsed;
    public bool collided;
    PlayerCombat enemy;
    public AudioSource audioSource;
    public float clipLength = 0;
    public float clipduration = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collided = true;
        enemy = collision.GetComponent<PlayerCombat>();
        if (enemy != null)
        {
            enemy.PlayertakeDamage(initDamage);
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
                enemy.PlayertakeDamage(initDamage);
            }
        }
    }
}
