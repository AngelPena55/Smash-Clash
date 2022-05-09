using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBlade : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;

    public GameObject impactFab;
    public Rigidbody2D rb;
    private Vector3 myScreenPos;
    // Start is called before the first frame update
    void Start()
    {
        // Have the blade fly towards where on the screen the mouse is clicked
        myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 direction = (Input.mousePosition - myScreenPos).normalized;
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("FlyingBlade Connected with: " + hitInfo.name); // Logs what the blade collides with in the console

        // Determine if the blade connected with an enemy, and if so determine interaction
        EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
        BossFIght boss = hitInfo.GetComponent<BossFIght>();
        if (enemy != null)
        {
            Instantiate(impactFab, enemy.transform.position + new Vector3(1.1f, 1f, 0) * Random.Range(-0.1f, 0.3f), transform.rotation); // Spawn the impact affect with slight randomness on position
            enemy.TakeDamage(damage);
        }
        else if (boss != null && boss.CompareTag("Boss"))
        {
            Instantiate(impactFab, boss.transform.position + new Vector3(1.1f, 1f, 0) * Random.Range(-0.1f, 0.3f), transform.rotation);
            boss.TakeDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Flip the object to correctly face the direction it is flying towards
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
