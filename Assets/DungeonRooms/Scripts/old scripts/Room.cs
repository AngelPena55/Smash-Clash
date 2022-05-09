using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int X, Y;


    // Start is called before the first frame update
    void Start()
    {
        
        if(RoomController.instance == null) {
            Debug.Log("Wrong scene");
            return;
        }

        RoomController.instance.RegisterRoom(this);

    }//end start

    // Update is called once per frame
    void Update()
    {
        
    }//end update


    void OnDrawGizmos(){

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));

    }

    public Vector3 GetRoomCenter(){

        return new Vector3(X * Width, Y * Height);

    }


}//end room
