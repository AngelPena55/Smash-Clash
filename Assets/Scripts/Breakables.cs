using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public int health = 1;
    public GameObject[] Droppables;
    public int active = 0;
    // add a audio componet during polish phase

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks collion anything that the player does
        if(collision.CompareTag("Player"))
        {
            health--;
            if(health <=0)
            {
                ExplodeObject();
            }
        }
    }

    // drop an item and destory the barrel.
    private void ExplodeObject()
    {
        active = 1;

        Destroy(gameObject);
        DropItem();
    }

    private void DropItem()
    {
        if (active == 1)
        {
            // as time goes on we need to add a random capability of then getting a % chance of a drop
            // itterates through droppable items and select 1 
            int indexNumber = Random.Range(0, Droppables.Length);
            Vector2 position = transform.position;
            GameObject droppeditem = Instantiate(Droppables[indexNumber], position, Quaternion.identity);
            active = 0;
        }
        //for random item drop you can use Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation) just make an array of items and a spawn point
    }
 
}
