using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    

    //public SpawnedRoomsScript getBool;
    bool isColliding = false;
    //public bool doorsDestroyed = false;



    public GameObject inactive;
    



    void OnTriggerEnter2D(Collider2D other){



        //check if object colliding is in another room
        //basically if its parent is different from this gameObject

        //gameObject is the door, parent is DoorParent, parent^2  should be the room itself
        if (gameObject.transform.parent.parent != other.gameObject.transform.parent.parent)
            isColliding = true;


    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
 
        // Code to execute after the delay
        if (isColliding == false){



            //get door name
            // delete diabled duplicate object

            inactive.SetActive(true);

            //door locking script disables the locked door (inactive)
            //so just instantiate a new copy that won't be disabled
            GameObject duplicate = Instantiate(inactive);
            duplicate.transform.position = inactive.transform.position;
            duplicate.tag = "Locked";
            duplicate.transform.parent = inactive.transform.parent.parent;
            
            inactive.SetActive(false);
            gameObject.SetActive(false);
            
            Debug.Log("doors destroyed");
            //disable door object
            //gameObject.SetActive(false);


            //enable wall in its place

            //compare name of door being disabled
            //if bottom door, enable bottom wall
            //etc for other sides
            //again
            //will need to get the game object with certain name to enable correct wall

            //TODO SetActive(true);

        }
        
    }
        
    void Start(){


        StartCoroutine(ExecuteAfterTime(3));

    }

    
}
