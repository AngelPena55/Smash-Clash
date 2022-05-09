using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    public float force = 3;
    public int damage = 5;
    public GameObject impactFab;
    public AudioSource audioSource;


    public float clipLength = 0;
    public float clipduration = 0f;
    public bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Opacity(0f, 0.5f, 0.2f));
        Destroy(gameObject, 0.4f);
    }
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCombat enemy = collision.GetComponent<EnemyCombat>();
        BossFIght boss = collision.GetComponent<BossFIght>();
        if (enemy != null)
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            float multiplier = 7 - Vector3.Distance(enemy.transform.position, transform.position);
            // Debug.Log("Multiplier = " + multiplier + "\nForce = " + multiplier * direction * force); // DEBUGGING
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * force * Mathf.Clamp(multiplier, 1f, 7f);
            //enemy.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force * multiplier, transform.position, ForceMode2D.Impulse);
            Instantiate(impactFab, enemy.transform);
            enemy.TakeDamage(damage);
            enemy.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            Vector3 direction = (boss.transform.position - transform.position).normalized;
            float multiplier = 7 - Vector3.Distance(boss.transform.position, transform.position);
            boss.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * force * Mathf.Clamp(multiplier, 1f, 7f);
            //boss.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force * multiplier, transform.position, ForceMode2D.Impulse);
            Instantiate(impactFab, boss.transform.position, Quaternion.identity, boss.transform);
            boss.TakeDamage(damage);
            boss.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
        }

        audioSource.Play();
        clipLength = audioSource.clip.length;
        collided = true;
    }

    IEnumerator Opacity(float start, float end, float duration)
    {
        Color tmp = transform.GetComponent<SpriteRenderer>().color;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            tmp.a = Mathf.Lerp(start, end, normalizedTime);
            transform.GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }
        tmp.a = end; //without this, the value will end at something like 0.9992367
        transform.GetComponent<SpriteRenderer>().color = tmp;
    }
    private void Update()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        if (transform.GetComponent<SpriteRenderer>().color.a == 0.5f)
        {
            StartCoroutine(Opacity(0.5f, 0, 0.2f));
        }

        if (collided)
        {
            if (audioSource.time >= clipLength)
            {
                Destroy(gameObject);
            }
        }
    }
}
