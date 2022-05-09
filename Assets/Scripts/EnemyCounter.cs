using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public float enemyCounter;
    public bool bossCheck = false;
    public bool roomVisited = false;
    private Transform playerPos;
    public float spawnDist = 30f;
    public Transform itemSpawn;
    public GameObject[] doors;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Debug.Log(other.gameObject.tag);
         if (enemyCounter > 0 && Vector2.Distance(playerPos.position, itemSpawn.position) <= spawnDist)
        {
            for(int i = 0; i<doors.Length; i++)
            {
                if (doors[i])
                {
                    doors[i].SetActive(true);
                }
                    
            }
            //lock doors
            if (!roomVisited)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().roomsVisited++;
                roomVisited = true;
            }
           
        }
        else if (enemyCounter == 0 && !CompareTag("BossRoom"))
        {
            //unlock door
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i])
                {
                    doors[i].SetActive(false);
                }
                    
            }
        }
         if (CompareTag("BossRoom") && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>().roomsVisited  < GameObject.Find("SpawnedRoomArray").GetComponent<SpawnedRoomsScript>().numberOfTotalRooms)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i])
                {
                    doors[i].SetActive(true);
                }

            }
        }
         else if (CompareTag("BossRoom"))
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i])
                {
                    doors[i].SetActive(false);
                }

            }
        }
    }
}
