using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class BossFIght : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    Animator animator;
    public float time = 1;
    public int CurrentSpell;
    public bool onetimespell = false;


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
    public GameObject[] BossSpells;

    public AudioSource audioSrc;
    public AudioClip swordswing;
    public AudioClip hurt;
    public AudioClip death;

    //things for items
    public Transform loot;
    public Transform spell;
    public GameObject[] Droppables;
    public GameObject[] SpellDrops;
    public GameObject Portal;

    public float timeElapsed;
    public string newscene;
    public Transform portalnewscene;

  

    // Start is called before the first frame update
    void Start()
    {
        GenerateBossSpell();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void GenerateBossSpell()
    {
        // happens only once 
        if (onetimespell == false)
        {
            CurrentSpell = Random.Range(0, BossSpells.Length);
            //CurrentSpell = 2; // DEBUGGING
            onetimespell = true;
            spellPrefab = BossSpells[CurrentSpell];
        }
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
        if (currentHealth <= 0 && !enemyDeath)
        {
            GetComponent<AudioSource>().PlayOneShot(death);
            enemyDeath = true;
            Die();

            Invoke("LoadNewScene", 5f);

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
            GetComponent<AudioSource>().PlayOneShot(hurt);
            //character health is reduced by damage
            currentHealth -= damage;
            // animator.SetTrigger("isAttacked"); // If enemy should display a animation for taking DoT damage

            Debug.Log("DotDamage Health");
            Debug.Log(currentHealth);

            //If the health reaches 0 call Die()
            if (currentHealth <= 0 && !enemyDeath)
            {
                enemyDeath = true;
                Die();
                yield return new WaitForSeconds(5);
            }

            yield return new WaitForSeconds(1);



        }
    }

    //AbilityActivated is called when the character's ability animation is called
    //--------------------------------------------------------------------------------------------------------
    public void AbilityActivated()
    {
        GameObject shoot;
        //Creates the prefab for firing the ability and destroy it after a few seconds
        if (spellPrefab.name == "Enemy 04_Earthquake Variant" || spellPrefab.name == "05_Lightning Enemy")
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            shoot = Instantiate(spellPrefab, playerPos.position, Quaternion.identity);
        }
        else
        {
            shoot = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        }
        Destroy(shoot, .5f);

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
        // When the Bosses health == to 0 they dropitems 
        animator.SetTrigger("isDying");
        Destroy(gameObject, time);

        DropItems();
        
       // Invoke(nameof(LoadNewScene), 5.0f);







    }
    void LoadNewScene()
    {
        SceneManager.LoadScene(newscene);
    }

    private void DropItems()
    {
        // Here is where the boss drops his goodies and a piller to go the next level
        int Droptable = Random.Range(0, Droppables.Length);
        int currnetbossdrop = CurrentSpell;
        Vector2 position = transform.position;
        transform.position = new Vector2(position.x + 5, position.y + 5);
        Vector2 waypoint = new Vector2(position.x - 5, position.y - 5);
        
        GameObject item = Instantiate(Droppables[Droptable], transform.position, Quaternion.identity);
        GameObject lootspell = Instantiate(SpellDrops[currnetbossdrop], position, Quaternion.identity);

        // this spawns a portal to go to the next level
        GameObject newlevel = Instantiate(Portal, waypoint, Quaternion.identity);



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
           // Debug.Log("isTrigger: " + GetComponent<Collider2D>().isTrigger);
        }
        else
        {
            triggered = true;
            GetComponent<Collider2D>().isTrigger = true;
           // Debug.Log("isTrigger: " + GetComponent<Collider2D>().isTrigger);
        }
    }

}
