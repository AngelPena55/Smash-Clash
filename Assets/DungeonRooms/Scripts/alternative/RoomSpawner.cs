using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{



    public int openingDirection;
    // 1 = bottom dorr
    // 2 = top door
    // 3 = left door
    // 4 = right door

    public SpawnedRoomsScript spawnedScript;
    private RoomTemplates templates;
    private int randomNumber;
    private bool spawned = false;
    int numberOfRooms;
    

    //needed to be a seperate script, because there are dozens of instances of RoomSpawner
    // having one public size variable is easier to edit than dozens of seperate size variables inside each script instance
    private LevelSizeScript script; 


    //DoorDestroyer doorDestroyer;


    //bool isColliding = false;

    
    bool levelLoaded = false;


    // Start is called before the first frame update
    void Start()
    {


        script = GameObject.Find("LevelSize").GetComponent<LevelSizeScript>();
        spawnedScript = GameObject.Find("SpawnedRoomArray").GetComponent<SpawnedRoomsScript>();

        templates = GameObject.FindGameObjectWithTag("RoomArray").GetComponent<RoomTemplates>();

        



        
    }

    void FixedUpdate(){
        
       

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");     

        if(spawnedScript.numberOfTotalRooms < script.size){
            Spawn();

            //levelLoaded = true;
                
        }
      
        

        
        
    }

    
    void Spawn()
    {

        // checks the opening direction value of each spawnpoint object
        // selects random room prefab from appropriate list
        // instantiates selected room at the location of the spawnpoint

       

        if(openingDirection == 1){
            // bottom
            randomNumber = Random.Range(0,templates.bottomRooms.Length);
            Instantiate(templates.topRooms[randomNumber], transform.position, templates.topRooms[randomNumber].transform.rotation);
                
        }

        else if(openingDirection == 2){
            // top
            randomNumber = Random.Range(0,templates.topRooms.Length);
            Instantiate(templates.bottomRooms[randomNumber], transform.position, templates.bottomRooms[randomNumber].transform.rotation);
                
        }

        else if(openingDirection == 3){
            // left
            randomNumber = Random.Range(0,templates.leftRooms.Length);
            Instantiate(templates.rightRooms[randomNumber], transform.position, templates.rightRooms[randomNumber].transform.rotation);
                
        }

        else if(openingDirection == 4){
            // right
            randomNumber = Random.Range(0,templates.rightRooms.Length);
            Instantiate(templates.leftRooms[randomNumber], transform.position, templates.leftRooms[randomNumber].transform.rotation);
                
        }


      
              
        
    }//end spawn


    void OnTriggerEnter2D(Collider2D other){

        
        if(other.CompareTag("RoomSpawnPoint")) { //if the spawnpoint is intersecting with another spawnpoint, delete

            Destroy(gameObject); 

        }
        

        if(other.CompareTag("RoomCenter")) {

            Destroy(gameObject); //destroys spawnpoint once a room is created, so more rooms won't spawn

        }

    }


}// end
