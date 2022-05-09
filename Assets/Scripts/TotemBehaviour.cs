using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBehaviour : MonoBehaviour
{
    private Transform playerPos;
    public bool usedUp = false;
    public AudioSource audioSrc;
    public AudioClip dropClip;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<AudioSource>().PlayOneShot(dropClip);
    }

    // Update is called once per frame
    void Update()
    {
        //Give player a certain amount of health and use item when the player is 1 unit away from this object
        if (Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp)
        {
            usedUp = true;
            playerPos.GetComponent<PlayerCombat>().addTotem();
            Destroy(gameObject, .1f);
        }
    }
}
