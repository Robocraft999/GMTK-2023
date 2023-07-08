using System;
using System.Collections;
using System.Collections.Generic;
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
            var newPosition = mirror.Other.transform.position + 2 * GetComponent<Collider2D>().bounds.extents * Math.Sign(previousVelocity.x);
            transform.SetPositionAndRotation(newPosition, transform.rotation);
            //rigidbody.velocity = previousVelocity;
            rigidbody.gravityScale *= -1;
        }
    }
}
