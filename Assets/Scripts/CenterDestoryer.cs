using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDestoryer : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){



        if(other.CompareTag("RoomCenter")) {

            Debug.Log(gameObject.transform.root.gameObject);

            Destroy(gameObject.transform.root.gameObject);

        }

    }
}
