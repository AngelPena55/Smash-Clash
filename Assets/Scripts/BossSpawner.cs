using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{

    public Transform itemSpawn;
    public GameObject[] itemArray;
    public bool spawnedIn =false;
    private Transform parentTrans;

    void Start()
    {
        parentTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
         if (this.CompareTag("BossRoom") && !spawnedIn)
        {
            GameObject item = Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation, parentTrans);
            spawnedIn = true;
            GetComponent<EnemyCounter>().bossCheck = true;
        }
    }
}
