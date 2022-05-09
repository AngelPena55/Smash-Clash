using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForce : MonoBehaviour
{
    public float force = 6;
    public int damage = 5;
    public GameObject impactFab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Opacity(0f, 0.5f, 0.2f));
        Destroy(gameObject, 0.4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCombat player = collision.GetComponent<PlayerCombat>();
        
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            float multiplier = 7 - Vector3.Distance(player.transform.position, transform.position);
            // Debug.Log("Multiplier = " + multiplier + "\nForce = " + multiplier * direction * force); // DEBUGGING
            if (multiplier < 1) multiplier = 1;
            //enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * force;
            player.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force * multiplier, transform.position, ForceMode2D.Impulse);
            Instantiate(impactFab, player.transform);
            player.PlayertakeDamage(damage);
            //player.GetComponent<PlayerCombat>().crowdControl(0.5f, 1f, 1);
        }

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
        transform.position = this.transform.position;
        if (transform.GetComponent<SpriteRenderer>().color.a == 0.5f)
        {
            StartCoroutine(Opacity(0.5f, 0, 0.1f));
        }
    }
}
