using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnplayer : MonoBehaviour
{
    public  GameObject spawnplayer;
    public GameObject dontdestory;
    public bool iscreated;

    // Start is called before the first frame update
    void Start()
    {

        
    }
    public void Awake()
    {
        DontDestroyOnLoad(dontdestory);
        if(!iscreated)
        {
            Instantiate(spawnplayer, transform.position, Quaternion.identity);
            iscreated = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
