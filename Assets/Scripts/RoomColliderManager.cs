using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomColliderManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with "+other.gameObject.name);
        //GetComponentInParent<EnemyCounter>().DoorLockMechanism(other);
    }
 }
