using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    public Transform itemSpawn;
    public GameObject[] itemArray;
    public bool spawnedIn = false;

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("StoreRoom") && !spawnedIn)
        {
            GameObject item = Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation);
            spawnedIn = true;
        }
    }
}
