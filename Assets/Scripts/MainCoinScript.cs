using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCoinScript : MonoBehaviour
{
    private Transform playerPos;
    public int value = 1;
    public bool usedUp = false;
    public AudioSource audioSrc;
    public AudioClip coinClip;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<AudioSource>().PlayOneShot(coinClip);
    }

    // Update is called once per frame
    void Update()
    {
        //increase player's wealth by the value of the object
        if (Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp)
        {
            
            usedUp = true;
            playerPos.GetComponent<PlayerCombat>().AddCoins(value);
            Debug.Log(value);
            Destroy(gameObject, .1f);
        }
    }
}
