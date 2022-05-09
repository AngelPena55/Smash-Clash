using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObjects : MonoBehaviour
{
    public int health = 1;
    public int active = 0;
    public Animator animator;
    public Transform ExplosionPoint;
    public LayerMask layers;
    public float ExplosionRange = 0.5f;
    public int attackDamage = 20;
    public AudioSource audioSrc;
    public AudioClip explosion;
    public GameObject light;
    //Checks for collision against object
    //--------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            health--;
            if (health <= 0)
            {
                ExplodeObject();
            }
        }
    }

    //ExplodeObject is called whenever health of object is reduced to 0
    //--------------------------------------------------------------------------------------------------------
    private void ExplodeObject()
    {
        
        active = 1;
        animator.SetTrigger("Ignited");
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(ExplosionPoint.position, ExplosionRange, ~layers);

        //Deals damage to both enemies and players within a range
        foreach (Collider2D entity in hitEntities)
        {
            if (entity.tag == "Enemy")
            {
                animator.SetTrigger("isAttacked");
                entity.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
            }

            if (entity.tag == "Player")
            {
                entity.GetComponent<PlayerCombat>().PlayertakeDamage(attackDamage);
            }
        }
            light.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(explosion);
            Destroy(gameObject, 1.3f);
        
    }

    // ondraw gizmos selected is the attack range trasform to see in the inspecter.
    //Draws the attack range in the scene view
    //--------------------------------------------------------------------------------------------------------
    private void OnDrawGizmosSelected()
    {

        if (ExplosionPoint == null)

            return;
        Gizmos.DrawWireSphere(ExplosionPoint.position, ExplosionRange);

    }

}
