using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Item : MonoBehaviour
{

    private Animator m_Animator;
    private AudioSource m_AudioSource;
    public AudioClip lightToDark;
    public AudioClip darkToLight;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
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
        else if (other.CompareTag("Plate"))
        {
            m_Animator.ResetTrigger("Activation");
            other.GetComponent<Plate>().SetPlateState(false);
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

            //m1Tilt
            var m1Tilt = mirror.transform.rotation.eulerAngles.z;
            //Debug.Log("M1 Tilt: " + m1Tilt); //60
            //velocityAngle
            var velocityAngleQ = Quaternion.FromToRotation(Vector3.up, new Vector3(previousVelocity.x, previousVelocity.y));
            //Debug.Log("Velocity Angle: " + velocityAngleQ.eulerAngles.z); 
            //alpha (Einfallswinkel = velocityAngle - m1 rotation)
            var alpha = velocityAngleQ.eulerAngles.z - m1Tilt;
            if (alpha > 90)
                alpha = 180 - alpha;
            if (alpha < 0)
                alpha += 360;
            //Debug.Log("Einfallswinkel: " + alpha);
            //m2Tilt
            var m2Tilt = mirror.Other.transform.rotation.eulerAngles.z;
            //Debug.Log("M2 Tilt: " + m2Tilt);
            //target
            var target = m2Tilt - alpha;
            //Debug.Log("target angle: " + target);
            //diff
            var diff = target - velocityAngleQ.eulerAngles.z;
            var diffQ = Quaternion.RotateTowards( velocityAngleQ, Quaternion.Euler(0, 0, target), 360);

            //Debug.Log("Diff to rotate: " + diff);
            //Debug.DrawRay(mirror.Other.transform.position, Quaternion.Euler(0,0,diff) * new Vector3(previousVelocity.x, previousVelocity.y), Color.yellow, 100);
            
            //sin alpha * h = d
            Debug.Log(3  * mirror.Other.GetComponent<BoxCollider2D>().bounds.extents.magnitude + " " + (1 / Math.Sin(Mathf.Deg2Rad * alpha)));
            var length = Mathf.Min((float) (1 / Math.Sin(Mathf.Deg2Rad * alpha)), 3  * mirror.Other.GetComponent<BoxCollider2D>().bounds.extents.magnitude) ;
            var outVector = Quaternion.Euler(0,0,diff) * new Vector3(previousVelocity.x, previousVelocity.y).normalized * length;
            //Debug.DrawRay(mirror.transform.position, new Vector3(previousVelocity.x, previousVelocity.y), Color.black, 100);
            Debug.DrawRay(mirror.Other.transform.position, outVector, Color.white, 100);
            Debug.Log(outVector);
            //Debug.Log("t");
            m_AudioSource.PlayOneShot(rigidbody.gravityScale > 0? lightToDark : darkToLight);
            transform.SetPositionAndRotation(mirror.Other.transform.position + outVector, transform.rotation);
            rigidbody.gravityScale *= -1;
            rigidbody.velocity = Quaternion.Euler(0, 0, target) * rigidbody.velocity;
            //Debug.Log("v: " + rigidbody.velocity);
        }

        if (other.CompareTag("Plate"))
        {
            m_Animator.SetTrigger("Activation");
            other.GetComponent<Plate>().SetPlateState(true);
        }
    }
}
