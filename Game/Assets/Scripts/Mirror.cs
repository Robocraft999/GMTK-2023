using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Mirror Other;
    public LayerMask ItemMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.collider + " " + col.gameObject.layer);
        var layerMaskConatinsLayer = ItemMask == (ItemMask | (1 << col.gameObject.layer));
        if (layerMaskConatinsLayer)
        {
            col.collider.transform.SetPositionAndRotation(Other.transform.position + 2*col.collider.bounds.extents * Math.Sign(col.relativeVelocity.x), col.collider.transform.rotation);
            col.rigidbody.gravityScale *= -1;
        }
            
    }
}
