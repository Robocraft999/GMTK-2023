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

    private Mirror nextTarget;

    private const float AngleThreshold = 10;
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
        }else if (other.CompareTag("Mirror"))
        {
            var mirror = other.GetComponent<Mirror>();
            if (nextTarget == mirror)
                nextTarget = null;
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
        if (other.CompareTag("Plate"))
        {
            m_Animator.SetTrigger("Activation");
            other.GetComponent<Plate>().SetPlateState(true);
        }
        if (other.CompareTag("Mirror"))
        {
            var mirror = other.GetComponent<Mirror>();
            if (nextTarget == mirror.Other)
                return;

            var rigidbody = GetComponent<Rigidbody2D>();
            var previousVelocity = rigidbody.velocity;

            //m1Tilt
            var m1Tilt = mirror.transform.rotation.eulerAngles.z;
            
            //velocityAngle
            var velocityAngleQ = Quaternion.FromToRotation(Vector3.up, new Vector3(previousVelocity.x, previousVelocity.y));
            
            //alpha (Einfallswinkel = velocityAngle - m1 rotation)
            var alpha = velocityAngleQ.eulerAngles.z - m1Tilt;
            if (alpha > 90)
                alpha = 180 - alpha;
            if (alpha < 0)
                alpha += 360;

            var m2Tilt = mirror.Other.transform.rotation.eulerAngles.z;

            float diff;
            float length;
            bool shouldTeleport;

            if (MenuSceneUIManager.Instance && MenuSceneUIManager.Instance.UseSafePortals)
            {
                shouldTeleport = true;
                
                var target = m2Tilt - 90; //front
                diff = target - velocityAngleQ.eulerAngles.z;

                length = 0.8f + mirror.Other.GetComponent<BoxCollider2D>().bounds.extents.x;
            }
            else
            {
                var beta = Math.Abs(alpha > 180 ? alpha - 180 : alpha);
                shouldTeleport = beta > AngleThreshold && beta < 180 - AngleThreshold;
                
                var target = m2Tilt - alpha;
                diff = target - velocityAngleQ.eulerAngles.z;
            
                //sin alpha * h = d
                //var length = Mathf.Min((float) (1 / Math.Sin(Mathf.Deg2Rad * alpha)), 3 * mirror.Other.GetComponent<BoxCollider2D>().bounds.extents.magnitude);
                length = Mathf.Min(1f + mirror.Other.GetComponent<BoxCollider2D>().bounds.extents.magnitude, Mathf.Abs(1 / Mathf.Sin(Mathf.Deg2Rad * alpha)));
            }
            var outVector = Quaternion.Euler(0,0,diff) * new Vector3(previousVelocity.x, previousVelocity.y).normalized * length;

            Debug.DrawRay(mirror.transform.position, new Vector3(previousVelocity.x, previousVelocity.y), Color.black, 100);
            Debug.DrawRay(mirror.Other.transform.position, outVector, Color.white, 100);

            if (!shouldTeleport)
            {
                nextTarget = null;
                return;
            }
            
            nextTarget = mirror.Other;
            
            m_AudioSource.PlayOneShot(rigidbody.gravityScale > 0? lightToDark : darkToLight);
            rigidbody.gravityScale *= -1;
            //Debug.Log("t");
            rigidbody.velocity = Quaternion.Euler(0, 0, diff) * previousVelocity;
            transform.SetPositionAndRotation(mirror.Other.transform.position + outVector, transform.rotation);
        }
    }
}
