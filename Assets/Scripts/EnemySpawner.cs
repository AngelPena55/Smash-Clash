using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnDist = 30f;
    public Transform itemSpawn;
    public GameObject[] itemArray;
    private GameObject parentObj;
    private Transform parentTrans;
    private Transform playerPos;
    public bool usedUp = false;

    public bool bossUsed = false;
    public int bossMaxH;
    public int bossHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        parentObj = this.transform.parent.gameObject;
        parentTrans = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(playerPos.position, itemSpawn.position) <= spawnDist && !parentObj.CompareTag("StoreRoom") && !usedUp)
        {
            GameObject item = Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation, parentTrans);
            usedUp = true;
            GetComponentInParent<EnemyCounter>().enemyCounter++;
        }
        if (Vector2.Distance(playerPos.position, itemSpawn.position) <= spawnDist && parentObj.CompareTag("BossRoom") && !bossUsed)
        {
            
            bossHealth = parentObj.GetComponentInChildren<BossFIght>().currentHealth;
            bossMaxH = parentObj.GetComponentInChildren<BossFIght>().maxHealth;
            if (bossMaxH / 2 == bossHealth)
            {
                bossUsed = true;
                GameObject item = Instantiate(itemArray[Random.Range(0, itemArray.Length)], itemSpawn.position, itemSpawn.rotation, parentTrans);
                GetComponentInParent<EnemyCounter>().enemyCounter++;
            }
            
        }
    }
}
