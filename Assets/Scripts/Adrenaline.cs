using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : MonoBehaviour
{
    public float cooldown = 13f;
    public Transform player;
    public float multiplier;
    public GameObject buffedArrow;
    public float tempS;
    public int tempD;
    public GameObject tempArrow;
    public float duration = 10f;
    public AudioSource audioSource;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Spell>().spellRate = cooldown;
        audioSource = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
        // Initiliaze cooldown settings for UI
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTime = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().cooldownTimer = cooldown;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cooldownRadial>().isCooldown = true;

        player = transform.parent;
        tempS = player.GetComponent<PlayerControl>().moveSpeed;
        player.GetComponent<PlayerControl>().moveSpeed = tempS * 2;
        tempD = player.GetComponent<PlayerCombat>().attackDamage;
        player.GetComponent<PlayerCombat>().attackDamage = tempD * 2;
        tempArrow = player.GetComponent<Spell>().arrowfab;
        player.GetComponent<Spell>().arrowfab = buffedArrow;
        StartCoroutine(BuffDuration());
    }

    IEnumerator BuffDuration()
    {
        yield return new WaitForSeconds(duration);
        audioSource.Play();
        //Any visual effects for buff turning off can go here
        player.GetComponent<PlayerControl>().moveSpeed = tempS; // Return speed to normal
        player.GetComponent<PlayerCombat>().attackDamage = tempD; // Return damage to normal
        player.GetComponent<Spell>().arrowfab = tempArrow; // Return Arrow to normal
        Destroy(gameObject);
    }
}
