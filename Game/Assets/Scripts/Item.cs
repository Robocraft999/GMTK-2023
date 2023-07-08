using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player.ItemTriggered == this)
                player.ItemTriggered = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (!player.ItemTriggered)
                player.ItemTriggered = this;
        }
        if (other.CompareTag("Mirror"))
        {
            var mirror = other.GetComponent<Mirror>();
            var rigidbody = GetComponent<Rigidbody2D>();
            var previousVelocity = rigidbody.velocity;

            /*
            //red (m1 rotation)
            var red = mirror.transform.rotation.eulerAngles.z;
            Debug.Log(red); //60
            //green
            var green = 90 - red;
            //yellow
            var yellow = Quaternion.FromToRotation(Vector3.up, new Vector3(previousVelocity.x, previousVelocity.y));
            Debug.Log(yellow.eulerAngles.z); //0.745
            //blue
            var blue = red + 90 - yellow.eulerAngles.z;
            Debug.Log(blue); //149.25
            //m2 rotation
            var pink = mirror.Other.transform.rotation.eulerAngles.z;
            Debug.Log(pink); //180
            //target angle
            var target = pink + green;
            Debug.Log(target); //210
            //diff
            var diff = Quaternion.Angle(yellow, quaternion.Euler(0, 0, target));
            var diffQ = Quaternion.RotateTowards(yellow, quaternion.Euler(0, 0, target), 360);*/
            
            //m1Tilt
            var m1Tilt = mirror.transform.rotation.eulerAngles.z;
            Debug.Log("M1 Tilt: " + m1Tilt); //60
            //velocityAngle
            var velocityAngleQ = Quaternion.FromToRotation(Vector3.up, new Vector3(previousVelocity.x, previousVelocity.y));
            Debug.Log("Velocity Angle: " + velocityAngleQ.eulerAngles.z); 
            //alpha (Einfallswinkel = velocityAngle - m1 rotation)
            var alpha = velocityAngleQ.eulerAngles.z - m1Tilt;
            if (alpha > 90)
                alpha = 180 - alpha;
            if (alpha < 0)
                alpha += 360;
            Debug.Log("Einfallswinkel: " + alpha);
            //m2Tilt
            var m2Tilt = mirror.Other.transform.rotation.eulerAngles.z;
            Debug.Log("M2 Tilt: " + m2Tilt);
            //target
            var target = m2Tilt - alpha;
            Debug.Log("target angle: " + target);
            //diff
            var diff = target - velocityAngleQ.eulerAngles.z;//Quaternion.Angle(velocityAngleQ, quaternion.Euler(0, 0, target));
            var diffQ = Quaternion.RotateTowards( velocityAngleQ, Quaternion.Euler(0, 0, target), 360);

            Debug.Log("Diff to rotate: " + diff); //151.36
            Debug.DrawRay(mirror.Other.transform.position, Quaternion.Euler(0,0,diff) * new Vector3(previousVelocity.x, previousVelocity.y), Color.yellow, 100);
            
            //var outVector = diffQ * new Vector3(previousVelocity.x, previousVelocity.y).normalized * 3f;
            //sin alpha * h = d
            var outVector = Quaternion.Euler(0,0,diff) * new Vector3(previousVelocity.x, previousVelocity.y).normalized * (float)(1.5 / Math.Sin(alpha));
            Debug.DrawRay(mirror.transform.position, new Vector3(previousVelocity.x, previousVelocity.y), Color.black, 100);
            Debug.DrawRay(mirror.Other.transform.position, outVector, Color.white, 100);
            Debug.Log("t");
            transform.SetPositionAndRotation(mirror.Other.transform.position + outVector, transform.rotation);
            rigidbody.gravityScale *= -1;
            rigidbody.velocity = Quaternion.Euler(0, 0, target) * rigidbody.velocity;
            Debug.Log("v: " + rigidbody.velocity);

            //var angle2 = Quaternion.Angle(mirror.transform.rotation, mirror.Other.transform.rotation);
            //Debug.Log(mirror.Other.transform.rotation);
            //Debug.Log(angle2);
            //Debug.DrawRay(mirror.Other.transform.position, Quaternion.Euler(0, 0, angle2) * new Vector3(previousVelocity.x, previousVelocity.y), Color.cyan, 100);
            //Debug.Log(Quaternion.AngleAxis(Quaternion.Angle(Quaternion.FromToRotation(Vector3.right, previousVelocity), mirror.Other.transform.rotation), Vector3.forward));
            //Debug.DrawRay(mirror.transform.position, mirror.transform.right - new Vector3(previousVelocity.x, previousVelocity.y), Color.yellow, 100);
            //Debug.DrawRay(mirror.Other.transform.position, mirror.transform.right - new Vector3(previousVelocity.x, previousVelocity.y) - mirror.Other.transform.right, Color.yellow, 100);
            //Debug.DrawRay(mirror.transform.position,  mirror.transform.rotation * new Vector3(previousVelocity.x, previousVelocity.y), Color.cyan, 100);
            //Debug.DrawRay(mirror.Other.transform.position,  mirror.Other.transform.rotation * new Vector3(previousVelocity.x, previousVelocity.y), Color.cyan, 100);
            //Debug.DrawRay(mirror.Other.transform.position,  mirror.Other.transform.rotation * new Vector3(previousVelocity.x, previousVelocity.y), Color.cyan, 100);
            //Debug.DrawRay(mirror.Other.transform.position,  (Quaternion.RotateTowards(mirror.transform.rotation, mirror.Other.transform.rotation, 360)) * new Vector3(previousVelocity.x, previousVelocity.y), Color.magenta, 100);
            /*var outVector = (Quaternion.Euler(0, 0, angle2) * new Vector3(previousVelocity.x, previousVelocity.y)).normalized * 3f;
            Debug.DrawRay(mirror.transform.position, new Vector3(previousVelocity.x, previousVelocity.y), Color.black, 100);
            Debug.DrawRay(mirror.Other.transform.position, outVector, Color.white, 100);
            Debug.Log("t");
            transform.SetPositionAndRotation(mirror.Other.transform.position + outVector, transform.rotation);
            rigidbody.gravityScale *= -1;*/
        }
    }
}
