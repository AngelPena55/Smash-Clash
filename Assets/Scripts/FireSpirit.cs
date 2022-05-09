using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSpirit : MonoBehaviour
{
    public float cooldown = 15f; // How many seconds the spell cooldown should be
    public float nextAttack = 1f; // How many seconds before the spirit can attack again
    private float timePassed;

    private Vector2 center;
    public float spiritRadius = 7f;
    public float rotationRadius = 0.45f;
    public float rotateSpeed = 5f;
    private float angle;
    public AudioSource audioSource;
    public AudioClip fireballClip;

    public SpiritFireball spiritFireball;

    private void Awake()
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

        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 2.1f, 0);
        timePassed = 100f; // Set timePassed to either 0 or a value larger than nextAttack to determine whether to spawn attack on first frame
        Destroy(gameObject, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime; // Increment timePassed every frame

        // Have the sprite moving in a circular path above the player's head
        center = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 2.1f, 0);
        angle += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * rotationRadius;
        transform.position = center + offset;

        // Check if enough time has passed for the next attack to occur, and if so check for enemy collisions
        if (timePassed > nextAttack)
        {
            // Determine if the collision connected with an enemy, and if so determine interaction
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, spiritRadius);
            foreach (Collider2D collider2D in colliderArray)
            {  
                if (collider2D.TryGetComponent<EnemyCombat>(out EnemyCombat enemy))
                {
                    audioSource.PlayOneShot(fireballClip);
                    SpiritFireball fireball = Instantiate(spiritFireball, transform.position, Quaternion.identity);
                    fireball.target = enemy.transform;
                }
                else if (collider2D.TryGetComponent<BossFIght>(out BossFIght boss) && boss.CompareTag("Boss"))
                {
                    audioSource.PlayOneShot(fireballClip);
                    SpiritFireball fireball = Instantiate(spiritFireball, transform.position, transform.rotation, boss.transform);
                    fireball.target = boss.transform;
                }
            }
            timePassed = 0f;
        }
    }
}
