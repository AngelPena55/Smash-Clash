using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothSpeed = 0.125f;

        public Vector3 offset;



        private void FixedUpdate()
        {
            Vector3 desiredPositiion = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPositiion, smoothSpeed);
            transform.position = smoothedPosition;
            
        }
    }
}
