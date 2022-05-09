using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowBundleShop : MonoBehaviour
{
    private Transform playerPos;
    private GameObject player;
    private int pwealth;
    public int cost = 1;
    public bool usedUp = false;
    public AudioSource audioSrc;
    public AudioClip dropClip;
    private int addArrows;
    private int totArrows;
    public int maxQuiver;
    public TextMeshPro priceTag;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<AudioSource>().PlayOneShot(dropClip);
        addArrows = Random.Range(1, 10);
        player = GameObject.FindGameObjectWithTag("Player");
        maxQuiver = playerPos.GetComponent<Spell>().maxArrows;
        priceTag.text = "Price: " + cost;
        //GetComponent<AudioSource>().PlayOneShot(dropClip);
    }

    // Update is called once per frame
    void Update()
    {
        pwealth = player.GetComponent<PlayerCombat>().wealth;
        totArrows = playerPos.GetComponent<Spell>().totalArrows;
        //Give player a certain amount of health and use item when the player is 1 unit away from this object
        if (totArrows <= maxQuiver && Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp && cost <= pwealth)
        {
            usedUp = true;
            playerPos.GetComponent<PlayerCombat>().removeCoins(cost);
            if (totArrows + addArrows <= maxQuiver)
            {
                playerPos.GetComponent<Spell>().totalArrows += addArrows;
                playerPos.GetComponent<Spell>().arrowNum.text = "" + playerPos.GetComponent<Spell>().totalArrows;
                Destroy(gameObject, .1f);
            }
            else
            {
                playerPos.GetComponent<Spell>().totalArrows = maxQuiver;
                playerPos.GetComponent<Spell>().arrowNum.text = "" + playerPos.GetComponent<Spell>().totalArrows;
                Destroy(gameObject, .1f);
            }

        }
    }
}
