using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfBlades : MonoBehaviour
{
    public float cooldown = 3.5f;
    public float speed = 150;
    public int damage = 10;
    public float force = 7; // How much force to apply on collision
    public SpriteRenderer spriteRenderer;
    public Sprite[] newSprite;
    public GameObject bladeFab1;
    public GameObject bladeFab2;
    public int currentSprite;
    public GameObject impactFab;
    public AudioSource audioSource;
    public float ringRadius = 2.6f; // How large the radius for collision should be
    private float ringCollisionCooldown = 0.1f;
    private float totalTime;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().spellRate = cooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initiliaze cooldown settings for UI
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTime = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTimer = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().isCooldown = true;

        currentSprite = 0;
        totalTime = 0f;
        Invoke("Disperse", 10f);
    }
    

    void Disperse()
    {
        for (; currentSprite < 7; currentSprite++) 
        {
            if (currentSprite < 4)
            {
                Instantiate(bladeFab1, transform.position, Quaternion.identity);
                spriteRenderer.sprite = newSprite[currentSprite];
            }
            else if (currentSprite < 7)
            {
                Instantiate(bladeFab2, transform.position, Quaternion.identity);
                spriteRenderer.sprite = newSprite[currentSprite];
            }
        }
        Instantiate(bladeFab2, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D hitInfo)
    {
        totalTime += Time.deltaTime;
        if (totalTime >= ringCollisionCooldown)
        {
            // Determine if the fireball connected with an enemy, and if so determine interaction
            EnemyCombat enemy = hitInfo.GetComponent<EnemyCombat>();
            BossFIght boss = hitInfo.GetComponent<BossFIght>();
            if (enemy != null)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                // Debug.Log("Multiplier = " + multiplier + "\nForce = " + multiplier * direction * force); // DEBUGGING
                enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * force;
                //enemy.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force * multiplier, transform.position, ForceMode2D.Impulse);
                Instantiate(impactFab, enemy.transform.position + new Vector3(1f, 1f, 0) * Random.Range(-0.1f, 0.1f), transform.rotation);
                if (currentSprite < 7)
                {
                    spriteRenderer.sprite = newSprite[currentSprite];
                    currentSprite++;
                    enemy.TakeDamage(damage);
                    enemy.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
                }
                else
                {
                    Debug.Log("Destroy ring of blades...");
                    enemy.TakeDamage(damage);
                    enemy.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
                    Destroy(gameObject);
                }

            }
            else if (boss != null && boss.CompareTag("Boss"))
            {
                Vector3 direction = (boss.transform.position - transform.position).normalized;
                float multiplier = 7 - Vector3.Distance(boss.transform.position, transform.position);
                boss.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * force;
                //boss.GetComponent<Rigidbody2D>().AddForceAtPosition(direction * force * multiplier, transform.position, ForceMode2D.Impulse);
                Instantiate(impactFab, boss.transform.position + new Vector3(1f, 1f, 0) * Random.Range(-0.1f, 0.1f), transform.rotation);
                if (currentSprite < 7)
                {
                    spriteRenderer.sprite = newSprite[currentSprite];
                    currentSprite++;
                    boss.TakeDamage(damage);
                    boss.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
                }
                else
                {
                    boss.TakeDamage(damage);
                    boss.GetComponent<EnemyCombat>().crowdControl(0.5f, 1f, 1);
                    Destroy(gameObject);
                }

            }
            audioSource.Play();
            totalTime = 0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if a Ring of Blades ability already exists, and if so, Disperse the oldest version immediately
        GameObject[] cloneCheck = GameObject.FindGameObjectsWithTag("Ring Of Blades");
        if (cloneCheck.Length >= 2)
        {
            cloneCheck[0].GetComponent<RingOfBlades>().Disperse();
        }

        // Rotate the sprite around the player at variable "speed"
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
        
        // Fire a blade on mouse click and change the sprite to account for missing blade
        if (Input.GetMouseButtonDown(1))
        {
            if (currentSprite < 4)
            {
                Instantiate(bladeFab1, transform.position, Quaternion.identity);
                spriteRenderer.sprite = newSprite[currentSprite];
            } 
            else if (currentSprite < 7)
            {
                Instantiate(bladeFab2, transform.position, Quaternion.identity);
                spriteRenderer.sprite = newSprite[currentSprite];
            }
            else
            {
                Instantiate(bladeFab2, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            currentSprite++;
        }
    }
}
