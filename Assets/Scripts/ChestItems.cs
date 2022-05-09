using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItems : MonoBehaviour
{
    public Animator animator;
    private Transform playerPos;
    public Transform itemSpawn;
    public GameObject[] itemArray;
    public bool usedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Once player's position is less than or equal to 1 unit and the item is not used up then create a random item from array itemArray itemspawn's position
        if (Vector2.Distance(playerPos.position, transform.position) <= 1f && !usedUp)
        {
            usedUp = true;
            animator.SetTrigger("isOpen");
            GameObject item = Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation);
        }
    }
}
