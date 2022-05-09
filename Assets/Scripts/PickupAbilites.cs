using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAbilites : MonoBehaviour
{
    private Transform playerPos;
    public bool usedUp = false;
    public AudioSource audioSrc;
    public AudioClip coinClip;
    public int newspell = 0;
    public int currentS;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp)
        {

            usedUp = true;
            currentS = playerPos.GetComponent<Spell>().CurrentSpell;
            playerPos.GetComponent<Spell>().AbilitiesPanel[currentS].SetActive(false);
            playerPos.GetComponent<Spell>().CurrentSpell = newspell;
            Debug.Log(newspell);
            Destroy(gameObject, .1f);
            audioSrc.Play();
        }

    }
}
