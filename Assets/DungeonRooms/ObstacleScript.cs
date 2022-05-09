using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{


    //array of object prefabs to select to spawn
    public GameObject[] objects;

    public int minimumObjects;
    public int maximumObjects;

    public GameObject[] inScene;

    //select randomly from array



    // I want to have a degree of randomness to the randomness
    //ya know
    // what I mean is, each room has its own obstacle grid object.
    //and each object has its own ObstacleScript
    // SO
    // there will be a value to select how many random objects to spawn in a room
    // that value is randomly generated at the start of the script.
    // SO that means each script will generate its own value, meaning each room can have different number of objects spawning.
    // this might help things look less same-y



    //this script can do more than just obstacles like rocks and barrels
    // it can also be used to place decorations and eye candy
    // basically stuff that you don't have to walk around

    // obstacles = can't walk through it
    // decorations = can walk through it
    // so stuff like different floor textures and sprites, bloodsplatter?, those cool S things I always drew in elementary school, stuff like that
    



    // gizmo to help visualize the space that objects can spawn in
    // it is not the full size of the room, we want there to be a buffer between objects and the walls
    //so they won't spawn directly in front of doors
    
    /*
    void OnDrawGizmos() {
    

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(25, 15, 1));

        
    }
    */


    void Start(){

        //int[]

        // generate random number
        int rnd = Random.Range(minimumObjects, maximumObjects); //between 3 and 10 objects sounds reasonable


        // select that many objects from array of possible objects
        for(int i = 0; i<rnd; i++){ //loop for all objects

            int objectToUse = Random.Range(0, objects.Length-1); //index for object
                                                            // need to subtract 1 or index-out-of-bounds

            // select random x and y positions around the room (within set limits)
            
            int x = Random.Range(-12, 12);
            int y = Random.Range(-7, 7);


            //snap object position to even numbers
            if (x % 2 != 0)
                x += 1;
            
            if (y % 2 != 0)
                y += 1;

            
            Vector3 randomPosition = new Vector3(x, y, 10); //pick a position
            
            // instantiate selected object at that x and y
            GameObject thingy = Instantiate(objects[objectToUse], gameObject.transform.position, Quaternion.identity);
            thingy.transform.parent = gameObject.transform;
            thingy.transform.localPosition = randomPosition;


            //add object to list of instantiated objects
            //inScene[i] = thingy;

            
        

        // objects can be the same multiple times or mix and match

        

        

        // if there is already an object there, put object somewhere else

        // check if object is there by saving the x and y coords generated each time.
        // when placing new object, compare candidate x and y coords with coords already used
        //if both x or y are the same (or within overlap range), pick new x and y values

       
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
