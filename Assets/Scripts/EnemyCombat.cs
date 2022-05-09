using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    Animator animator;
    public float time = 1;
    private float audioTimer = 0;

    public Transform attackpointright;
    public Transform attackpointup;
    public Transform attackpointleft;
    public Transform attackpointdown;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 20;

    public float enemyCooldown = 1f;
    public bool canAttack = true;

    public float attackTime = 1f;
    public bool takeDamage = false;
    public float attackDirection;

    private bool enemyDeath = false;
    public GameObject spellPrefab;
    public Transform firePoint;

    public AudioSource audioSrc;
    public AudioClip swordswing;
    public AudioClip hurt;
    public AudioClip death;
    public GameObject parentObj;



    // Start is called before the first frame update
    void Start()
    {
        parentObj = this.transform.parent.gameObject;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // TakeDamage is called whenever the character is attacked
    //-------------------------------------------------------------------------------------------------------------
    public void TakeDamage(int damage)
    {
        GetComponent<AudioSource>().PlayOneShot(hurt);
        //character health is reduced by damage
        currentHealth -= damage;
        animator.SetTrigger("isAttacked");
        
        Debug.Log("Damage Health");
        Debug.Log(currentHealth);

        //If the health reaches 0 call Die()
        if(currentHealth <= 0 && !enemyDeath)
        {
            GetComponent<AudioSource>().PlayOneShot(death);
            enemyDeath = true;
            Die();
        }
    }

    // TakeDotDamage is called whenever the character is attacked with a move that damages over time
    //------------------------------------------------------------------------------------------------------------
    public void TakeDotDamage(int damage, int duration)
    {
        //Call to timer
        StartCoroutine(waiter(damage, duration));
    }

    //TakeDotDamage() Timer
    //-----------------------------------------------------------------------------------------------------------
    IEnumerator waiter(int damage, int duration)
    {
        for (int i = 0; i < duration; i++)
        {
            // If at least 1 second has passed between the last time the audio clip has been played, allow it to play again
            if(audioTimer > 1f)
            {
                GetComponent<AudioSource>().PlayOneShot(hurt);
                audioTimer = 0;
            }
            //character health is reduced by damage
            currentHealth -= damage;
            animator.SetTrigger("isAttacked"); // If enemy should display a animation for taking DoT damage

            Debug.Log("DotDamage Health");
            Debug.Log(currentHealth);

            //If the health reaches 0 call Die()
            if (currentHealth <= 0 && !enemyDeath)
            {
                enemyDeath = true;
                Die();
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    //AbilityActivated is called when the character's ability animation is called
    //--------------------------------------------------------------------------------------------------------
    public void AbilityActivated()
    {
        //Creates the prefab for firing the ability and destroy it after a few seconds
        if (spellPrefab.name != "05_Lightning Enemy" && spellPrefab.name != "Enemy 04_Earthquake Variant")
        {
            GameObject shoot = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            Destroy(shoot, .5f);
        }
        else
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject shoot = Instantiate(spellPrefab, playerPos.position, Quaternion.identity);
            Destroy(shoot, .5f);
        }
       

    }

    //crowdControl is called when an attack has an area of effect slowing or weakness function
    //--------------------------------------------------------------------------------------------------------
    public void crowdControl(float slow, float weakness, int duration)
    {
        //Call to timer
        StartCoroutine(ccWaiter(slow, weakness, duration));
    }

    //CrowdControl() Timer
    //--------------------------------------------------------------------------------------------------------
    IEnumerator ccWaiter(float slow, float weakness, int duration)
    {
        float tempS = GetComponent<Animator>().GetBehaviour<followBehaviour>().speed;
        float tempD = attackDamage;

        //Get the enemies' speed and reduce it
        GetComponent<Animator>().GetBehaviour<followBehaviour>().speed = tempS * slow;

        //Reduce the enemies' attack
        attackDamage = (int)(attackDamage * weakness);
        
        for (int i = 0; i < duration; i++)
        {
            yield return new WaitForSeconds(1);
        }

        //return the speed and attack to normal
        GetComponent<Animator>().GetBehaviour<followBehaviour>().speed = tempS;
        attackDamage = (int)tempD;
    }

    //freeze is called when an attack has a freezing effect
    //--------------------------------------------------------------------------------------------------------
    public void Freeze(float slow, int freezeDuration, int slowDuration)
    {
        StartCoroutine(freezeWaiter(slow, freezeDuration, slowDuration));
    }

    //Freeze() timer
    //--------------------------------------------------------------------------------------------------------
    IEnumerator freezeWaiter(float slow, int freezeDuration, int slowDuration)
    {
        float tempS = GetComponent<Animator>().GetBehaviour<followBehaviour>().speed;

        GetComponent<Animator>().GetBehaviour<followBehaviour>().speed = tempS * 0;
        for (int i = 0; i < freezeDuration; i++)
        {
            yield return new WaitForSeconds(1);
        }
        GetComponent<Animator>().GetBehaviour<followBehaviour>().speed = tempS * slow;
        for (int i = 0; i < freezeDuration; i++)
        {
            yield return new WaitForSeconds(1);
        }
        GetComponent<Animator>().GetBehaviour<followBehaviour>().speed = tempS;
    }

    //EnemyDealDamage is called whenever the enemy is close to the player and its attack hits
    //--------------------------------------------------------------------------------------------------------
    public void EnemyDealDamage()
    {
        //Check if the enemy can attack again
        if (canAttack)
        {
            GetComponent<AudioSource>().PlayOneShot(swordswing);
            //Gather any objects that are not on the enemy layers within the range of the character
            Collider2D[] hitEnemiesright = Physics2D.OverlapCircleAll(attackpointright.position, attackRange, ~enemyLayers);
            Collider2D[] hitEnemiesup = Physics2D.OverlapCircleAll(attackpointup.position, attackRange, ~enemyLayers);
            Collider2D[] hitEnemiesdown = Physics2D.OverlapCircleAll(attackpointdown.position, attackRange, ~enemyLayers);



            //show the damage dealt
        
                foreach (Collider2D player in hitEnemiesright)
                {
                   //Call a timer for attack wind up     
                    StartCoroutine(attackWindUp());

                    if (takeDamage)
                    {
                        //deal damage to any players in the right side of the attack
                        player.GetComponent<PlayerCombat>().PlayertakeDamage(attackDamage);
                        Debug.Log("right");
                    }
                    break;
                }
            foreach (Collider2D player in hitEnemiesup)
            {
                //Call a timer for attack wind up   
                StartCoroutine(attackWindUp());
                if (takeDamage)
                {
                    //deal damage to any players in the left side of the attack
                    player.GetComponent<PlayerCombat>().PlayertakeDamage(attackDamage);
                    Debug.Log("up");
                }
                break;
            }
            foreach (Collider2D player in hitEnemiesdown)
            {
                //Call a timer for attack wind up   
                StartCoroutine(attackWindUp());
                if (takeDamage)
                {
                    //deal damage to any players in the down side of the attack
                    player.GetComponent<PlayerCombat>().PlayertakeDamage(attackDamage);
                    Debug.Log("down");
                }
                break;
            }
            //Cooldown timer
            StartCoroutine(coolDown());
        }
    }

    //Timer for attack wind up
    //--------------------------------------------------------------------------------------------------------
    IEnumerator attackWindUp()
    {

        yield return new WaitForSeconds(attackTime);
        Debug.Log("DamageTime");
        takeDamage = true;
    }

    //Timer for attack cool down
    //--------------------------------------------------------------------------------------------------------
    IEnumerator coolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(enemyCooldown);
        canAttack = true;
    }

    //Die gets called whenever the health is less than or equal to 0 
    //--------------------------------------------------------------------------------------------------------
    void Die()
    {
        GetComponentInParent<EnemyCounter>().enemyCounter--;
        animator.SetTrigger("isDying");
        Destroy(gameObject, time);

       
    }

    //Sets the last direction the enemy attacked
    //--------------------------------------------------------------------------------------------------------
    public void SetDirection(float lastDirection)
    {
        attackDirection = lastDirection;
    }

    //Draws the attack range in the scene view
    //--------------------------------------------------------------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        if (attackpointright == null)

            return;
        Gizmos.DrawWireSphere(attackpointright.position, attackRange);

        if (attackpointup == null)

            return;
        Gizmos.DrawWireSphere(attackpointup.position, attackRange);

        if (attackpointdown == null)
            return;
        Gizmos.DrawWireSphere(attackpointdown.position, attackRange);
    }
    void Update()
    {
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        bool triggered = true;
        if (Vector2.Distance(playerPos.position, this.transform.position) <= 3f && triggered)
        {
            triggered = false;
            GetComponent<Collider2D>().isTrigger = false;
            //Debug.Log("isTrigger: "+ GetComponent<Collider2D>().isTrigger);
        }
        else
        {
            triggered = true;
            GetComponent<Collider2D>().isTrigger = true;
            //Debug.Log("isTrigger: " + GetComponent<Collider2D>().isTrigger);
        }
        audioTimer += Time.deltaTime;
    }

}
