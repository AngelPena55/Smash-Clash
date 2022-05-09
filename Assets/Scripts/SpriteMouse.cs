using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMouse : MonoBehaviour
{
    Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

     void faceMouse()
    {


       // Vector3 mousePosition = Input.mousePosition;
        //mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector2 direction = new Vector2(
       // mousePosition.x - transform.position.x,
       // mousePosition.y - transform.position.y);

       // transform.up = direction;
    }
}
