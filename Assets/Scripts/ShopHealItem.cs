using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopHealItem : MonoBehaviour
{
    private Transform playerPos;
    private GameObject player;
    private int pwealth;
    public int cost = 1; 
    public int health = 20;
    public bool usedUp = false;
    public AudioSource audioSrc;
    public AudioClip dropClip;
    public TextMeshPro priceTag;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        priceTag.text = "Price: " + cost;
        //GetComponent<AudioSource>().PlayOneShot(dropClip);
    }

    // Update is called once per frame
    void Update()
    {
        pwealth = player.GetComponent<PlayerCombat>().wealth;
        //Debug.Log(pwealth);
        //Give player a certain amount of health and use item when the player is 1 unit away from this object
        if (Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp && cost <= pwealth)
        {
            usedUp = true;
            playerPos.GetComponent<PlayerCombat>().removeCoins(cost);
            playerPos.GetComponent<PlayerCombat>().PlayerReceiveHealth(health);
            Destroy(gameObject, .1f);
        }
    }
}
